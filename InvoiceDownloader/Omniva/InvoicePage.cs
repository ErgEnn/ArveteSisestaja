using OpenQA.Selenium;

namespace InvoiceDownloader.Omniva;

public class InvoicePage
{
    public static By LoadingSpinner = By.Id("aranea-loading-message");
    public static By FirstAttachmentTab = By.XPath("//a[contains(.,'Manus 1')]");
    public static By SecondAttachmentTab = By.XPath("//a[contains(.,'Manus 2')]");
    public static By SecondAttachmentIFrame = By.ClassName("txtFile");
    public static By InvoiceText = By.XPath("/html/body/pre");
    public static By InvoiceMetadataCollapse = By.XPath("//a[contains(.,'Arve info')]");
    public static By InvoiceNo = By.XPath("//*//label[contains(., 'Arve nr:')]/../following-sibling::td");
    public static By InvoicePDF = By.CssSelector("embed");
    public static By InvoiceDate = By.XPath("//*//label[contains(., 'Arve kuupäev:')]/../following-sibling::td");
    public static By InvoiceSender = By.XPath("//*//label[contains(., 'Arve saatja:')]/../following-sibling::td");
    public static By NextInvoiceButton = By.XPath("//*//input[@arn-evntid='nextListItem']");
}