using OpenQA.Selenium;

namespace InvoiceDownloader.Omniva;

public class InvoiceSearchComponent
{
    public static By InvoicesSubmenuLink = By.CssSelector("#eak-nav > ul > li:nth-child(2) > div > a");
    public static By InvoiceArrivalDateStartDatepicker = By.Id("m.f0.root.menu.f0.list.form.arrivalDate_start");
    public static By InvoiceArrivalDateEndDatepicker = By.Id("m.f0.root.menu.f0.list.form.arrivalDate_end");
    public static By InvoiceDateFromDatepicker = By.Id("m.f0.root.menu.f0.list.form.invoiceDate_start");
    public static By InvoiceDateToDatepicker = By.Id("m.f0.root.menu.f0.list.form.invoiceDate_end");
    public static By InvoiceStatePicker = By.Id("m.f0.root.menu.f0.list.form.state");
    public static By Submit = By.Id("fe-span-m.f0.root.menu.f0.list.form.filter");
}