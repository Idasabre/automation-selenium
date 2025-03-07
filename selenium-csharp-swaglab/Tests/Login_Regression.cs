/*
 * run by category on CLI
 * dotnet test selenium-csharp-swaglab.csproj --filter TestCategory=Login_Regression
 */
using System.Configuration;
using selenium_csharp_swaglab.Utilities;
using selenium_csharp_swaglab.PageObjects;


namespace selenium_csharp_swaglab.Tests;

[Parallelizable(ParallelScope.Children)]
public class Login_Regression : Base
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

    [Test, Order(1), Category("Login_Regression")]
    public void ValidLoginTest()
    {
        string userName = ConfigurationManager.AppSettings["username"];
        string pwd = ConfigurationManager.AppSettings["password"];
        login.Login(userName, pwd);
    }

    [Test, Order(2), Category("Login_Regression")]
    public void InValidLoginTest()
    {
        login.Login("invaliduser", "12345");
    }

    [Test, Order(3), Category("Login_Regression")]
    public void BlankLoginTest()
    {
        login.ClickLogin();
        Assert.That(login.ErrorMessage(), Is.EqualTo("Epic sadface: Username is required"));
    }
}
