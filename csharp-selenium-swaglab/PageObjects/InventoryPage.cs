using OpenQA.Selenium;
using selenium_swaglab.Utils;
using System.Linq;
using OpenQA.Selenium.Interactions; //for Actions.MoveToElement()

namespace selenium_swaglab.PageObjects
{
    public class InventoryPage
    {
        private IWebDriver driver;

        public InventoryPage() => driver = DriverHelper.Driver;

        private By inventoryContainer = By.Id("inventory_container");
        private By sortDropdown = By.CssSelector(".product_sort_container");
        private By inventoryPrices = By.CssSelector(".inventory_item_price");
        private By addToCartButtons = By.CssSelector(".btn_inventory");

        public bool IsInventoryPageDisplayed()
        {
            bool isDisplayed = driver.FindElement(inventoryContainer).Displayed;
            ScreenshotHelper.CaptureScreenshot(driver, "05_InventoryPageLoaded");
            return isDisplayed;
        }

        public void SortItemsLowToHigh()
        {
            var dropdown = driver.FindElement(sortDropdown);
            var selectElement = new OpenQA.Selenium.Support.UI.SelectElement(dropdown);
            selectElement.SelectByValue("lohi");
            ScreenshotHelper.CaptureScreenshot(driver, "06_SortedByPrice");
        }

        public void AddMostExpensiveItemToCart()
        {
            var buttons = driver.FindElements(addToCartButtons);
            var mostExpensiveButton = buttons.Last();

            // Initialize Actions class
            Actions actions = new Actions(driver);

            // Move to the element and click
            actions.MoveToElement(mostExpensiveButton).Click().Perform();
            ScreenshotHelper.CaptureScreenshot(driver, "07_AddedMostExpensiveItem");
        }
    }
}
