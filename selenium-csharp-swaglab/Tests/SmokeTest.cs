/*
 * 
 * run TestCategory=Smoke on CLI
 * dotnet test selenium-csharp-swaglab.csproj --filter TestCategory=Smoke
 * 
 */
using System.Configuration;
using selenium_csharp_swaglab.Utilities;
using selenium_csharp_swaglab.PageObjects;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;


namespace selenium_csharp_swaglab.Tests;

[Parallelizable(ParallelScope.Children)]
public class Smoke : Base
{
    private LoginPage login;
    private InventoryPage inventory;
    private CartPage cart;
    private CheckoutPage checkout;
    private string username;
    private string password;
    private WebDriverWait wait;

    [SetUp]
    public void initE2E()
    {
        login = new LoginPage(driver.Value);
        inventory = new InventoryPage(driver.Value);
        cart = new CartPage(driver.Value);
        checkout = new CheckoutPage(driver.Value);

        username = ConfigurationManager.AppSettings["username"];
        password = ConfigurationManager.AppSettings["password"];

        wait = new WebDriverWait(driver.Value, TimeSpan.FromSeconds(10));
    }

    [Test, Order(1), Category("Smoke")]
    public void ValidLoginTest()
    {
        login.Login(username, password);
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

    [Test, Order(4), Category("Smoke")]
    public void sortProductTest()
    {
        string sortoption = getDataParser.ExtractData("inventory.sortoption");

        login.Login(username, password);
        wait.Until(ExpectedConditions.ElementIsVisible(By.CssSelector(".product_sort_container")));
        inventory.SortItems(sortoption);
    }
}
