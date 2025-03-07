/*
 * 2.run all test method in one class parallel
 * dotnet test selenium-csharp-swaglab.csproj --filter TestCategory=E2E
*/
using NUnit.Framework;
using selenium_csharp_swaglab.Utilities;
using selenium_csharp_swaglab.PageObjects;
using OpenQA.Selenium;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;


namespace selenium_csharp_swaglab.Tests;

[Parallelizable(ParallelScope.Children)]
public class E2E : Base
{
    private LoginPage login;
    private InventoryPage inventory;
    private CartPage cart;
    private CheckoutPage checkout;

    [SetUp]
    public void initE2E()
    {
        login = new LoginPage(driver.Value);
        inventory = new InventoryPage(driver.Value);
        cart = new CartPage(driver.Value);
        checkout = new CheckoutPage(driver.Value);
    }

    [Test, Order(1)]
    public void ValidLoginTest()
    {
        string userName = ConfigurationManager.AppSettings["username"];
        string pwd = ConfigurationManager.AppSettings["password"];
        login.Login(userName, pwd);
    }

    [Test, Order(2)]
    public void InValidLoginTest()
    {
        login.Login("invaliduser", "12345");
    }

    [Test, Order(3)]
    public void BlankLoginTest()
    {
        login.ClickLogin();
        Assert.That(login.ErrorMessage(), Is.EqualTo("Epic sadface: Username is required"));
    }

    [Test, Order(4), Category("E2E")]
    [TestCaseSource(nameof(AddTestDataConfig))]
    public void E2ETest(string username, string password, string sortItem, string firstname, string lastname, string postalCode)
    {
        try
        {
            Console.WriteLine($"Logging in as {username}...");
            login.Login(username, password);


            Console.WriteLine("Validating landing page...");
            Assert.That(inventory.IsInventoryPageDisplayed(), "Inventory page is not displayed.");
            ExtentReportHelper.LogInfo("Navigated to Inventory Page");
            ScreenshotHelper.CaptureScreenshot(driver.Value, "02_InventoryPage");


            Console.WriteLine("Sorting products...");
            inventory.SortItems(sortItem);
            Console.WriteLine("Product sorted from low to high price");
            ExtentReportHelper.LogInfo("Sorted product");
            ScreenshotHelper.CaptureScreenshot(driver.Value, "03_InventoryPage-product sorted");


            Console.WriteLine("Add item to cart...");
            List<(string, string)> productDetails = inventory.AddItemsToCart();
            Console.WriteLine($"{string.Join(", ", productDetails)} are added.");
            ExtentReportHelper.LogInfo("Added product to cart");
            ScreenshotHelper.CaptureScreenshot(driver.Value, "04_InventoryPage-product added");


            Console.WriteLine("Validating Item(s) in cart...");
            cart.NavigateToCart();
            IList<string> itemsInCart = cart.VerifyItemInCart();
            Assert.That(itemsInCart, Is.EquivalentTo(productDetails.Select(p => p.Item1)), "Items in the cart do not match the expected product names.");
            ExtentReportHelper.LogInfo("Validated product in cart page");
            ScreenshotHelper.CaptureScreenshot(driver.Value, "05_CartPage");


            Console.WriteLine("Navigating to checkout page...");
            checkout.NavigateToCheckoutPage();
            checkout.enterCheckoutInfo(firstname, lastname, postalCode);
            checkout.proceedCheckout();
            Assert.That(checkout.isCheckoutPageDisplayed(), "Checkout page is not displayed.");


            Console.WriteLine("Validating product in checkout page...");
            IList<string> itemsInCheckout = checkout.VerifyItemInCheckout();
            Assert.That(itemsInCheckout, Is.EquivalentTo(productDetails.Select(p => p.Item1)), "Items in checkout do not match expected product names.");
            ExtentReportHelper.LogInfo("Validated product in Checkout page");
            ScreenshotHelper.CaptureScreenshot(driver.Value, "06_CheckoutPage-product validated");
        }
        catch (Exception ex)
        {
            ExtentReportHelper.LogFail($"Test failed: {ex.Message}");
            ExtentReportHelper.AttachScreenshot(ScreenshotHelper.CaptureScreenshot(driver.Value, "Failure"));
            throw; // Rethrow to ensure NUnit marks it as failed
        }
    }
    public static IEnumerable<TestCaseData> AddTestDataConfig()
    {
        if (getDataParser == null)
        {
            getDataParser = new TestDataReader();
        }

        string username = getDataParser.ExtractData("user.username");
        string password = getDataParser.ExtractData("user.password");

        yield return new TestCaseData(username, password, "lohi", "User1", "Test1", "12456");
        //yield return new TestCaseData(username, password, "az", "User2", "Test2", "34567");
    }
}
