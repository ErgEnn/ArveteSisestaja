using OpenQA.Selenium;

namespace ArveteSisestajaCore.Omniva;

public class InvoicePage
{
    public static By LoadingSpinner = By.Id("aranea-loading-message");
    public static By SecondAttachmentTab = By.XPath("//a[contains(.,'Manus 2')]");
    public static By SecondAttachmentIFrame = By.ClassName("txtFile");
    public static By InvoiceText = By.XPath("/html/body/pre");

    public static By InvoiceNo =
        By.CssSelector(
            "#m\\.f0\\.root\\.menu\\.f1\\.tabContainer\\.invoice\\.tcw\\.invoicePartySellerFormRegion > div > div.col2 > table > tbody > tr:nth-child(2) > td > span > span");
    
    public static By InvoicePDF = By.CssSelector("#embedpdf_1");

    public static By InvoiceDate = By.CssSelector("#m\\.f0\\.root\\.menu\\.f1\\.tabContainer\\.invoice\\.tcw\\.invoicePartySellerFormRegion > div > div.col2 > table > tbody > tr:nth-child(4) > td > span > span");
    public static By InvoiceSellerParty = By.CssSelector("#m\\.f0\\.root\\.menu\\.f1\\.tabContainer\\.invoice\\.tcw\\.invoicePartySellerFormRegion > div > div.col1 > table > tbody > tr:nth-child(1) > td > span > span");
    public static By NextInvoiceButton = By.XPath("//*[@id=\"tabsRegion\"]/ul/div/p/input");
    public static By NextInvoiceButtonAlt = By.XPath("//*[@id=\"tabsRegion\"]/ul/div/p/input");
}