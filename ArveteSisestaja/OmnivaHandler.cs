using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using HtmlAgilityPack;
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
				co.AddArgument("--disable-gpu");
				co.SetLoggingPreference(LogType.Driver, LogLevel.Off);
				co.SetLoggingPreference(LogType.Browser, LogLevel.Off);
				co.SetLoggingPreference(LogType.Client, LogLevel.Off);
				co.SetLoggingPreference(LogType.Profiler, LogLevel.Off);
				co.SetLoggingPreference(LogType.Server, LogLevel.Off);
				chromeDriver = new ChromeDriver(co);
				//chromeDriver.Manage().Window.Position = new System.Drawing.Point(0, -2000);
				var wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(15));
				chromeDriver.Navigate().GoToUrl("https://eservice.omniva.eu/epit/ui/finance"); //GOTO Login page
				wait.Until(ExpectedConditions.ElementExists(By.Id("username")));
				chromeDriver.FindElementById("username").SendKeys(SettingsHandler.GetSetting(SettingsHandler.SETTING.OMNIVA_USERNAME)); // Enter username
				chromeDriver.FindElementById("password").SendKeys(SettingsHandler.GetSetting(SettingsHandler.SETTING.OMNIVA_PASSWORD)); // Enter password
				chromeDriver.FindElementByXPath("/html/body/div/div[2]/div/form/div[5]/a").Click(); // Submit
				
				wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("/html/body/div[1]/div[4]/div[2]/div/div/div[1]/div/div[2]/eak-menu/div/div/ul/li[2]/a"))); //Wait for everything to load
				wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("loading-overlay")));
				chromeDriver.FindElementByXPath("/html/body/div[1]/div[4]/div[2]/div/div/div[1]/div/div[2]/eak-menu/div/div/ul/li[2]/a").Click(); //Select 'invoices'
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
				chromeDriver.FindElementByXPath("/html/body/div[2]/form/div[2]/div[2]/div[2]/div[2]/div/div/p[7]/span/select/option[1]").Click(); //Select to show all invoices
				chromeDriver.FindElementById("m.f0.root.menu.f0.list.form.filter").Click(); // Submit
				wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("aranea-loading-message")));
				string amountStr = chromeDriver.FindElementByXPath("/html/body/div[2]/form/div[2]/div[2]/div[2]/div[5]/div/p").Text.Split(new[] {" | "}, StringSplitOptions.None)[1].Split(' ')[1]; //Get amount of invoices
				int amount = Int32.Parse(amountStr);
				Console.WriteLine("Arveid: " + amount);
				chromeDriver.FindElementByXPath("/html/body/div[2]/form/div[2]/div[2]/div[2]/div[4]/table/tbody/tr[1]/td[3]/a[2]").Click(); // Select first entry in table
				List<Invoice> _invoices = new List<Invoice>();
				for (int i = 0; i < amount; i++) {

					wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id=\"invoiceAttachmentViewerBlockEl\"]/div/div[1]/div/ul/li[1]/a[1]"))); // Button Manus 1
					wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.Id("aranea-loading-message")));
					try {
						chromeDriver.FindElementByXPath("//*[@id=\"invoiceAttachmentViewerBlockEl\"]/div/div[1]/div/ul/li[2]/a[1]").Click(); // Select XML file
						wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.ClassName("txtFile")));
						_invoices.Add(new Invoice(chromeDriver.FindElement(By.XPath("/html/body/pre")).Text));
						chromeDriver.SwitchTo().ParentFrame();
					} catch (Exception e) {
						Console.WriteLine(e);
						try {
							chromeDriver.FindElementByXPath("//*[@id=\"invoiceAttachmentViewerBlockEl\"]/div/div[1]/div/ul/li[1]/a[2]").Click(); // Open attachment 1 in new window if possible
							
						} catch (Exception) { }
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
