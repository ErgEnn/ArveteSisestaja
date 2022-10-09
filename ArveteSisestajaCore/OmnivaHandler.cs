using System.ComponentModel;
using ArveteSisestajaCore.Omniva;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using Keys = OpenQA.Selenium.Keys;

namespace ArveteSisestajaCore {

	public class OmnivaHandler {

        public async Task<List<BLL.Entities.Invoice>> LoadInvoices(DateTime from, DateTime to)
        {
            return await Task.Run(() =>
            {
				ChromeDriver chromeDriver = null;
				try
				{
					ChromeOptions co = new ChromeOptions();
					co.BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe";
					co.AddArgument("--disable-gpu");
					co.SetLoggingPreference(LogType.Driver, LogLevel.Off);
					co.SetLoggingPreference(LogType.Browser, LogLevel.Off);
					chromeDriver = new ChromeDriver(co);
					//chromeDriver.Manage().Window.Position = new System.Drawing.Point(0, -2000);
					chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
					var wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(15));
					chromeDriver.Navigate().GoToUrl("https://eservice.omniva.eu/epit/ui/finance"); //GOTO Login page
					wait.Until(ExpectedConditions.ElementExists(By.Id("username")));
					chromeDriver.FindElement(LoginPage.Username).SendKeys(SettingsHandler.GetSetting(SettingsHandler.SETTING.OMNIVA_USERNAME)); // Enter username
					chromeDriver.FindElement(LoginPage.Password).SendKeys(SettingsHandler.GetSetting(SettingsHandler.SETTING.OMNIVA_PASSWORD)); // Enter password
					chromeDriver.FindElement(LoginPage.Password).SendKeys(Keys.Enter);
					//chromeDriver.FindElement(By.XPath("/html/body/div/div/div[2]/form/div[5]/div/a")).Click(); // Submit
					//By.XPath("/html/body/div[1]/div[4]/div[2]/div/div/div[1]/div/div[2]/eak-menu/div/div/ul/li[2]/a")
					//wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[4]/div[2]/div/div/div[1]/div/div[2]/eak-menu/div/div/ul/li[2]/a"))); //Wait for everything to load
					//wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("loading-overlay")));
					//chromeDriver.FindElementByXPath("/html/body/div[1]/div[4]/div[2]/div/div/div[1]/div/div[2]/eak-menu/div/div/ul/li[2]/a").Click(); //Select 'invoices'
					wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("app-iframe")));
					wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("m.f0.root.menu.f0.list.form.arrivalDate_start")));
					chromeDriver.FindElement(InvoiceSearchComponent.InvoiceArrivalDateStartDatepicker).Click();
					chromeDriver.FindElement(InvoiceSearchComponent.InvoiceArrivalDateStartDatepicker).SendKeys(Keys.Backspace); //Set arrival beginning date
					chromeDriver.FindElement(InvoiceSearchComponent.InvoiceArrivalDateEndDatepicker).Click();
					chromeDriver.FindElement(InvoiceSearchComponent.InvoiceArrivalDateEndDatepicker).SendKeys(Keys.Backspace); //Set arrival beginning date
					chromeDriver.FindElement(InvoiceSearchComponent.InvoiceDateFromDatepicker).Click();
					chromeDriver.FindElement(InvoiceSearchComponent.InvoiceDateFromDatepicker).SendKeys(Keys.Home + from.ToString("ddMMyyyy")); //Set beginning date
					chromeDriver.FindElement(InvoiceSearchComponent.InvoiceDateToDatepicker).Click();
					chromeDriver.FindElement(InvoiceSearchComponent.InvoiceDateToDatepicker).SendKeys(Keys.Home + to.ToString("ddMMyyyy")); //Set end date
					chromeDriver.FindElement(InvoiceSearchComponent.InvoiceStatePicker).SendKeys("Näita"); //Select to show all invoices
					wait.Until(ExpectedConditions.ElementToBeClickable(InvoiceSearchComponent.Submit));
					Console.WriteLine("Clickable");
					chromeDriver.FindElement(InvoiceSearchComponent.Submit).Click();// Submit

					wait.Until(ExpectedConditions.ElementExists(InvoiceSearchPage.LoadingSpinner));
					Console.WriteLine("1");
					wait.Until(ExpectedConditions.ElementIsVisible(InvoiceSearchPage.LoadingSpinner));
					Console.WriteLine("2");
					wait.Until(ExpectedConditions.InvisibilityOfElementLocated(InvoiceSearchPage.LoadingSpinner));
					wait.Until(ExpectedConditions.ElementToBeClickable(InvoiceSearchPage.FirstResultItem));
					string amountStr = chromeDriver.FindElement(InvoiceSearchPage.TotalResultsCount).Text.Split(new[] { " | " }, StringSplitOptions.None)[1].Split(' ')[1]; //Get amount of invoices
					int amount = Int32.Parse(amountStr);
					Console.WriteLine("Arveid: " + amount);
					chromeDriver.FindElement(InvoiceSearchPage.FirstResultItem).Click(); // Select first entry in table
					List<BLL.Entities.Invoice> _invoices = new List<BLL.Entities.Invoice>();
					for (int i = 0; i < amount; i++)
					{
						try
						{
							wait.Until(ExpectedConditions.InvisibilityOfElementLocated(InvoicePage.LoadingSpinner));
							chromeDriver.FindElement(InvoicePage.SecondAttachmentTab).Click();
							//chromeDriver.FindElementByXPath("//*[@id=\"invoiceAttachmentViewerBlockEl\"]/div/div[1]/div/ul/li[2]/a[1]").Click(); // Select XML file
							wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(InvoicePage.SecondAttachmentIFrame));
							string xml;
							try
							{
								//xml = chromeDriver.FindElement(InvoicePage.InvoiceText).Text;
								xml = (string)chromeDriver.ExecuteScript("return document.body.innerText");
								_invoices.Add(BLL.Entities.Invoice.FromEInvoiceXML(xml));
							}
							catch (Exception e)
							{
								Console.WriteLine(e);
								_invoices.Add(BLL.Entities.Invoice.FromRawData(null,null,DateOnly.MinValue,chromeDriver.GetScreenshot()));
								Console.WriteLine("Ei suutnud laadida arvet.");
							}
							chromeDriver.SwitchTo().ParentFrame();
						}
						catch (Exception e)
						{
							Console.WriteLine(e);
							_invoices.Add(BLL.Entities.Invoice.FromRawData(null, null, DateOnly.MinValue, chromeDriver.GetScreenshot()));
							Console.WriteLine("Ei suutnud laadida arvet.");
						}


						wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("aranea-loading-message")));
						try
						{
							if (chromeDriver.FindElement(InvoicePage.NextInvoiceButton).GetAttribute("value") == "Järgmine")
								chromeDriver.FindElement(InvoicePage.NextInvoiceButton).Click(); // Button Järgmine
							else
							{
								chromeDriver.FindElement(InvoicePage.NextInvoiceButtonAlt).Click(); // Button Järgmine
							}
						}
						catch (Exception) { }
					}
					Console.WriteLine(_invoices.Count);
					chromeDriver.Close();
					return _invoices;
				}
				catch (Exception e)
				{
					MessageBox.Show("Viga OMNIVAst arvete laadimisel.\nProovi uuesti!");
					try { chromeDriver.Close(); } catch (Exception) { }
                }

				return new List<BLL.Entities.Invoice>();
			});
			
        }
    }
}
