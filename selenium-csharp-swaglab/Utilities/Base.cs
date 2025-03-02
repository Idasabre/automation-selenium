using System;
using System.Configuration;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace selenium_csharp_swaglab.Utilities;

public class Base
{
    public IWebDriver driver;
    protected static TestDataReader getDataParser;  //protected since it will be used in another class as well eg. E2ETest class and static bcause E@E method that use getDataParses has static method method

    [SetUp]
    public void SetUp()
    {
        //read data from App.config xml file
        String BrowserName = ConfigurationManager.AppSettings["browser"];
        String URL = ConfigurationManager.AppSettings["url"];

        //or read data from TestDataReader.cs to read data from TestData.json file
        //String BrowserName = getDataParser.ExtractData("config.browser");
        //String URL = getDataParser.ExtractData("config.url");

        InitBrowser(BrowserName);
        driver.Manage().Window.Maximize();
        driver.Url = URL;

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5); //wait for 5 seconds for each steps using Implicit wait propoerty

        // Initialize TestDataReader
        getDataParser = new TestDataReader();
    }

    public void InitBrowser(string BrowserName)
    {
        switch (BrowserName)
        {
            case "Chrome":
                new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig()); //WebDriverManager.DriverConfigs.Impl; for ChromeConfig()
                driver = new ChromeDriver();
                break;

            case "Firefox":
                new WebDriverManager.DriverManager().SetUpDriver(new FirefoxConfig()); //WebDriverManager.DriverConfigs.Impl; for ChromeConfig()
                driver = new FirefoxDriver();
                break;

            case "Edge":
                new WebDriverManager.DriverManager().SetUpDriver(new EdgeConfig()); //WebDriverManager.DriverConfigs.Impl; for ChromeConfig()
                driver = new EdgeDriver();
                break;
        }
        
    }

    [TearDown]
    public void TearDown()
    {
        if (driver != null)
        {
            driver.Quit();
            driver.Dispose();   // Ensure resources are freed
            driver = null;      // Avoid referencing disposed object
        }
    }


}
