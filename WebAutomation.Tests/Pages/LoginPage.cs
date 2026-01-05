using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Security;
using WebAutomation.Core.Locators;

namespace WebAutomation.Tests.Pages
{
    public class LoginPage : BasePage
    {
        private readonly LocatorRepository _repo;

        public LoginPage(IWebDriver driver) : base(driver)
        {
            _repo = new LocatorRepository("Locators.txt");
        }

        public By PageReadyLocator() => _repo.GetBy("Login.PageReady");

        public void LoginWithDefaultCredentials()
        {
            var creds = CredentialProvider.GetDefaultCredentials();
            Driver.FindElement(_repo.GetBy("Login.Username")).SendKeys(creds.Username);
            Driver.FindElement(_repo.GetBy("Login.Password")).SendKeys(creds.Password);
            Driver.FindElement(_repo.GetBy("Login.Submit.Button")).Click();
        }
    }
}