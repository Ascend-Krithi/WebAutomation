using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Security;
using WebAutomation.Core.Locators;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class LoginPage : BasePage
    {
        private readonly LocatorRepository _repo;

        public LoginPage(IWebDriver driver) : base(driver)
        {
            _repo = new LocatorRepository("Locators/Locators.txt");
        }

        public void LoginWithDefaultCredentials()
        {
            var (username, password) = CredentialProvider.GetDefaultCredentials();
            Wait.UntilVisible(_repo.GetBy("Login.PageReady"));
            Driver.FindElement(_repo.GetBy("Login.Username")).SendKeys(username);
            Driver.FindElement(_repo.GetBy("Login.Password")).SendKeys(password);
            Driver.FindElement(_repo.GetBy("Login.Submit.Button")).Click();
        }
    }
}