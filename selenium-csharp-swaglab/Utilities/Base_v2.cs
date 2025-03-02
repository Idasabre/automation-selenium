using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace selenium_csharp_swaglab.Utilities;

public class Base_v2
{
    public IWebDriver driver;

    [SetUp]
    public void SetUp()
    {

        InitBrowser("Chrome");
        driver.Manage().Window.Maximize();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5); //wait for 5 seconds for each steps using Implicit wait propoerty
        driver.Url = "https://www.saucedemo.com/";
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
        driver.Quit();
    }


}
