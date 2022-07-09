using OpenQA.Selenium;

namespace ArveteSisestajaCore.Omniva;

public class InvoicePage
{
    public static By LoadingSpinner = By.Id("aranea-loading-message");
    public static By SecondAttachmentTab = By.XPath("//a[contains(.,'Manus 2')]");
    public static By SecondAttachmentIFrame = By.ClassName("txtFile");
    public static By InvoiceText = By.XPath("/html/body/pre");
    public static By NextInvoiceButton = By.XPath("//*[@id=\"tabsRegion\"]/ul/div/p/input");
    public static By NextInvoiceButtonAlt = By.XPath("//*[@id=\"tabsRegion\"]/ul/div/p/input");
}