using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace csharp_selenium_practice.Pages
{
    public class PaginationWebTable {
        private IWebDriver driver;

        //locators


        public PaginationWebTable(IWebDriver driver) { 
            this.driver = driver;
        }

        public void CheckProductByName(string Name)
        {
            bool itemFound = false;

            IList<IWebElement> pagination = driver.FindElements(By.XPath("//ul[@id='pagination']/li/a"));
            for (int pageIndex = 0; pageIndex < pagination.Count; pageIndex++) 
            {
                // Find all rows in the table
                IList<IWebElement> rows = driver.FindElements(By.XPath("//table[@id='productTable']/tbody/tr"));
                //loop the rows
                for (int rowIndex = 1; rowIndex <= rows.Count; rowIndex++)
                {
                    // Get the text of the second column (Product Name)
                    string productName = driver.FindElement(By.XPath($"//table[@id='productTable']/tbody/tr[{rowIndex}]/td[2]")).Text;
                    Console.WriteLine($"Validating {productName} on page {pageIndex + 1} at row {rowIndex}");

                    if (productName.Equals($"{Name}", StringComparison.OrdinalIgnoreCase))
                    {
                        // Click the checkbox in the same row
                        IWebElement checkBox = driver.FindElement(By.XPath($"//table[@id='productTable']/tbody/tr[{rowIndex}]/td[4]/input[@type='checkbox']"));
                        checkBox.Click();
                        itemFound = true;
                        
                        Console.WriteLine($"Found '{Name}' on page {pageIndex + 1} at Row {rowIndex} and checked");

                        break;
                    }
                }
                
                // Check if next page exists and is enabled
                // Try to find and click the next page button and handle xception when nextPage is not found and throw error    
                if (!itemFound) {
                    try
                    {
                        IWebElement nextPage = driver.FindElement(By.XPath($"//ul[@id='pagination']/li/a[text()='{pageIndex + 2}']"));

                        if (nextPage.Displayed && nextPage.Enabled)
                        {
                            nextPage.Click();
                            Thread.Sleep(2000);
                        }
                        else
                        {
                            Console.WriteLine($"Reached last page, but '{Name}' not found.");
                            break;
                        }
                    }
                    catch (NoSuchElementException)
                    {
                        Console.WriteLine($"No more pages found. Stopping pagination.");
                        break;
                    }
                }
            }            
            Assert.That(itemFound, Is.True, $"'{Name}' was not found in the table.");
        }
    }
}