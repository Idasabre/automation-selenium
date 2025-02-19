using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
//using Newtonsoft.Json.Linq;

namespace csharp_selenium_practice.Pages
{
    public class UploadFiles {
        private readonly IWebDriver driver;
        //private string url = "https://testautomationpractice.blogspot.com/";
        private string url;
        private string filePath1 = @"C:\Users\user\Documents\Automation\automation-exercise\c#_selenium\csharp-selenium-practice\TestSingleUpload.txt";
        private string filePath2 = @"C:\Users\user\Documents\Automation\automation-exercise\c#_selenium\csharp-selenium-practice\TestMultipleUpload.txt";
        
        //locators
        private By singleUploadButton = By.Id("singleFileInput");

        private By multUploadButton = By.Id("multipleFilesInput");
        private By submitSingleUploadButton = By.XPath("//button[text()='Upload Single File']");
        private By submitMultipleUploadButton = By.XPath("//button[text()='Upload Multiple Files']");

        public UploadFiles(IWebDriver driver) {
            this.driver = driver;
            
            var jsonPath = @"C:\Users\user\Documents\Automation\automation-exercise\c#_selenium\csharp-selenium-practice\TestData.json";
            var jsonData = File.ReadAllText(jsonPath);
            var testData = Newtonsoft.Json.Linq.JObject.Parse(jsonData);

            url = testData["url"].ToString();
        }

        public void LaunchURL() {
            Console.WriteLine("Navigating to URL...");
            driver.Navigate().GoToUrl(url);
        }

        public void UploadSingleFile() {

            Console.WriteLine("Uploading single file...");

            Actions actions = new Actions(driver);
            
            string filePath = Path.GetFullPath(filePath1);
            
            IWebElement singleUpload = driver.FindElement(singleUploadButton);
            actions.MoveToElement(singleUpload);
            singleUpload.SendKeys(filePath);
            Thread.Sleep(2000);
            driver.FindElement(submitSingleUploadButton).Click();
        }


        public void UploadMultipleFile() {

            Console.WriteLine("Uploading multiple file...");

            Actions actions = new Actions(driver);
            
            string multiFile1 = Path.GetFullPath(filePath1);
            string multiFile2 = Path.GetFullPath(filePath2);
            
            IWebElement multipleUpload = driver.FindElement(multUploadButton);
            actions.MoveToElement(multipleUpload);
            // Upload multiple files (separated by newline in Windows)
            multipleUpload.SendKeys(multiFile1 + "\n" + multiFile2);
            Thread.Sleep(2000);
            driver.FindElement(submitMultipleUploadButton).Click();
        }
    }
} 