using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace csharp_selenium_practice.Pages
{
    public class StaticTable {
        private IWebDriver driver;

        //locators
        private By table = By.XPath("//table[@name='BookTable']");
        private By rows  = By.XPath(".//tr");
        private By cells  = By.XPath(".//td");

        
        public StaticTable(IWebDriver driver) { 
            this.driver = driver;
        }

        public void readRows() {

            Console.WriteLine("reading rows of table...");
            
            IWebElement tableElement = driver.FindElement(table);

            //scroll ot element
            Actions actions = new Actions(driver);
            actions.MoveToElement(tableElement);

            //get list of rows
            IList<IWebElement> rowElements = tableElement.FindElements(rows);

            //loop through each row
            foreach (IWebElement row in rowElements) {
                Console.WriteLine(row.Text);
            }
        }

        public void readCells() {
            Console.WriteLine("reading cells of table...");

            IWebElement tableElement = driver.FindElement(table);

            //scroll ot element
            Actions actions = new Actions(driver);
            actions.MoveToElement(tableElement);

            IList<IWebElement> rowsElement = tableElement.FindElements(rows);

            foreach (IWebElement row in rowsElement)
            {
                IList<IWebElement> cellElements = row.FindElements(cells);

                foreach (IWebElement cell in cellElements)
                {
                    Console.WriteLine($"{cell.Text}");
                }                
            }
        }

        public void readDataFromCells(string text)
        {
            // Find the table element
            IWebElement tableElement = driver.FindElement(table);

            // Get list of rows (tr) inside the table
            IList<IWebElement> rowElements = tableElement.FindElements(rows);

            // Loop through each row to find and check cells (td) in each row
            for (int rowIndex = 0; rowIndex < rowElements.Count; rowIndex++)
            {
                IWebElement row = rowElements[rowIndex];
                IList<IWebElement> cellElements = row.FindElements(cells); // Find cells (td) in each row

                // Loop through each cell and check if it contains the matching text
                for (int cellIndex = 0; cellIndex < cellElements.Count; cellIndex++)
                {
                    IWebElement cell = cellElements[cellIndex];
                    if (cell.Text.Contains(text)) // Check if cell's text matches the input text
                    {
                        Console.WriteLine($"Match found at Row {rowIndex + 1}, Column {cellIndex + 1}: {cell.Text}");
                        return; // Exit the loop if a match is found
                    }
                }
            }

            // If no match is found, print "Not Match"
            Console.WriteLine("Not Match");
        }


        /* public void readDataFromCells(string text)
        {
            Console.WriteLine("reading data from table...");
            // Find the table element
            IWebElement tableElement = driver.FindElement(table);

            // Get list of rows (tr) inside the table
            IList<IWebElement> rowElements = tableElement.FindElements(rows);

            // Loop through each row to find and check cells (td) in each row
            foreach (IWebElement row in rowElements)
            {
                IList<IWebElement> cellElements = row.FindElements(cells); // Find cells (td) in each row

                // Loop through each cell and check if it contains the matching text
                foreach (IWebElement cell in cellElements)
                {
                    if (cell.Text.Contains(text)) // Check if cell's text matches the input text
                    {
                        Console.WriteLine("Match: " + cell.Text); // Print match
                        return; // Exit the loop if a match is found
                    }
                }
            }
            // If no match is found, print "Not Match"
            Console.WriteLine("Not Match");
        } */
    }
}