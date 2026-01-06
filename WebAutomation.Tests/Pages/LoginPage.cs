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
            var (username, password) = CredentialProvider.GetDefaultCredentials();
            Driver.FindElement(By.Id("email")).SendKeys(username);
            Driver.FindElement(By.Id("password")).SendKeys(password);
            Driver.FindElement(By.XPath("//button[contains(text(),'Sign in')]")).Click();
        }
    }
}