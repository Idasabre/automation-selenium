using OpenQA.Selenium;
using selenium_csharp_swaglab.Utilities;
using SeleniumExtras.PageObjects;


namespace selenium_csharp_swaglab.PageObjects;

public class LoginPage : Base
{
    private IWebDriver driver;

    // Define locators
    private By usernameLocator = By.Id("user-name");
    private By passwordLocator = By.XPath("//input[@id='password']");
    private By loginButtonLocator = By.Id("login-button");
    private By erroMsg = By.XPath("//h3[@data-test='error']");


    public LoginPage(IWebDriver driver)
    {
        this.driver = driver;
    }

    // Page actions
    public void EnterUsername(string username)
    {
        driver.FindElement(usernameLocator).SendKeys(username);
    }

    public void EnterPassword(string password)
    {
        driver.FindElement(passwordLocator).SendKeys(password);
    }

    public void ClickLogin()
    {
        driver.FindElement(loginButtonLocator).Click();
    }

    public string ErrorMessage()
    {
        var elements = driver.FindElements(erroMsg);
        Console.WriteLine($"Number of elements found: {elements.Count}");

        string errorElement = driver.FindElement(erroMsg).Text;
        Console.WriteLine($"Error Message: {errorElement}");

        return errorElement.Trim(); // Trim to remove extra spaces
    }

    public void Login(string username, string password)
    {
        EnterUsername(username);
        EnterPassword(password);

        ExtentReportHelper.LogInfo("Navigated to Login Page");
        ScreenshotHelper.CaptureScreenshot(driver, "01_LoginPage");

        ClickLogin();
    }
}
