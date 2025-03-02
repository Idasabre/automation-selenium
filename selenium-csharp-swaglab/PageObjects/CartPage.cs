using System;
using OpenQA.Selenium;
using selenium_csharp_swaglab.Utilities;

namespace selenium_csharp_swaglab.PageObjects
{
    class CartPage : Base
    {
        private IWebDriver driver;

        //locators
        private By cartLocator = By.CssSelector(".shopping_cart_link");
        private By cartItems = By.XPath(".//div[@class='cart_item']");
        private By itemLabelName = By.XPath("//div/a/div[@class=\"inventory_item_name\"]");

        public CartPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void NavigateToCart()
        {
            driver.FindElement(cartLocator).Click(); 
        }

        public List<string> VerifyItemInCart()
        {
            List<string> cartProductNames = new List<string>();
            IList<IWebElement> cartTable = driver.FindElements(itemLabelName);
            
            foreach (IWebElement item in cartTable)
            {
                string ProductName = item.Text;
                Console.WriteLine($"Found: {ProductName}");
                cartProductNames.Add(ProductName);
            }
            return cartProductNames;
        }
    }
}
