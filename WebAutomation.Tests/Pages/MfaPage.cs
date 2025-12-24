using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;

namespace WebAutomation.Tests.Pages
{
    public class MfaPage : BasePage
    {
        private readonly LocatorRepository _locators = new LocatorRepository("Locators.txt");

        public By DialogLocator => _locators.GetBy("Mfa.Dialog");
        private By EmailMethodSelectLocator => _locators.GetBy("Mfa.EmailMethod.Select");
        private By SendCodeButtonLocator => _locators.GetBy("Mfa.SendCode.Button");

        public MfaPage(IWebDriver driver) : base(driver) { }

        public void SelectFirstEmailAndSendCode()
        {
            Wait.UntilClickable(EmailMethodSelectLocator).Click();
            // Select first option in dropdown
            var options = Driver.FindElements(By.CssSelector("mat-option"));
            if (options.Count > 0)
                options[0].Click();
            Wait.UntilClickable(SendCodeButtonLocator).Click();
        }
    }
}