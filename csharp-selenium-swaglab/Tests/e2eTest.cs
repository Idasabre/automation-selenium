using NUnit.Framework;
using OpenQA.Selenium;
using selenium_swaglab.PageObjects;
using selenium_swaglab.Utils;
using System;

namespace selenium_swaglab.Tests
{
    [TestFixture]
    public class E2ETest
    {
        private IWebDriver driver;
        private LoginPage loginPage;
        private InventoryPage inventoryPage;
        private readonly dynamic testData;

        public E2ETest()
        {
            testData = new 
            {
                username = "standard_user",
                password = "secret_sauce"
            };
        }

        [SetUp]
        public void Setup()
        {
            Console.WriteLine("Initializing WebDriver...");
            DriverHelper.Initialize();
            driver = DriverHelper.Driver;

            loginPage = new LoginPage();
            inventoryPage = new InventoryPage();
        }

        [Test]
        public void Test_Login_And_Add_Product()
        {
            Console.WriteLine("Navigating to Login Page...");
            loginPage.Login(testData.username, testData.password);
            Thread.Sleep(3000);

            Assert.That(inventoryPage.IsInventoryPageDisplayed(), Is.True, "Login failed!");
            TestContext.WriteLine("Login successful!");

            Console.WriteLine("Sorting products by price (Low to High)...");
            inventoryPage.SortItemsLowToHigh();
            TestContext.WriteLine("Sorted products!");

            Console.WriteLine("Adding the most expensive item to cart...");
            inventoryPage.AddMostExpensiveItemToCart();
            TestContext.WriteLine("Product added successfully!");
        }

        [TearDown]
        public void TearDown()
        {
            Console.WriteLine("Genereting report...");
            ExtentReportHelper.FlushReport();

            Console.WriteLine("Closing browser...");
            DriverHelper.Quit();
        }
    }
}
