using System.Net;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Text.RegularExpressions;
using InvoiceDownloader.Helpers;
using InvoiceDownloader.Omniva;
using Cookie = System.Net.Cookie;
using System.Xml.Serialization;
using System.Xml;
using EInvoice;

namespace InvoiceDownloader;

public class Downloader((string username, string password) credentials, bool headless = true, int timeoutsInSec = 10)
{
    public async Task<IReadOnlyCollection<Invoice>> DownloadInvoices(DateOnly from, DateOnly to, IProgress<(int progress,int total)> progress)
    {
        return await Task.Run(async () =>
        {
            ChromeDriver chromeDriver = null;
            WebDriverWait wait;
            CookieContainer cookieContainer = new CookieContainer();
            HttpClient httpClient = new HttpClient(new HttpClientHandler(){CookieContainer = cookieContainer});
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/123.0.0.0 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");

            void InitChrome()
            {
                ChromeOptions co = new ChromeOptions
                {
                    BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"
                };
                co.AddArgument("--disable-gpu");
                if(headless)
                    co.AddArgument("--headless=new");
                co.SetLoggingPreference(LogType.Driver, LogLevel.Off);
                co.SetLoggingPreference(LogType.Browser, LogLevel.Off);
                chromeDriver = new ChromeDriver(co);
                chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeoutsInSec);
                wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(timeoutsInSec));
            }

            void NavigateTo(string url)
            {
                chromeDriver.Navigate().GoToUrl(url);
            }

            void LogIn()
            {
                wait.Until(ExpectedConditions.ElementExists(()=>LoginPage.UserPassAuthMethodTab));
                chromeDriver.FindElement(LoginPage.UserPassAuthMethodTab).Click();
                wait.Until(ExpectedConditions.ElementExists(()=>LoginPage.Username));
                chromeDriver.FindElement(LoginPage.Username).SendKeys(credentials.username); // Enter username
                chromeDriver.FindElement(LoginPage.Password).SendKeys(credentials.password); // Enter password
                chromeDriver.FindElement(LoginPage.Password).SendKeys(Keys.Enter);
            }

            void OpenInvoicesView()
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(() => InvoiceSearchComponent.InvoicesSubmenuLink));
                chromeDriver.FindElement(InvoiceSearchComponent.InvoicesSubmenuLink).Click();
                Thread.Sleep(500);
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
                progress.Report((0,amount));
                Console.WriteLine("Arveid: " + amount);
                NavigateToFirstInvoice();

                List<Invoice> invoices = new (amount);
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
                        var pdfSrc = InvoicePage.InvoiceAttachmentEmbed.FindElement(chromeDriver)!.GetAttribute("src");

                        foreach (var cookie in chromeDriver.Manage().Cookies.AllCookies)
                        {
                            cookieContainer.Add(new Uri(pdfSrc),new Cookie(cookie.Name, cookie.Value));
                        }

                        byte[] pdfBytes = await httpClient.GetByteArrayAsync(pdfSrc);

                        var secondAttachment = chromeDriver.FindElement(InvoicePage.SecondAttachmentTab);
                        string xml = null;
                        if (secondAttachment != null)
                        {
                            secondAttachment.Click();
                            Thread.Sleep(250);
                            try
                            {
                                var xmlSrc =
                                    InvoicePage.InvoiceAttachmentIframe.FindElement(chromeDriver)!.GetAttribute("src");
                                xml = await httpClient.GetStringAsync(xmlSrc);

                                var reader = XmlReader.Create(xml.ToStream(),
                                    new XmlReaderSettings() {ConformanceLevel = ConformanceLevel.Document});
                                var einvoice = new XmlSerializer(typeof(E_Invoice)).Deserialize(reader) as E_Invoice;
                                var innerInvoice = einvoice.Invoice.Single();
                                if (innerInvoice.InvoiceInformation.InvoiceNumber != invoiceNo)
                                {
                                    invoiceNo = InvoicePage.InvoiceNo.SafeExecuteOnElement(chromeDriver, element => element.Text).Trim();
                                    if (innerInvoice.InvoiceInformation.InvoiceNumber != invoiceNo)
                                    {
                                        if (string.IsNullOrWhiteSpace(invoiceNo))
                                            invoiceNo = innerInvoice.InvoiceInformation.InvoiceNumber;
                                        else
                                            throw new Exception($"InvoiceNo Mismatch {innerInvoice.InvoiceInformation.InvoiceNumber}!={invoiceNo}");
                                    }
                                        
                                }

                                if (innerInvoice.InvoiceInformation.InvoiceDate !=
                                    invoiceDate.ToDateTime(TimeOnly.MinValue))
                                {
                                    invoiceDateStr = InvoicePage.InvoiceDate.FindElement(chromeDriver)!.Text.Trim();
                                    if (string.IsNullOrWhiteSpace(invoiceDateStr))
                                        invoiceDateStr =
                                            innerInvoice.InvoiceInformation.InvoiceDate.ToString("dd.MM.yyyy");
                                    invoiceDate = DateOnly.ParseExact(invoiceDateStr, "dd.MM.yyyy");
                                    if (innerInvoice.InvoiceInformation.InvoiceDate !=
                                        invoiceDate.ToDateTime(TimeOnly.MinValue))
                                        throw new Exception($"Date Mismatch {innerInvoice.InvoiceInformation.InvoiceDate} != {invoiceDate.ToDateTime(TimeOnly.MinValue)}");
                                }

                                if (innerInvoice.InvoiceParties.SellerParty.Name != invoiceSender)
                                {
                                    invoiceSender = InvoicePage.InvoiceSender.FindElement(chromeDriver)!.Text.Trim();
                                    if(innerInvoice.InvoiceParties.SellerParty.Name != invoiceSender)
                                        if (string.IsNullOrWhiteSpace(invoiceSender))
                                            invoiceSender = innerInvoice.InvoiceParties.SellerParty.Name;
                                        else
                                            throw new Exception("Seller Mismatch");
                                }
                                    
                            }
                            catch (Exception _)
                            {
                                xml = null;
                            }
                        }

                        invoices.Add(new Invoice(invoiceNo, invoiceSender, invoiceDate, xml, pdfBytes));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        Console.WriteLine("Ei suutnud laadida arvet.");
                    }
                    progress.Report((invoices.Count, amount));
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