using OpenQA.Selenium;

namespace InvoiceDownloader.Omniva;

public class LoginPage
{
    public static By Username = By.Name("username");
    public static By Password = By.Name("password");
    public static By UserPassAuthMethodTab = By.Id("userpass-tab");
}