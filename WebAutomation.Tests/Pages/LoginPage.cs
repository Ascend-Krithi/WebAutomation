using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Security;
using WebAutomation.Core.Locators;

namespace WebAutomation.Tests.Pages
{
    public class LoginPage : BasePage
    {
        private readonly LocatorRepository _repo = new LocatorRepository("Locators.json");

        public LoginPage(IWebDriver driver) : base(driver) { }

        public void LoginWithDefaultCredentials()
        {
            var (username, password) = CredentialProvider.GetDefaultCredentials();
            Wait.UntilVisible(_repo.GetBy("Login.Username")).SendKeys(username);
            Wait.UntilVisible(_repo.GetBy("Login.Password")).SendKeys(password);
            Wait.UntilClickable(_repo.GetBy("Login.Submit.Button")).Click();
        }
    }
}