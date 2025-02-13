using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using System;
using System.IO;

namespace selenium_swaglab.Utils
{
    public static class ExtentReportHelper
    {
        private static ExtentReports? extent;
        private static ExtentTest? test;
        private static string reportDirectory = Path.Combine(Environment.CurrentDirectory, "TestReports");
        private static string reportPath = Path.Combine(reportDirectory, "ExtentReport.html");

        static ExtentReportHelper()
        {
            if (!Directory.Exists(reportDirectory))
            {
                Directory.CreateDirectory(reportDirectory);
            }

            // Use ExtentSparkReporter instead of ExtentHtmlReporter
            var sparkReporter = new ExtentSparkReporter(reportPath);
            extent = new ExtentReports();
            extent.AttachReporter(sparkReporter);
        }

        public static void CreateTest(string testName)
        {
            test = extent?.CreateTest(testName);
        }

        public static void LogInfo(string message)
        {
            test?.Info(message);
        }

        public static void LogPass(string message)
        {
            test?.Pass(message);
        }

        public static void LogFail(string message)
        {
            test?.Fail(message);
        }

        public static void AttachScreenshot(string screenshotPath)
        {
            test?.AddScreenCaptureFromPath(screenshotPath);
        }

        public static void FlushReport()
        {
            extent?.Flush();
            Console.WriteLine($"Extent Report saved at: {reportPath}");
        }
    }
}
