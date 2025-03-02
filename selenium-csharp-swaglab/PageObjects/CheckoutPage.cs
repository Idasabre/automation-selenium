using System;
using OpenQA.Selenium;
using selenium_csharp_swaglab.Utilities;

namespace selenium_csharp_swaglab.PageObjects
{
    class CheckoutPage : Base
    {
        private IWebDriver driver;

        //locators
        private By checkoutButton = By.CssSelector("#checkout");
        private By firstNameField = By.Id("first-name");
        private By lastNameField = By.Id("last-name");
        private By postalCodeField = By.Id("postal-code");
        private By continueButton = By.Id("continue");
        private By checkoutTitle = By.XPath("//span[contains(text(),'Checkout: Overview')]");
        private By cartList = By.CssSelector(".cart_list");
        private By prodName = By.XPath("//div/div/a/div[@class='inventory_item_name']");


        public CheckoutPage(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void NavigateToCheckoutPage()
        {
            driver.FindElement(checkoutButton).Click();
        }

        public void enterFirstName(string fname)
        {
            driver.FindElement(firstNameField).SendKeys(fname);
        }

        public void enterLastName(string lname)
        {
            driver.FindElement(lastNameField).SendKeys(lname);
        }

        public void enterPostalCode(string pcode)
        {
            driver.FindElement(postalCodeField).SendKeys(pcode);
        }

        public void enterCheckoutInfo(string firstname, string lastname, string postalcode)
        {
            enterFirstName(firstname);
            enterLastName(lastname);
            enterPostalCode(postalcode);
        }

        public void proceedCheckout()
        {
            driver.FindElement(continueButton).Click();
        }

        public bool isCheckoutPageDisplayed()
        {
            return driver.FindElement(checkoutTitle).Displayed;
        }

        public List<string> VerifyItemInCheckout()
        {
            List<string> checkoutProductNames = new List<string>();
            IList<IWebElement> productList = driver.FindElements(prodName);

            for (int i = 0; i < productList.Count; i++)
            {
                string ProductName = productList[i].Text;
                Console.WriteLine($"Found: {ProductName}");
                checkoutProductNames.Add(ProductName);
            }
            return checkoutProductNames;
        }

    }
}
