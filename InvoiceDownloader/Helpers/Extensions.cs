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
    internal static IWebElement SafeExecuteOnElement(this By by, IWebDriver driver, Action<IWebElement> action)
    {
        IWebElement element = null;
        for (int i = 0; i < 100; i++)
        {
            try
            {
                element = by.FindElement(driver);
                action(element);
                break;
            }
            catch (StaleElementReferenceException)
            {

            }
        }

        return element;
    }
    internal static TResult SafeExecuteOnElement<TResult>(this By by, IWebDriver driver, Func<IWebElement, TResult> action)
    {
        IWebElement element = null;
        StaleElementReferenceException exception = null;
        for (int i = 0; i < 100; i++)
        {
            try
            {
                element = by.FindElement(driver);
                return action(element);
                break;
            }
            catch (StaleElementReferenceException ex)
            {
                exception = ex;
            }
        }

        throw exception;
    }
}