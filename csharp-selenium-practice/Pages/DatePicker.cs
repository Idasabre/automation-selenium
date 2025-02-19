using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Interactions;


namespace csharp_selenium_practice.Pages {
    public class DatePicker {
        private readonly IWebDriver driver;
        private readonly string url;

        //locators
        private By datePicker1 = By.Id("datepicker");
        private By currentMonth1 = By.XPath("//div[contains(@class, 'ui-datepicker')]//span[@class='ui-datepicker-month']");
        private By currentYear1 = By.XPath("//div[contains(@class, 'ui-datepicker')]//span[@class='ui-datepicker-year']");
        private By nextDatePicker1 = By.XPath("//a[@class='ui-datepicker-next ui-corner-all']");
        private By prevDatePicker1 = By.XPath("//a[@class='ui-datepicker-prev ui-corner-all']");

        //constructor - read TestData.json
        public DatePicker(IWebDriver driver) {

            this.driver = driver;
            var jsonFilePath = @"C:\Users\user\Documents\Automation\automation-exercise\c#_selenium\csharp-selenium-practice\TestData.json";
            var jsonData = File.ReadAllText(jsonFilePath);
            //var testData = Newtonsoft.Json.Linq.JObject.Parse(jsonData);
            var testData = JObject.Parse(jsonData);

            url = testData["url"].ToString();
            }

            public void LaunchURL() {
                Console.WriteLine("Navigating to URL...");
                driver.Navigate().GoToUrl(url);
            }

        public void PickDateFromDatePicker1(string date)
        {
            Console.WriteLine($"Selecting {date} from Date Picker...");
            
            //move to elemetn by Actions class
            Actions actions = new Actions(driver);
            
            // Click to open the DatePicker
            IWebElement datePickerField = driver.FindElement(datePicker1);
            actions.MoveToElement(datePickerField).Click().Perform();

            // Split the input date into month, day, and year components (MM/dd/yyyy)
            string[] dateParts = date.Split('/');
            int month = int.Parse(dateParts[0]);
            int day = int.Parse(dateParts[1]);
            int year = int.Parse(dateParts[2]);

            Console.WriteLine($"month/day/year: {month}/{day}/{year}");

            // Define an array for months
            string[] monthNames = new string[] {
                "January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            };

            // Wait until the calendar is displayed
            //wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//div[contains(@class, 'ui-datepicker')]")));

            // Get the current year and month from the DatePicker
            IWebElement yearElement = driver.FindElement(currentYear1);
            string currentYear = yearElement.Text;

            IWebElement monthElement = driver.FindElement(currentMonth1);
            string currentMonth = monthElement.Text;

            // Select the correct year
            while (currentYear != year.ToString())
            {
                if (int.Parse(currentYear) < year)
                {
                    // Click "Next" to go forward
                    IWebElement nextButton = driver.FindElement(nextDatePicker1);
                    nextButton.Click();
                    Thread.Sleep(1000);
                }
                else if (int.Parse(currentYear) > year)
                {
                    // Click "Previous" to go back
                    IWebElement prevButton = driver.FindElement(prevDatePicker1);
                    prevButton.Click();
                    Thread.Sleep(1000); 
                }

                currentYear = driver.FindElement(currentYear1).Text;
            }

            // Select the correct month
            string targetMonth = monthNames[month - 1]; // Get the month name
            if (currentMonth != targetMonth)
            {
                // Navigate to the desired month
                while (currentMonth != targetMonth)
                {
                    IWebElement nextButton = driver.FindElement(nextDatePicker1);
                    nextButton.Click();
                    Thread.Sleep(1000); 
                    currentMonth = driver.FindElement(currentMonth1).Text;
                }
            }

            // Select the correct day
            IWebElement dayElement = driver.FindElement(By.XPath($"//a[contains(@class, 'ui-state-default') and contains(text(), '{day}')]"));
            dayElement.Click();
        }
    }
}