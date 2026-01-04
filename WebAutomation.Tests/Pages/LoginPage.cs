using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Security;

namespace WebAutomation.Tests.Pages
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver) { }

        public void LoginWithDefaultCredentials()
        {
            var creds = CredentialProvider.GetDefaultCredentials();
            Wait.UntilVisible(By.Id("email")).SendKeys(creds.Username);
            Wait.UntilVisible(By.Id("password")).SendKeys(creds.Password);
            Wait.UntilClickable(By.CssSelector("button[type='submit']")).Click();
        }
    }
}