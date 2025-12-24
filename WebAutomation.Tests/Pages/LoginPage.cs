using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Security;
using WebAutomation.Core.Locators;

namespace WebAutomation.Tests.Pages
{
    public class LoginPage : BasePage
    {
        private readonly LocatorRepository _locators = new LocatorRepository("Locators.txt");

        public By PageReadyLocator => _locators.GetBy("Login.PageReady");
        private By UsernameLocator => _locators.GetBy("Login.Username");
        private By PasswordLocator => _locators.GetBy("Login.Password");
        private By SubmitButtonLocator => _locators.GetBy("Login.Submit.Button");

        public LoginPage(IWebDriver driver) : base(driver) { }

        public void LoginWithDefaultCredentials()
        {
            var (username, password) = CredentialProvider.GetDefaultCredentials();
            Wait.UntilVisible(UsernameLocator).SendKeys(username);
            Wait.UntilVisible(PasswordLocator).SendKeys(password);
            Wait.UntilClickable(SubmitButtonLocator).Click();
        }
    }
}