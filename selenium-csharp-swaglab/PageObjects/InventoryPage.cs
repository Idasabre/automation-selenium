using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using NUnit.Framework;
using selenium_csharp_swaglab.Utilities;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.WaitHelpers;

namespace selenium_csharp_swaglab.PageObjects
{
    class InventoryPage : Base
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        //locators
        private By inventoryContainer = By.Id("inventory_container");
        private By sortDropdown = By.CssSelector(".product_sort_container");
        private By inventoryPrices = By.CssSelector(".inventory_item_price");
        private By addToCartButtons = By.CssSelector(".btn_inventory");
        private By itemDetails = By.XPath("//div[contains(@class, 'inventory_item_description')]");
        private By prodName = By.XPath("//div/a/div[@class='inventory_item_name ']");
        private By prodPrice = By.XPath("//div/div[@class='inventory_item_price']");

        public InventoryPage(IWebDriver driver)
        {
            this.driver = driver;
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        public bool IsInventoryPageDisplayed()
        {
            bool IsDisplayed = driver.FindElement(inventoryContainer).Displayed;
            return IsDisplayed;
        }

        public void SortItemsLowToHigh(string sortOption)
        {
            IWebElement dropDown = driver.FindElement(sortDropdown);
            SelectElement select = new SelectElement(dropDown);
            select.SelectByValue(sortOption);

        }

        public List<(string, string)> AddItemsToCart()
        {
            
            List<(string, string)> productDetails = new List<(string, string)>();
            
            var productNames = driver.FindElements(prodName);
            var cheapestItemName = productNames.First().Text;

            var productPrice = driver.FindElements(prodPrice);
            var cheapestItemPrice = productPrice.First().Text.Replace("$", "");

            productDetails.Add((cheapestItemName, cheapestItemPrice));

            //Click add to cart
            var AddToCartButton = driver.FindElements(addToCartButtons);
            var CheapestItemButton = AddToCartButton.First();

            Actions action = new Actions(driver);
            action.MoveToElement(CheapestItemButton).Click().Perform();

            return productDetails;
        }
    }
}
