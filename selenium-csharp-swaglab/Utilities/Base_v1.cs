/*
 * -- run with global param at runtime. eg. send BrowserName as Chrome while App.config already pass value Firefox
 *      add BrowserName = TestContext.Parameters["BrowserName"]
 *      run from PowerShell: dotnet test selenium-csharp-swaglab.csproj --filter TestCategory=Smoke --% -- TestRunParameters.Parameter(name=\"BrowserName\", value=\"Chrome\")
 *      '\' represent special chars
 *      or dotnet test selenium-csharp-swaglab.csproj --filter TestCategory=Smoke -- %TestRunParameters% "BrowserName=Chrome"
 *      run from Git Bash: dotnet test selenium-csharp-swaglab.csproj --filter TestCategory=Smoke -- 'TestRunParameters.Parameter(name="BrowserName",value="Chrome")'
 *      run from CMD: dotnet test selenium-csharp-swaglab.csproj --filter TestCategory=Smoke -- TestRunParameters.Parameter(name="BrowserName",value="Chrome")
 *      
 *--Add ExtentReports package
 *  Initialiaze report file under [OneTimeSetup]
 *  Create object of ExtentSparkReporter class
 *  use ExtentReports class and create extent object to listen to all test cases and update to ExtentSparkReporter report (method AttachReporter)
 *  create entry of report in SetUp - test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
 *  
*/
using System;
using System.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;
using AventStack.ExtentReports;               // to use ExtentReports() class
using AventStack.ExtentReports.Reporter;      // to use ExtentSparkReporter class

namespace selenium_csharp_swaglab.Utilities;

public class Base_V1
{
    //public IWebDriver driver;
    protected ThreadLocal<IWebDriver> driver = new();
    protected static TestDataReader getDataParser;  //protected since it will be used in another class as well eg. E2ETest class and static bcause E@E method that use getDataParses has static method method
    String BrowserName;
    protected ExtentReports extent;
    protected ExtentTest test;


    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        string workingDir = Environment.CurrentDirectory;
        string projectDir = Directory.GetParent(workingDir).Parent.Parent.FullName;
        string reportPath = Path.Combine(projectDir, "ExtentReport.html");
        var htmlReport = new ExtentSparkReporter(reportPath);
        extent = new ExtentReports();
        extent.AttachReporter(htmlReport);
        extent.AddSystemInfo("Host Name", "Local Host"); //metadata info about the report
        extent.AddSystemInfo("Environment", "QA");
        extent.AddSystemInfo("User", "Standard User");
    }

    [SetUp]
    public void SetUp()
    {

        test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
        test.Log(Status.Info, "Test Started");


        /*//send global param at runtime
        // Retrieve BrowserName from TestContext, fallback to App.config
        BrowserName = TestContext.Parameters["BrowserName"];
        if (string.IsNullOrEmpty(BrowserName)) // Ensure no null values
        {
            BrowserName = ConfigurationManager.AppSettings["browser"];
        }*/


        //read data from App.config xml file
        string BrowserName = ConfigurationManager.AppSettings["browser"];
        string URL = ConfigurationManager.AppSettings["url"];

        //or read data from TestDataReader.cs to read data from TestData.json file
        //String BrowserName = getDataParser.ExtractData("config.browser");
        //String URL = getDataParser.ExtractData("config.url");

        InitBrowser(BrowserName);
        driver.Value.Manage().Window.Maximize();
        driver.Value.Url = URL;

        driver.Value.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5); //wait for 5 seconds for each steps using Implicit wait propoerty

        // Initialize TestDataReader
        getDataParser = new TestDataReader();
    }

    public void InitBrowser(string BrowserName)
    {
        switch (BrowserName)
        {
            case "Chrome":
                /*new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig()); //WebDriverManager.DriverConfigs.Impl; for ChromeConfig()
                driver.Value = new ChromeDriver();
                break;
                */
                var chromeVersion = new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig(), VersionResolveStrategy.MatchingBrowser);
                ChromeOptions chromeOptions = new ChromeOptions();
                driver.Value = new ChromeDriver(chromeOptions);
                break;

            case "Firefox":
                new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig());
                driver.Value = new FirefoxDriver();
                break;

            case "Edge":
                new WebDriverManager.DriverManager().SetUpDriver(new EdgeConfig());
                driver.Value = new EdgeDriver();
                break;
        }

    }

    public string CaptureScreenshot(IWebDriver driver, string screenshotName)
    {
        try
        {
            ITakesScreenshot ts = (ITakesScreenshot)driver;
            Screenshot screenshot = ts.GetScreenshot();

            // Define screenshot directory inside the project
            string workingDir = Environment.CurrentDirectory;
            string projectDir = Directory.GetParent(workingDir)?.Parent?.Parent?.FullName ?? workingDir;
            string screenshotsDir = Path.Combine(projectDir, "Screenshots");

            // Ensure the directory exists
            if (!Directory.Exists(screenshotsDir))
            {
                Directory.CreateDirectory(screenshotsDir);
            }

            // Full path for the screenshot
            string screenshotPath = Path.Combine(screenshotsDir, screenshotName + ".png");

            // Save screenshot as a file
            screenshot.SaveAsFile(screenshotPath);

            // Return the file path for attachment
            return screenshotPath;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error capturing screenshot: " + ex.Message);
            return null;
        }
    }
    
    [TearDown]
    public void TearDown()
    {
        if (driver.IsValueCreated && driver.Value != null)
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stackTrace = TestContext.CurrentContext.Result.StackTrace;  //error log

            //Generate a timestamped filename for uniqueness
            string timeStamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string screenshotName = "Screenshot_" + timeStamp;
            string screenshotPath = CaptureScreenshot(driver.Value, screenshotName);

            if (status == TestStatus.Failed && !string.IsNullOrEmpty(screenshotPath))
            {
                test.Fail("Test Failed")
                    .AddScreenCaptureFromPath(screenshotPath); // Attach screenshot file to report
                test.Log(Status.Fail, "Test failed with log trace: " + stackTrace);
            }

            extent.Flush();
            driver.Value.Quit();
            driver.Value.Dispose();
            test.Log(Status.Info, "Test Execution Completed");
        }
    }


    /*[OneTimeTearDown]
    public void OneTimeTearDown()
    {
        extent.Flush(); // Save the report
    }*/
}
