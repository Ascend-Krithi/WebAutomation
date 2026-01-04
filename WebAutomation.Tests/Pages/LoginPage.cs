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

        public void LoginWithDefaultCredentials()
        {
            var creds = CredentialProvider.GetDefaultCredentials();
            Wait.UntilVisible(_repo.GetBy("Login.Username")).SendKeys(creds.Username);
            Wait.UntilVisible(_repo.GetBy("Login.Password")).SendKeys(creds.Password);
            Wait.UntilClickable(_repo.GetBy("Login.Submit.Button")).Click();
        }
    }
}