/*Data driven from passing global data using [TestCase] NUnit annotation*/
using NUnit.Framework;
using selenium_csharp_swaglab.Utilities;
using selenium_csharp_swaglab.PageObjects;
using OpenQA.Selenium;
using System.Configuration;


namespace selenium_csharp_swaglab.Tests;

public class E2E_DataDriven_TestCase : Base
{
    private LoginPage login;
    private InventoryPage inventory;
    private CartPage cart;
    private CheckoutPage checkout;
    private String username;
    private String password;
    private bool isProductFound;

    [SetUp]
    public void initE2E()
    {
        login = new LoginPage(driver);
        inventory = new InventoryPage(driver);
        cart = new CartPage(driver);
        checkout = new CheckoutPage(driver);
        username = ConfigurationManager.AppSettings["username"];
        password = ConfigurationManager.AppSettings["password"];
    }

    [Test, Order(1)]
    [TestCase("standard_user", "secret_sauce","lohi","user","test","12456")]
    [TestCase("standard_user", "secret_sauce", "hilo", "user", "test", "34567")]
    public void E2ETest(string UserName, string Password, string sortItem, string firstname, string lastname, string postalCode)
    {
        //login page
        Console.WriteLine($"Logging in as {UserName}..."); 
        login.Login(UserName, Password);        //TestCase global data

        Console.WriteLine("Validating landing page...");
        if (inventory.IsInventoryPageDisplayed())
        {
            Console.WriteLine("Inventory page is displayed");
        }
        else
        {
            Console.WriteLine("Inventory page is not displayed");
        }


        //inventory page
        Console.WriteLine("Sorting products...");
        Thread.Sleep(2000); 
        inventory.SortItemsLowToHigh(sortItem);     //TestCase global data
        Console.WriteLine("Product sorted from low to high price");
        Thread.Sleep(2000);

        Console.WriteLine("Add item to cart...");
        List<(string, string)> productDetails = inventory.AddItemsToCart();
        Console.WriteLine($"{string.Join(", ", productDetails)} are added.");


        //cart page
        Console.WriteLine("Validating Item(s) in cart...");
        cart.NavigateToCart();
        IList<string> itemsInCart = cart.VerifyItemInCart();
        Console.WriteLine($"{string.Join(", ", itemsInCart)} is in cart page.");

        Assert.That(itemsInCart, Is.EquivalentTo(productDetails.Select(p => p.Item1)), "Items in the cart do not match the expected product names.");
        Console.WriteLine("Item matched in Cart page");

        //checkout page
        Console.WriteLine("Navigating to checkout page...");
        checkout.NavigateToCheckoutPage();
        Console.WriteLine("Entering checkout information and proceed...");
        checkout.enterCheckoutInfo(firstname, lastname, postalCode);        //TestCase global data
        checkout.proceedCheckout();
        checkout.isCheckoutPageDisplayed();
        Console.WriteLine("Validating product in checkout page...");
        IList<string> itemsInCheckout = checkout.VerifyItemInCheckout();
        Console.WriteLine($"{string.Join(", ", itemsInCheckout)} is in checkout page.");

        Assert.That(itemsInCheckout, Is.EquivalentTo(productDetails.Select(p => p.Item1)), "Items in the cart do not match the expected product names.");
        Console.WriteLine("Item matched in Checkout page");
    }
}

