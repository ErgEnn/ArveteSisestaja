using System.Globalization;
using ArveteSisestajaCore.Omniva;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using Keys = OpenQA.Selenium.Keys;

namespace ArveteSisestajaCore
{

    public class OmnivaHandler
    {

        public async Task<IReadOnlyCollection<OmnivaInvoiceExport>> LoadInvoices(DateTime from, DateTime to)
        {
            return await Task.Run(() =>
            {
                ChromeDriver chromeDriver = null;
                WebDriverWait wait;

                void InitChrome()
                {
                    ChromeOptions co = new ChromeOptions();
                    co.BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe";
                    co.AddArgument("--disable-gpu");
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
                    wait.Until(ExpectedConditions.ElementExists(By.Id("username")));
                    chromeDriver.FindElement(LoginPage.Username).SendKeys(SettingsHandler.GetSetting(SettingsHandler.SETTING.OMNIVA_USERNAME)); // Enter username
                    chromeDriver.FindElement(LoginPage.Password).SendKeys(SettingsHandler.GetSetting(SettingsHandler.SETTING.OMNIVA_PASSWORD)); // Enter password
                    chromeDriver.FindElement(LoginPage.Password).SendKeys(Keys.Enter);
                }

                void OpenInvoicesView()
                {
                    wait.Until(ExpectedConditions.ElementToBeClickable(InvoiceSearchComponent.InvoicesSubmenuLink));
                    chromeDriver.FindElement(InvoiceSearchComponent.InvoicesSubmenuLink).Click();
                    chromeDriver.Navigate().GoToUrl("https://finance.omniva.eu/finance/main");
                }

                void FillSearchForm()
                {
                    wait.Until(ExpectedConditions.ElementToBeClickable(InvoiceSearchComponent.InvoiceArrivalDateStartDatepicker));
                    chromeDriver.FindElement(InvoiceSearchComponent.InvoiceArrivalDateStartDatepicker).Click();
                    chromeDriver.FindElement(InvoiceSearchComponent.InvoiceArrivalDateStartDatepicker).SendKeys(Keys.Backspace);
                    chromeDriver.FindElement(InvoiceSearchComponent.InvoiceArrivalDateEndDatepicker).Click();
                    chromeDriver.FindElement(InvoiceSearchComponent.InvoiceArrivalDateEndDatepicker).SendKeys(Keys.Backspace);
                    chromeDriver.FindElement(InvoiceSearchComponent.InvoiceDateFromDatepicker).Click();
                    chromeDriver.FindElement(InvoiceSearchComponent.InvoiceDateFromDatepicker).SendKeys(Keys.Home + from.ToString("ddMMyyyy"));
                    chromeDriver.FindElement(InvoiceSearchComponent.InvoiceDateToDatepicker).Click();
                    chromeDriver.FindElement(InvoiceSearchComponent.InvoiceDateToDatepicker).SendKeys(Keys.Home + to.ToString("ddMMyyyy"));
                    chromeDriver.FindElement(InvoiceSearchComponent.InvoiceStatePicker).SendKeys("Näita");
                    wait.Until(ExpectedConditions.ElementToBeClickable(InvoiceSearchComponent.Submit));
                    chromeDriver.FindElement(InvoiceSearchComponent.Submit).Click();// Submit
                }

                void WaitLoading()
                {
                    wait.Until(ExpectedConditions.ElementExists(InvoiceSearchPage.LoadingSpinner));
                    wait.Until(ExpectedConditions.ElementIsVisible(InvoiceSearchPage.LoadingSpinner));
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(InvoiceSearchPage.LoadingSpinner));
                    wait.Until(ExpectedConditions.ElementToBeClickable(InvoiceSearchPage.FirstResultItem));
                }

                int GetInvoicesCount()
                {
                    string amountStr = chromeDriver.FindElement(InvoiceSearchPage.TotalResultsCount).Text.Split(new[] { " | " }, StringSplitOptions.None)[1].Split(' ')[1]; //Get amount of invoices
                    return int.Parse(amountStr);
                }

                void NavigateToFirstInvoice()
                {
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
                    WaitLoading();
                    NavigateToFirstInvoice();

                    List<OmnivaInvoiceExport> invoices = new List<OmnivaInvoiceExport>();
                    for (int i = 0; i < amount; i++)
                    {

                        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(InvoicePage.LoadingSpinner));
                        chromeDriver.FindElement(InvoicePage.SecondAttachmentTab).Click();
                        wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(InvoicePage.SecondAttachmentIFrame));

                        try
                        {
                            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(InvoicePage.LoadingSpinner));

                            var invoiceNo = InvoicePage.InvoiceNo.FindElement(chromeDriver)?.Text.Trim();
                            var invoiceDate = (InvoicePage.InvoiceDate.FindElement(chromeDriver)?.Text
                                .Trim() as IConvertible)?.ToDateTime(new DateTimeFormatInfo(){ShortDatePattern = "dd.MM.yyyy"});
                            var invoiceSellerParty = InvoicePage.InvoiceSellerParty.FindElement(chromeDriver)?.Text.Trim();
                            var pdfSrc = InvoicePage.InvoicePDF.FindElement(chromeDriver)?.GetAttribute("src");

                            chromeDriver.FindElement(InvoicePage.SecondAttachmentTab).Click();
                            wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(InvoicePage.SecondAttachmentIFrame));
                            try
                            {
                                var xml = (string)chromeDriver.ExecuteScript("return document.body.innerText");
                                invoices.Add(new XMLOmnivaInvoice(xml));
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                invoices.Add(new FailedOmnivaInvoice()
                                {
                                    ScreenshotB64 = chromeDriver.GetScreenshot().AsBase64EncodedString,
                                    InvoiceNo = invoiceNo,
                                    InvoiceDate = invoiceDate,
                                    InvoiceSellerParty = invoiceSellerParty,
                                    PdfSrc = pdfSrc,
                                });
                                Console.WriteLine("Ei suutnud laadida arvet.");
                            }
                            chromeDriver.SwitchTo().ParentFrame();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            invoices.Add(new FailedOmnivaInvoice()
                            {
                                ScreenshotB64 = chromeDriver.GetScreenshot().AsBase64EncodedString
                            });
                            Console.WriteLine("Ei suutnud laadida arvet.");
                        }


                        wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("aranea-loading-message")));
                        try
                        {
                            if (chromeDriver.FindElement(InvoicePage.NextInvoiceButton).GetAttribute("value") ==
                                "Järgmine")
                                chromeDriver.FindElement(InvoicePage.NextInvoiceButton).Click(); // Button Järgmine
                            else
                            {
                                chromeDriver.FindElement(InvoicePage.NextInvoiceButtonAlt).Click(); // Button Järgmine
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Järgmise arve nuppu ei leitud!");
                            throw;
                        }
                    }

                    Console.WriteLine(invoices.Count);
                    chromeDriver.Close();
                    return invoices;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Viga OMNIVAst arvete laadimisel.\nProovi uuesti!");
                    try { chromeDriver.Close(); } catch (Exception) { }
                }

                return new List<OmnivaInvoiceExport>();
            });

        }
    }

    public abstract class OmnivaInvoiceExport
    {

    }

    public class XMLOmnivaInvoice : OmnivaInvoiceExport
    {
        public XMLOmnivaInvoice(string rawXml)
        {
            RawXML = rawXml;
        }

        public string RawXML { get; init; }
    }

    public class FailedOmnivaInvoice : OmnivaInvoiceExport
    {
        public string ScreenshotB64 { get; set; }
        public string InvoiceSellerParty { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }
        public string PdfSrc { get; set; }
    }
}
