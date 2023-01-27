using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V107.Debugger;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;


var cred = File.ReadAllLines("cred.txt").ToList();

System.Console.WriteLine("Sit tight, let me do all the work....");


new DriverManager().SetUpDriver(new ChromeConfig());
var driver = new ChromeDriver();


driver.Navigate().GoToUrl("https://heart.globalservice.co.id/");
driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(500);

var username = driver.FindElement(By.Name("username"));
var password = driver.FindElement(By.Name("password"));
var loginSubmitBtn = driver.FindElement(By.TagName("button"));

username.SendKeys(cred[0]);
password.SendKeys(cred[1]);
loginSubmitBtn.Click();

driver.Navigate().GoToUrl("https://heart.globalservice.co.id/staff/absensi");

var mood = driver.FindElement(By.CssSelector("[src*='https://heart.globalservice.co.id/assets/img/emoji/3.png']"));
var desc = driver.FindElement(By.Id("note"));
var submitBtn = driver.FindElement(By.CssSelector("[type*='submit']"));

desc.SendKeys($"{submitBtn.Text} at {Round(DateTime.Now, TimeSpan.FromMinutes(1)).ToString("H:mm")}");
mood.Click();

submitBtn.Click();

Thread.Sleep(3000);
driver.Quit();
return;

static DateTime Round(DateTime date, TimeSpan interval) {
    return new DateTime(
        (long)Math.Round(date.Ticks / (double)interval.Ticks) * interval.Ticks
    );
}