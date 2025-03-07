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
 *      create ExtendReport.cs class to handle report
 *      
 *
*/
using System;
using System.Configuration;
using AventStack.ExtentReports;
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

namespace selenium_csharp_swaglab.Utilities;

public class Base
{
    //public IWebDriver driver;
    protected ThreadLocal<IWebDriver> driver = new();
    protected static TestDataReader getDataParser;  //protected since it will be used in another class as well eg. E2ETest class and static bcause E@E method that use getDataParses has static method method
    String BrowserName; 

    [SetUp]
    public void SetUp()
    {

        ExtentReportHelper.CreateTest(TestContext.CurrentContext.Test.Name);
        ExtentReportHelper.Log("Test Started");


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
                break;*/
                
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
    
    [TearDown]
    public void TearDown()
    {
        var testStatus = TestContext.CurrentContext.Result.Outcome.Status;

        if (testStatus == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            string errorMessage = TestContext.CurrentContext.Result.Message;
            ExtentReportHelper.LogFail($"Test Failed: {errorMessage}");
            ExtentReportHelper.AttachScreenshot(ScreenshotHelper.CaptureScreenshot(driver.Value, "Failure"));
        }
        else
        {
            ExtentReportHelper.LogPass("Test Passed Successfully");
        }
        
        ExtentReportHelper.FlushReport();
        driver.Value.Quit();
        driver.Value.Dispose();
    }
}
