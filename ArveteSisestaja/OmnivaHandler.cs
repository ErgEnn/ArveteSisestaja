using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using HtmlAgilityPack;
using iTextSharp.text.log;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Keys = OpenQA.Selenium.Keys;
using MessageBox = System.Windows.MessageBox;

namespace ArveteSisestaja {

	public class OmnivaHandler {

		public BackgroundWorker OmnivaWorker;
		private DateTime _beginDate;
		private DateTime _endDate;

		public OmnivaHandler(DateTime beginDate, DateTime endDate) {
			this._beginDate = beginDate;
			this._endDate = endDate;
			OmnivaWorker = new BackgroundWorker();
			OmnivaWorker.DoWork += new DoWorkEventHandler(LoadInvoices);
			OmnivaWorker.WorkerReportsProgress = true;
		}

		public void LoadInvoices(object sender, DoWorkEventArgs args) {
			ChromeDriver chromeDriver=null;
			try {
				OmnivaWorker.ReportProgress(-1);
				ChromeOptions co = new ChromeOptions();
                co.BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe";
				co.AddArgument("--disable-gpu");
				co.SetLoggingPreference(LogType.Driver, LogLevel.Off);
				co.SetLoggingPreference(LogType.Browser, LogLevel.Off);
				chromeDriver = new ChromeDriver(co);
				//chromeDriver.Manage().Window.Position = new System.Drawing.Point(0, -2000);
				chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
				var wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(15));
				chromeDriver.Navigate().GoToUrl("https://finance.omniva.eu/finance/ui/cost/receipts"); //GOTO Login page
				wait.Until(ExpectedConditions.ElementExists(By.Id("username")));
				chromeDriver.FindElementById("username").SendKeys(SettingsHandler.GetSetting(SettingsHandler.SETTING.OMNIVA_USERNAME)); // Enter username
				chromeDriver.FindElementById("password").SendKeys(SettingsHandler.GetSetting(SettingsHandler.SETTING.OMNIVA_PASSWORD)); // Enter password
				chromeDriver.FindElementById("password").SendKeys(Keys.Enter);
				//chromeDriver.FindElement(By.XPath("/html/body/div/div/div[2]/form/div[5]/div/a")).Click(); // Submit
				//By.XPath("/html/body/div[1]/div[4]/div[2]/div/div/div[1]/div/div[2]/eak-menu/div/div/ul/li[2]/a")
				//wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[4]/div[2]/div/div/div[1]/div/div[2]/eak-menu/div/div/ul/li[2]/a"))); //Wait for everything to load
				//wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("loading-overlay")));
				//chromeDriver.FindElementByXPath("/html/body/div[1]/div[4]/div[2]/div/div/div[1]/div/div[2]/eak-menu/div/div/ul/li[2]/a").Click(); //Select 'invoices'
				wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.Id("app-iframe")));
				wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("m.f0.root.menu.f0.list.form.arrivalDate_start")));
				chromeDriver.FindElementById("m.f0.root.menu.f0.list.form.arrivalDate_start").Click();
				chromeDriver.FindElementById("m.f0.root.menu.f0.list.form.arrivalDate_start").SendKeys(Keys.Backspace); //Set arrival beginning date
				chromeDriver.FindElementById("m.f0.root.menu.f0.list.form.arrivalDate_end").Click();
				chromeDriver.FindElementById("m.f0.root.menu.f0.list.form.arrivalDate_end").SendKeys(Keys.Backspace); //Set arrival beginning date
				chromeDriver.FindElementById("m.f0.root.menu.f0.list.form.invoiceDate_start").Click();
				chromeDriver.FindElementById("m.f0.root.menu.f0.list.form.invoiceDate_start").SendKeys(Keys.Home + _beginDate.ToString("ddMMyyyy")); //Set beginning date
				chromeDriver.FindElementById("m.f0.root.menu.f0.list.form.invoiceDate_end").Click();
				chromeDriver.FindElementById("m.f0.root.menu.f0.list.form.invoiceDate_end").SendKeys(Keys.Home + _endDate.ToString("ddMMyyyy")); //Set end date
				chromeDriver.FindElementById("m.f0.root.menu.f0.list.form.state").SendKeys("Näita"); //Select to show all invoices
				wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("m.f0.root.menu.f0.list.form.filter")));
				Console.WriteLine("Clickable");
				chromeDriver.FindElementById("fe-span-m.f0.root.menu.f0.list.form.filter").Click();// Submit

				wait.Until(ExpectedConditions.ElementExists(By.Id("aranea-loading-message")));
				Console.WriteLine("1");
				wait.Until(ExpectedConditions.ElementIsVisible(By.Id("aranea-loading-message")));
				Console.WriteLine("2");
				wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("aranea-loading-message")));
				wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[2]/form/div[2]/div[2]/div[2]/div[4]/table/tbody/tr[1]/td[3]/a[2]")));
				string amountStr = chromeDriver.FindElementByXPath("/html/body/div[2]/form/div[2]/div[2]/div[2]/div[5]/div/p").Text.Split(new[] {" | "}, StringSplitOptions.None)[1].Split(' ')[1]; //Get amount of invoices
				int amount = Int32.Parse(amountStr);
				Console.WriteLine("Arveid: " + amount);
				chromeDriver.FindElementByXPath("/html/body/div[2]/form/div[2]/div[2]/div[2]/div[4]/table/tbody/tr[1]/td[3]/a[2]").Click(); // Select first entry in table
				List<Invoice> _invoices = new List<Invoice>();
				for (int i = 0; i < amount; i++) {
					//int retries = 0;
					//while (true) {
					//	try {
					//		wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("aranea-loading-message")));
					//		Console.WriteLine("hidden");
					//		chromeDriver.FindElementByXPath("//a[contains(.,'Manus 1')]")
					//		wait.Until(ExpectedConditions.ElementExists(By.XPath("//a[contains(.,'Manus 1')]"))); // Button Manus 1
					//		break;
					//	} catch (Exception e) {
					//		Console.WriteLine("Retrying");
					//		retries++;
					//		if (retries == 3) {
					//			_invoices.Add(new Invoice(chromeDriver.GetScreenshot()));
					//			break;
					//		}
					//	}
					//}
					try {
						wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("aranea-loading-message")));
						chromeDriver.FindElementByXPath("//a[contains(.,'Manus 2')]").Click();
						//chromeDriver.FindElementByXPath("//*[@id=\"invoiceAttachmentViewerBlockEl\"]/div/div[1]/div/ul/li[2]/a[1]").Click(); // Select XML file
						wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.ClassName("txtFile")));
						string xml;
						string test;
						try {
							xml = chromeDriver.FindElement(By.XPath("/html/body/pre")).Text;
							test = (string)chromeDriver.ExecuteScript("return document.body.innerText");
							_invoices.Add(new Invoice(test));
						} catch(Exception e) {
							Console.WriteLine(e);
							_invoices.Add(new Invoice(chromeDriver.GetScreenshot()));
							Console.WriteLine("Ei suutnud laadida arvet.");
						}
						chromeDriver.SwitchTo().ParentFrame();
					} catch (Exception e) {
						Console.WriteLine(e);
						_invoices.Add(new Invoice(chromeDriver.GetScreenshot()));
						Console.WriteLine("Ei suutnud laadida arvet.");
					}
					

					wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("aranea-loading-message")));
					try {
						if (chromeDriver.FindElementByXPath("//*[@id=\"tabsRegion\"]/ul/div/p/input").GetAttribute("value") == "Järgmine")
							chromeDriver.FindElementByXPath("//*[@id=\"tabsRegion\"]/ul/div/p/input").Click(); // Button Järgmine
						else {
							chromeDriver.FindElementByXPath("//*[@id=\"tabsRegion\"]/ul/div/p/input[2]").Click(); // Button Järgmine
						}
					} catch (Exception) { }
					OmnivaWorker.ReportProgress((int) ((double) ((double) i / (double) amount) * 100)); //Progress bar update report
				}
				Console.WriteLine(_invoices.Count);
				chromeDriver.Close();
				OmnivaWorker.ReportProgress(100);
				args.Result = _invoices;
			} catch (Exception e) {
				MessageBox.Show("Viga OMNIVAst arvete laadimisel.\nProovi uuesti!");
				try {chromeDriver.Close();} catch (Exception) {}
				args.Result = new List<Invoice>();
			}
		}

	}
}
