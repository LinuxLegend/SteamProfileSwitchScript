using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

var mainDriver = new FirefoxDriver();
var loggedIn = false;

// Go to login
mainDriver.Url = "https://steamcommunity.com/login/home/";
mainDriver.Navigate();

if (File.Exists("Cookie.txt"))
{
    // Using saved cookie to avoid re-login
    var content = await File.ReadAllLinesAsync("Cookie.txt");
    var cookie = new Cookie(content[1], content[3], content[0], content[2],
        string.IsNullOrWhiteSpace(content[5]) ? null :  DateTime.FromBinary(long.Parse(content[5])),
        content[6] != "false", content[7] != "false", content[5]);
    mainDriver.Manage().Cookies.AddCookie(cookie);

    // Navigate to profile
    mainDriver.Url = content[8];
    mainDriver.Navigate();

    // Wait about 5 seconds
    await Task.Delay(5000);
    loggedIn = true;
}

if (!loggedIn)
{
    // Now fill in login details
    var usernameBox = mainDriver.FindElement(By.Id("input_username"));
    usernameBox.Clear();
    usernameBox.Click();
    usernameBox.SendKeys("Enter Username Here");

    var passwordBox = mainDriver.FindElement(By.Id("input_password"));
    passwordBox.Clear();
    passwordBox.Click();
    passwordBox.SendKeys("Enter Password Here");

    var loginButton =
        mainDriver.FindElement(
            By.XPath("/html/body/div[1]/div[7]/div[2]/div[1]/div/div[1]/div/div/div/div/div[3]/div[1]/button"));
    loginButton.Click();

    // Wait about 1 minute to allow for Two Factors Authentication steps
    await Task.Delay(60000);

    var loginCookie = mainDriver.Manage().Cookies.GetCookieNamed("steamLoginSecure");
    await File.WriteAllLinesAsync("Cookie.txt", new[]
    {
        loginCookie.Domain,
        loginCookie.Name,
        loginCookie.Path,
        loginCookie.Value,
        loginCookie.SameSite,
        loginCookie.Expiry?.ToBinary().ToString(),
        loginCookie.Secure.ToString().ToLower(),
        loginCookie.IsHttpOnly.ToString().ToLower(),
        mainDriver.Url
    }!);
}

// Clicking on Edit Profile hyperlink
var profileEditButton =
    mainDriver.FindElement(By.XPath("/html/body/div[1]/div[7]/div[6]/div[1]/div[1]/div/div/div/div[3]/div[2]/a"));
profileEditButton.Click();

// Wait about 5 seconds
await Task.Delay(5000);

// Click on profile background
var profileBackgroundButton =
    mainDriver.FindElement(By.XPath("/html/body/div[1]/div[7]/div[3]/div/div[2]/div/div/div[3]/div[2]/div[1]/a[3]"));
profileBackgroundButton.Click();
await Task.Delay(5000);

string[] imageUrls =
{
    "https://community.cloudflare.steamstatic.com/economy/profilebackground/items/392110/8df3da908ad460660c6d6bb525f1d22f7b35862e.jpg?size=252x160",
    "https://community.cloudflare.steamstatic.com/public/images/profile/2020/bg_dots.png"
};

// Add whatever logic to pick out image
var selectedImage = imageUrls[0];

// Select the image from list...
var img = mainDriver.FindElements(By.TagName("img")).FirstOrDefault(I => I.GetAttribute("src") == selectedImage);
if (img == null)
    return;
img.Click();
await Task.Delay(1000);

// Now save selection...
var saveButton = mainDriver.FindElement(By.XPath("/html/body/div[1]/div[7]/div[3]/div/div[2]/div/div/div[3]/div[2]/div[2]/div[3]/div/div[3]/button[1]"));
saveButton.Click();
await Task.Delay(5000);

// All done!
