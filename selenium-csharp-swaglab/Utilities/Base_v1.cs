using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace selenium_csharp_swaglab.Utilities;

public class Base_v1
{
    public IWebDriver driver;

    [SetUp]
    public void SetUp()
    {
        new WebDriverManager.DriverManager().SetUpDriver(new ChromeConfig()); //WebDriverManager.DriverConfigs.Impl; for ChromeConfig()
        driver = new ChromeDriver();

        driver.Manage().Window.Maximize();
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5); //wait for 5 seconds for each steps using Implicit wait propoerty
        driver.Url = "https://www.saucedemo.com/";
    }

    [TearDown]
    public void TearDown()
    {
        driver.Quit();
    }


}
