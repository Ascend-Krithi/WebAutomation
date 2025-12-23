using OpenQA.Selenium;
using WebAutomation.Core.Security;
using WebAutomation.Core.Locators;

namespace WebAutomation.Tests.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly LocatorRepository _locators;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            _locators = new LocatorRepository("Locators.txt");
        }

        public bool IsPageReady()
        {
            return _driver.FindElements(_locators.GetBy("Login.PageReady")).Count > 0;
        }

        public void LoginWithDefaultCredentials()
        {
            var (username, password) = CredentialProvider.GetDefaultCredentials();
            _driver.FindElement(_locators.GetBy("Login.Username")).SendKeys(username);
            _driver.FindElement(_locators.GetBy("Login.Password")).SendKeys(password);
            _driver.FindElement(_locators.GetBy("Login.Submit.Button")).Click();
        }
    }
}