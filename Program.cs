using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

var driver = new ChromeDriver();
try
{
    // Read username and password from txt file
    string[] lines = File.ReadAllLines("cred.txt");
    string username = lines[0];
    string password = lines[1];

    // Choose browser

    // Step 1: Login
    driver.Navigate().GoToUrl("https://heart.globalservice.co.id");
    driver.FindElement(By.Name("username")).SendKeys(username);
    driver.FindElement(By.Name("password")).SendKeys(password);
    driver.FindElement(By.TagName("button")).Click();

    // Wait for dashboard page to load
    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    wait.Until(d => d.Url.Contains("dashboard"));

    // Redirect to Absensi page
    driver.Navigate().GoToUrl("https://heart.globalservice.co.id/staff/absensi");

    // Wait for Absensi page to load
    wait.Until(d => d.Url.Contains("absensi"));

    try
    {
        // Allow location permission
        IAlert alert = driver.SwitchTo().Alert();
        alert.Accept();
    }
    catch (NoAlertPresentException)
    {

    }

    // Step 2: Perform check-in
    var btnSubmit = driver.FindElement(By.CssSelector("[type*='submit']"));
    IWebElement noteElement = driver.FindElement(By.Id("note"));
    noteElement.Clear();
    noteElement.SendKeys($"{btnSubmit.Text} at {Round(DateTime.Now, TimeSpan.FromMinutes(1)).ToString("H:mm")}");
    Thread.Sleep(1000);

    IWebElement emojiElement = driver.FindElement(By.CssSelector("img[src='https://heart.globalservice.co.id/assets/img/emoji/3.png']"));
    emojiElement.Click();

    Thread.Sleep(10000);
    btnSubmit.Submit();
    WebDriverWait modalWait = new WebDriverWait(driver, TimeSpan.FromSeconds(100));
    modalWait.Until(d => d.FindElement(By.ClassName("swal-modal")).Displayed);

    // Close the modal by clicking the OK button
    IWebElement okButton = driver.FindElement(By.CssSelector(".swal-button--confirm"));
    okButton.Click();

    // Close the browser
    driver.Quit();
}
catch (System.Exception)
{
    driver.Quit();
}

DateTime Round(DateTime date, TimeSpan interval)
{
    return new DateTime(
        (long)Math.Round(date.Ticks / (double)interval.Ticks) * interval.Ticks
    );
}
