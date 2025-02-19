using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using csharp_selenium_practice.Pages;

namespace csharp_selenium_practice;

public class Tests
{
    private IWebDriver driver;
    public static IWebDriver? Driver { get; set; }
    private GUIElements guiEle;
    private DatePicker datePicker;
    private UploadFiles uploadFile;
    private StaticTable readTable;
    private PaginationWebTable paginationWebTable;

    

    [SetUp]
    public void Setup()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");  // Open browser in full screen
        options.AddArgument("--disable-infobars"); // Remove Chrome info bar

        driver = new ChromeDriver(options); // Launch Chrome browser

        guiEle = new GUIElements(driver); // Initialize the page object with the WebDriver instance
        datePicker = new DatePicker(driver);
        uploadFile = new UploadFiles(driver);
        readTable = new StaticTable(driver);
        paginationWebTable = new PaginationWebTable(driver);
    }

   [Test]
    public void TestGUIElements()
    {
        //Assert.Pass();
        guiEle.LaunchURL();
        Thread.Sleep(2000);
        guiEle.EnterTextField();
        guiEle.ClickRadioButton();
        guiEle.CheckBox("sunday");
        guiEle.selectDDL("France");
        //guiEle.PickDateFromDatePicker("02/17/2025");
        Thread.Sleep(2000);
    }

    [Test]
    public void TestDatePicker() {    
        guiEle.LaunchURL();
        Thread.Sleep(2000);    
        datePicker.PickDateFromDatePicker1("02/17/2025");
        Thread.Sleep(2000);    
    }

    [Test]
    public void TestUploadFile() {
        uploadFile.LaunchURL();
        Thread.Sleep(2000);  
        uploadFile.UploadSingleFile();
        uploadFile.UploadMultipleFile();
        Thread.Sleep(2000);
    }

    [Test]
    public void TestTable() {
        guiEle.LaunchURL();
        Thread.Sleep(2000);
        // readTable.readRows();
        // readTable.readCells();
        // readTable.readDataFromCells("Learn Java");
        paginationWebTable.CheckProductByName("VR Headset");
        Thread.Sleep(2000);
    }

    [TearDown]  
    public void TearDown()
    {
        Console.WriteLine("Closing browser...");
        driver.Quit();  // Close browser
        driver.Dispose(); // Release WebDriver resources
    }
}

//run test by test name: dotnet test --filter "TestGUIElements"
