using OpenQA.Selenium;
using WebAutomation.Core.Security;
using WebAutomation.Core.Utilities;

namespace WebAutomation.Tests.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly SmartWait _wait;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new SmartWait(driver);
        }

        public bool IsPageReady()
        {
            return _wait.UntilPresent(By.CssSelector("form"), 10);
        }

        public void LoginWithDefaultCredentials()
        {
            var creds = CredentialProvider.GetDefaultCredentials();
            _wait.UntilVisible(By.Id("email")).SendKeys(creds.Username);
            _wait.UntilVisible(By.Id("password")).SendKeys(creds.Password);
            _wait.UntilClickable(By.CssSelector("button[type='submit']")).Click();
        }
    }
}