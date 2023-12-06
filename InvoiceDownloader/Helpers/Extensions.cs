using OpenQA.Selenium;

namespace InvoiceDownloader.Helpers;

internal static class Extensions
{
    internal static IWebElement SafeFindElement(this By by, IWebDriver driver)
    {
        IWebElement element = null;
        for (int i = 0; i < 100; i++)
        {
            try
            {
                element = by.FindElement(driver);
                var _ = element.Text;
                break;
            }
            catch (StaleElementReferenceException)
            {

            }
        }

        return element;
    }
}