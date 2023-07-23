using OpenQA.Selenium;

namespace ArveteSisestajaCore.Omniva;

public class InvoiceSearchPage
{
    public static By LoadingSpinner = By.Id("aranea-loading-message");
    public static By FirstResultItem = By.XPath("/html/body/div[2]/form/div[2]/div[2]/div[2]/div[4]/table/tbody/tr[1]/td[3]/a[2]");
    public static By TotalResultsCount = By.XPath("/html/body/div[2]/form/div[2]/div[2]/div[2]/div[5]/div/p");
}