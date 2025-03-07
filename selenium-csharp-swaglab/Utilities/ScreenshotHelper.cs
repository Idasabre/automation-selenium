using OpenQA.Selenium;
using System;
using System.IO;

namespace selenium_csharp_swaglab.Utilities
{
    public static class ScreenshotHelper
    {
        private static string screenshotDirectory;
        static ScreenshotHelper()
        {
            //string workingDir = Path.Combine(Environment.CurrentDirectory);
            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            screenshotDirectory = Path.Combine(projectDirectory, "Screenshots");

            if (!Directory.Exists(screenshotDirectory))
            {
                Directory.CreateDirectory(screenshotDirectory);
            }
        }

        public static string CaptureScreenshot(IWebDriver driver, string stepName)
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

                // Ensure the file is fully written before attaching
                if (File.Exists(filePath))
                {
                    Console.WriteLine($"Screenshot captured: {filePath}");
                    System.Threading.Thread.Sleep(500);  // Small delay to ensure file is saved

                    // Attach file path to ExtentReports
                    ExtentReportHelper.AttachScreenshot(filePath);
                }
                else
                {
                    Console.WriteLine("Error: Screenshot file was not saved properly.");
                }

                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to capture screenshot: {ex.Message}");
                return string.Empty;
            }
        }
    }
}