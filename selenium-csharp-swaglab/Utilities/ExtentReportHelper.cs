using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;

namespace selenium_csharp_swaglab.Utilities;
public class ExtentReportHelper
{
    private static ExtentReports? extent;
    private static ExtentTest? test;
    private static readonly string reportDirectory;
    private static readonly string reportPath;
    private static bool isInitialized = false;

    static ExtentReportHelper()
    {
        //reportDirectory = Path.Combine(Environment.CurrentDirectory, "TestReports");
        //reportPath = Path.Combine(reportDirectory, "ExtentReport.html");

        string workingDir = Environment.CurrentDirectory;
       // string projectDirectory = Directory.GetParent(workingDir).Parent.Parent.Parent.FullName;
        string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
        string reportDirectory = Path.Combine(projectDirectory, "TestReports");
        string reportPath = Path.Combine(reportDirectory, "ExtentReport.html");

        if (!Directory.Exists(reportDirectory))
        {
            Directory.CreateDirectory(reportDirectory);
        }

        var sparkReporter = new ExtentSparkReporter(reportPath);
        extent = new ExtentReports();
        extent.AttachReporter(sparkReporter);

        extent.AddSystemInfo("Host Name", "Local Host");
        extent.AddSystemInfo("Environment", "QA");
        extent.AddSystemInfo("User", "Standard User");

        isInitialized = true;
    }

    public static void CreateTest(string testName)
    {
        test = extent?.CreateTest(testName);
    }

    public static void LogInfo(string message)
    {
        test?.Info(message);
    }

    public static void Log(string message)
    {
        test?.Log(Status.Info, message);
    }

    public static void LogPass(string message)
    {
        test?.Pass(message);
    }

    public static void LogFail(string message)
    {
        test?.Fail(message);
        extent?.Flush();  // Ensure report is updated immediately   
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
