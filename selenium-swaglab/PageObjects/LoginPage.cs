using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI; 
using selenium_swaglab.Utils;
using System;
using System.Threading;

namespace selenium_swaglab.PageObjects
{
    public class LoginPage
    {
        private IWebDriver driver;
        
        public LoginPage()
        {
            driver = DriverHelper.Driver; 
        }

        private By usernameField = By.Id("user-name");
        private By passwordField = By.XPath("//input[@id='password']");
        private By loginButton = By.cssSelector("#login-button");

        public void Login(string username, string password)
        {
            driver.Navigate().GoToUrl(TestData.BaseUrl);
            ExtentReportHelper.CreateTest("Verify Login Functionality");
            ExtentReportHelper.LogInfo("Navigated to Login Page");
            ScreenshotHelper.CaptureScreenshot(driver, "01_NavigateToLogin");

            // Explicit wait for username field to be visible 
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(drv => drv.FindElement(usernameField).Displayed);
            ExtentReportHelper.LogInfo("Login Page is visible");
            ScreenshotHelper.CaptureScreenshot(driver, "02_LoginPageVisible");

            driver.FindElement(usernameField).SendKeys(username);
            driver.FindElement(passwordField).SendKeys(password);
            ExtentReportHelper.LogInfo("Entered login credentials");
            ScreenshotHelper.CaptureScreenshot(driver, "03_EnteredCredentials");

            Thread.Sleep(3000);
            driver.FindElement(loginButton).Click();
            ExtentReportHelper.LogInfo("Clicked Login Button");
            ScreenshotHelper.CaptureScreenshot(driver, "04_ClickedLogin");
        }
    }
}
