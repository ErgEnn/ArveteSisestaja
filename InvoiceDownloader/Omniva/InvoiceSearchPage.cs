using OpenQA.Selenium;

namespace InvoiceDownloader.Omniva;

public class InvoiceSearchPage
{
    public static By LoadingSpinner = By.Id("aranea-loading-message");
    public static By FirstResultItem = By.XPath("//table/tbody/tr[1]/td[3]/a[2]");
    public static By TotalResultsCount = By.XPath("//div[@class='pager clear ']/p");
}