using OpenQA.Selenium;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Pages;

namespace WebAutomation.Tests.Pages
{
    public class LoginPage : BasePage
    {
        private readonly LocatorRepository _repo;

        public LoginPage(IWebDriver driver) : base(driver)
        {
            _repo = new LocatorRepository("Locators.txt");
        }

        public void NavigateToLoginPage()
        {
            Driver.Navigate().GoToUrl(WebAutomation.Core.Configuration.ConfigManager.Settings.BaseUrl);
            Wait.UntilVisible(_repo.GetBy("Login.PageReady"));
        }

        public void Login(string loanNumber, string state)
        {
            // For demo, using loanNumber as username and state as password
            Wait.UntilVisible(_repo.GetBy("Login.Username")).SendKeys(loanNumber);
            Wait.UntilVisible(_repo.GetBy("Login.Password")).SendKeys(state);
            Wait.UntilClickable(_repo.GetBy("Login.Submit.Button")).Click();
        }
    }
}