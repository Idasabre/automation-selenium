using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace selenium_swaglab.Utils
{
    public class DriverHelper
    {
        public static IWebDriver? Driver { get; set; }

        public static void Initialize()
        {
            if (Driver == null)
            {
                Console.WriteLine("Starting WebDriver...");
                var options = new ChromeOptions();
                options.AddArgument("--start-maximized");  // Open browser in maximized mode
                options.AddArgument("--disable-infobars"); // Disable Chrome info bars
                
                Driver = new ChromeDriver(options);

                // Set Implicit Wait (if element not found)
                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            }
        }

        public static void Quit()
        {
            Console.WriteLine("Closing WebDriver...");
            Driver?.Quit();
            Driver = null;
        }
    }
}
