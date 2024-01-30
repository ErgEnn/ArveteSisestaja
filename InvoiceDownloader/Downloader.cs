using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Globalization;
using System.Text.RegularExpressions;
using InvoiceDownloader.Helpers;
using InvoiceDownloader.Omniva;

namespace InvoiceDownloader;

public class Downloader((string username, string password) credentials)
{
    public async Task<IReadOnlyCollection<Invoice>> DownloadInvoices(DateOnly from, DateOnly to)
    {
        return await Task.Run(() =>
        {
            ChromeDriver chromeDriver = null;
            WebDriverWait wait;

            void InitChrome()
            {
                ChromeOptions co = new ChromeOptions
                {
                    BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"
                };
                co.AddArgument("--disable-gpu");
                co.AddArgument("--headless=new");
                co.SetLoggingPreference(LogType.Driver, LogLevel.Off);
                co.SetLoggingPreference(LogType.Browser, LogLevel.Off);
                chromeDriver = new ChromeDriver(co);
                chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                wait = new WebDriverWait(chromeDriver, TimeSpan.FromHours(1));//.FromSeconds(15));
            }

            void NavigateTo(string url)
            {
                chromeDriver.Navigate().GoToUrl(url);
            }

            void LogIn()
            {
                wait.Until(ExpectedConditions.ElementExists(()=>LoginPage.Username));
                chromeDriver.FindElement(LoginPage.Username).SendKeys(credentials.username); // Enter username
                chromeDriver.FindElement(LoginPage.Password).SendKeys(credentials.password); // Enter password
                chromeDriver.FindElement(LoginPage.Password).SendKeys(Keys.Enter);
            }

            void OpenInvoicesView()
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(() => InvoiceSearchComponent.InvoicesSubmenuLink));
                chromeDriver.FindElement(InvoiceSearchComponent.InvoicesSubmenuLink).Click();
                chromeDriver.Navigate().GoToUrl("https://finance.omniva.eu/finance/main");
            }

            void FillSearchForm()
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(() => InvoiceSearchComponent.InvoiceArrivalDateStartDatepicker));
                chromeDriver.FindElement(InvoiceSearchComponent.InvoiceArrivalDateStartDatepicker).Click();
                chromeDriver.FindElement(InvoiceSearchComponent.InvoiceArrivalDateStartDatepicker).SendKeys(Keys.Backspace);
                chromeDriver.FindElement(InvoiceSearchComponent.InvoiceArrivalDateEndDatepicker).Click();
                chromeDriver.FindElement(InvoiceSearchComponent.InvoiceArrivalDateEndDatepicker).SendKeys(Keys.Backspace);
                chromeDriver.FindElement(InvoiceSearchComponent.InvoiceDateFromDatepicker).Click();
                chromeDriver.FindElement(InvoiceSearchComponent.InvoiceDateFromDatepicker).SendKeys(Keys.Home + from.ToString("ddMMyyyy"));
                chromeDriver.FindElement(InvoiceSearchComponent.InvoiceDateToDatepicker).Click();
                chromeDriver.FindElement(InvoiceSearchComponent.InvoiceDateToDatepicker).SendKeys(Keys.Home + to.ToString("ddMMyyyy"));
                chromeDriver.FindElement(InvoiceSearchComponent.InvoiceStatePicker).SendKeys("Näita");
                wait.Until(ExpectedConditions.ElementToBeClickable(() => InvoiceSearchComponent.Submit));
                chromeDriver.FindElement(InvoiceSearchComponent.Submit).Click();// Submit
            }

            void WaitLoading()
            {
                wait.Until(ExpectedConditions.ElementExists(()=>InvoiceSearchPage.LoadingSpinner));
                wait.Until(ExpectedConditions.ElementIsVisible(() => InvoiceSearchPage.LoadingSpinner));
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(() => InvoiceSearchPage.LoadingSpinner));
                wait.Until(ExpectedConditions.ElementToBeClickable(() => InvoiceSearchPage.FirstResultItem));
            }

            int GetInvoicesCount()
            {
                wait.Until(ExpectedConditions.ElementIsVisible(()=>InvoiceSearchPage.TotalResultsCount));
                string amountStr = chromeDriver.FindElement(InvoiceSearchPage.TotalResultsCount).Text; //Get amount of invoices
                amountStr = new Regex("Kokku (\\d+)").Match(amountStr).Groups[1].Value;
                return int.Parse(amountStr);
            }

            void NavigateToFirstInvoice()
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(() => InvoiceSearchPage.FirstResultItem));
                chromeDriver.FindElement(InvoiceSearchPage.FirstResultItem).Click();
            }

            try
            {

                InitChrome();
                if (chromeDriver == null) throw new NullReferenceException("Chrome driver init failed");

                NavigateTo("https://finance.omniva.eu/finance/ui/cost/receipts");
                LogIn();
                OpenInvoicesView();
                FillSearchForm();
                WaitLoading();
                int amount = GetInvoicesCount();
                Console.WriteLine("Arveid: " + amount);
                NavigateToFirstInvoice();

                List<Invoice> invoices = new ();
                IWebElement? nextBtn;
                do
                {

                    try
                    {
                        wait.Until(ExpectedConditions.ElementToBeClickable(()=>InvoicePage.FirstAttachmentTab));
                        Thread.Sleep(500);
                        var invoiceNo = InvoicePage.InvoiceNo.FindElement(chromeDriver)!.Text.Trim();
                        if (string.IsNullOrWhiteSpace(invoiceNo))
                        {
                            InvoicePage.InvoiceMetadataCollapse.SafeExecuteOnElement(chromeDriver, element => element.Click());
                            Thread.Sleep(250);
                        }

                        wait.Until(ExpectedConditions.ElementIsVisible(() => InvoicePage.InvoiceNo));

                        invoiceNo = InvoicePage.InvoiceNo.SafeExecuteOnElement(chromeDriver, element => element.Text).Trim();
                        var invoiceDateStr = InvoicePage.InvoiceDate.FindElement(chromeDriver)!.Text.Trim();
                        var invoiceDate = DateOnly.ParseExact(invoiceDateStr, "dd.MM.yyyy");
                        var invoiceSender = InvoicePage.InvoiceSender.FindElement(chromeDriver)!.Text.Trim();
                        var pdfSrc = InvoicePage.InvoicePDF.FindElement(chromeDriver)!.GetAttribute("src");

                        var secondAttachment = chromeDriver.FindElement(InvoicePage.SecondAttachmentTab);
                        if (secondAttachment != null)
                        {
                            secondAttachment.Click();
                            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(() =>
                                InvoicePage.SecondAttachmentIFrame));
                            try
                            {
                                var xml = (string)chromeDriver.ExecuteScript("return document.body.innerText");
                                invoices.Add(new Invoice(invoiceNo, invoiceSender, invoiceDate, xml, pdfSrc));
                            }
                            catch (Exception e)
                            {
                                invoices.Add(new Invoice(invoiceNo, invoiceSender, invoiceDate, null, pdfSrc));
                                Console.WriteLine("Ei suutnud laadida arve XMLi.");
                                Console.WriteLine(e);
                            }

                            chromeDriver.SwitchTo().ParentFrame();
                        }
                        else
                        {
                            invoices.Add(new Invoice(invoiceNo, invoiceSender, invoiceDate, null, pdfSrc));
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine("Ei suutnud laadida arvet.");
                    }

                    while (true)
                    {
                        try
                        {
                            nextBtn = chromeDriver.FindElement(InvoicePage.NextInvoiceButton);
                            nextBtn?.Click();
                            break;
                        }
                        catch (StaleElementReferenceException){}
                        catch (NoSuchElementException)
                        {
                            nextBtn = null;
                            break;
                        }
                    }
                    
                } while (nextBtn != null);

                Console.WriteLine(invoices.Count);
                chromeDriver.Close();
                return invoices;
            }
            catch (Exception e)
            {
                try { chromeDriver?.Close(); } catch
                { }

                throw;
            }
        });

    }
}