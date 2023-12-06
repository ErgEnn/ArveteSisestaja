using System.Linq.Expressions;
using OpenQA.Selenium;

namespace InvoiceDownloader.Helpers;

public static class ExpectedConditions
{
    public static Func<IWebDriver, IWebElement> ElementToBeClickable(Expression<Func<By>> expr)
    {
        Console.WriteLine($"[{nameof(ElementToBeClickable)}] {((MemberExpression)expr.Body).Member.Name}");
        return SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(expr.Compile().Invoke());
    }
    public static Func<IWebDriver, IWebElement> ElementExists(Expression<Func<By>> expr)
    {
        Console.WriteLine($"[{nameof(ElementExists)}] {((MemberExpression)expr.Body).Member.Name}");
        return SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(expr.Compile().Invoke());
    }
    public static Func<IWebDriver, IWebElement> ElementIsVisible(Expression<Func<By>> expr)
    {
        Console.WriteLine($"[{nameof(ElementIsVisible)}] {((MemberExpression)expr.Body).Member.Name}");
        return SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(expr.Compile().Invoke());
    }
    public static Func<IWebDriver, bool> InvisibilityOfElementLocated(Expression<Func<By>> expr)
    {
        Console.WriteLine($"[{nameof(InvisibilityOfElementLocated)}] {((MemberExpression)expr.Body).Member.Name}");
        return SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(expr.Compile().Invoke());
    }
    public static Func<IWebDriver, IWebDriver> FrameToBeAvailableAndSwitchToIt(Expression<Func<By>> expr)
    {
        Console.WriteLine($"[{nameof(FrameToBeAvailableAndSwitchToIt)}] {((MemberExpression)expr.Body).Member.Name}");
        return SeleniumExtras.WaitHelpers.ExpectedConditions.FrameToBeAvailableAndSwitchToIt(expr.Compile().Invoke());
    }
}