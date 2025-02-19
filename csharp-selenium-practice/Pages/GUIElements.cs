using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Support.UI; //SelectElement Class


namespace csharp_selenium_practice.Pages {
    public class GUIElements {
        private readonly IWebDriver driver;
        private readonly string url;
        private readonly string name;
        private readonly string email;
        private readonly string phone;
        private readonly string address;

        //locators
        private By nameField = By.Id("name");
        private By emailField = By.XPath("//input[@id='email']");
        private By phoneField = By.CssSelector("#phone");
        private By addressFIeld = By.Id("textarea");
        private By genderRadioButton = By.Id("male");
        //private By daysCheckBox = By.Id("sunday");
        private By countryDDL = By.Id("country");
        private By datePicker1 = By.Id("datepicker");

        //constructor - read TestData.json
        public GUIElements(IWebDriver driver) {

            this.driver = driver;
            var jsonFilePath = @"C:\Users\user\Documents\Automation\automation-exercise\c#_selenium\csharp-selenium-practice\TestData.json";
            var jsonData = File.ReadAllText(jsonFilePath);
            var testData = JObject.Parse(jsonData);

            url = testData["url"].ToString();
            name = testData["name"].ToString();
            email = testData["email"].ToString();
            phone = testData["phone"].ToString();
            address = testData["address"].ToString();
        }

        public void LaunchURL() {
            Console.WriteLine("Navigating to URL...");
            driver.Navigate().GoToUrl(url);
        }

        public void EnterTextField() {
            Console.WriteLine("Entering Text Fields...");
            driver.FindElement(nameField).SendKeys(name);
            driver.FindElement(emailField).SendKeys(email);
            driver.FindElement(phoneField).SendKeys(phone);
            driver.FindElement(addressFIeld).SendKeys(address);            
        }

        public void ClickRadioButton() {
            Console.WriteLine("Clicking radio button...");
            driver.FindElement(genderRadioButton).Click();
        }

        public void CheckBox(string day)
        {
            Console.WriteLine($"Selecting {day} checkbox...");
            By dayCheckbox = By.Id(day.ToLower());
            driver.FindElement(dayCheckbox).Click();
        }

        public void selectDDL(string optionText) {
            Console.WriteLine($"Selecting {optionText} from drop down list");
            IWebElement ddlElement = driver.FindElement(countryDDL);

            SelectElement select = new SelectElement(ddlElement);
            select.SelectByText(optionText);
        }

        public void PickDateFromDatePicker(string date)
        {
            Console.WriteLine($"Selecting {date} from Date Picker...");
            // Click to open the DatePicker
            driver.FindElement(datePicker1).Click();

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
            IWebElement yearElement = driver.FindElement(By.XPath("//div[contains(@class, 'ui-datepicker')]//span[@class='ui-datepicker-year']"));
            string currentYear = yearElement.Text;

            IWebElement monthElement = driver.FindElement(By.XPath("//div[contains(@class, 'ui-datepicker')]//span[@class='ui-datepicker-month']"));
            string currentMonth = monthElement.Text;

            // Navigate to the correct year
            while (currentYear != year.ToString())
            {
                if (int.Parse(currentYear) < year)
                {
                    // Click "Next" to go forward
                    IWebElement nextButton = driver.FindElement(By.XPath("//a[@class='ui-datepicker-next ui-corner-all']"));
                    nextButton.Click();
                    Thread.Sleep(1000); // Adjust with WebDriverWait
                }
                else if (int.Parse(currentYear) > year)
                {
                    // Click "Previous" to go back
                    IWebElement prevButton = driver.FindElement(By.XPath("//a[@class='ui-datepicker-prev ui-corner-all']"));
                    prevButton.Click();
                    Thread.Sleep(1000); // Adjust with WebDriverWait
                }

                currentYear = driver.FindElement(By.XPath("//div[contains(@class, 'ui-datepicker')]//span[@class='ui-datepicker-year']")).Text;
            }

            // Now, select the month
            string targetMonth = monthNames[month - 1]; // Get the month name
            if (currentMonth != targetMonth)
            {
                // Navigate to the desired month
                while (currentMonth != targetMonth)
                {
                    IWebElement nextButton = driver.FindElement(By.XPath("//a[@class='ui-datepicker-next ui-corner-all']"));
                    nextButton.Click();
                    Thread.Sleep(1000); // Adjust with WebDriverWait
                    currentMonth = driver.FindElement(By.XPath("//div[contains(@class, 'ui-datepicker')]//span[@class='ui-datepicker-month']")).Text;
                }
            }

            // Select the correct day
            IWebElement dayElement = driver.FindElement(By.XPath($"//a[contains(@class, 'ui-state-default') and contains(text(), '{day}')]"));
            dayElement.Click();
        }
    }
}