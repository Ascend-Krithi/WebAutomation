using OpenQA.Selenium;
using WebAutomation.Core.Security;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Utilities;

namespace WebAutomation.Tests.Pages
{
    public class LoginPage : BasePage
    {
        private readonly LocatorRepository _locators;

        public LoginPage(IWebDriver driver) : base(driver)
        {
            _locators = new LocatorRepository("Locators.txt");
        }

        public bool IsPageReady()
        {
            return Wait.UntilPresent(_locators.GetBy("Login.PageReady"));
        }

        public void LoginWithDefaultCredentials()
        {
            var creds = CredentialProvider.GetDefaultCredentials();
            Driver.FindElement(_locators.GetBy("Login.Username")).SendKeys(creds.Username);
            Driver.FindElement(_locators.GetBy("Login.Password")).SendKeys(creds.Password);
            Driver.FindElement(_locators.GetBy("Login.Submit.Button")).Click();
        }
    }
}