/* using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using System;
using System.IO;

namespace selenium_swaglab.Utils
{
    public static class ScreenshotHelper
    {
        private static string screenshotDirectory = "../../../Screenshots"; //"Screenshots";

        static ScreenshotHelper()
        {
            if (!Directory.Exists(screenshotDirectory))
            {
                Directory.CreateDirectory(screenshotDirectory);
            }
        }

        public static void CaptureScreenshot(IWebDriver driver, string stepName)
        {

            try
            {
                Screenshot screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string filePath = Path.Combine(screenshotDirectory, $"{stepName}_{timestamp}.png");

                // Convert screenshot to Base64 and save it as an image
                byte[] imageBytes = Convert.FromBase64String(screenshot.AsBase64EncodedString);
                File.WriteAllBytes(filePath, imageBytes);

                Console.WriteLine($"Screenshot captured: {filePath}");

                TestContext.AddTestAttachment(filePath); // Attach to NUnit report
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to capture screenshot: {ex.Message}");
            }
        }
    }
}
 */