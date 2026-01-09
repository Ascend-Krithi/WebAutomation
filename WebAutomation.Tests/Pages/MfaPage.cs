using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;

namespace WebAutomation.Tests.Pages
{
    public class MfaPage : BasePage
    {
        private readonly LocatorRepository _locators;

        public By DialogLocator => _locators.GetBy("Mfa.Dialog");

        public MfaPage(IWebDriver driver) : base(driver)
        {
            _locators = new LocatorRepository("Locators.json");
        }

        public void SelectFirstEmailMethod()
        {
            Wait.UntilClickable(_locators.GetBy("Mfa.EmailMethod.Select")).Click();
            // Select first option in dropdown
            Driver.FindElements(By.CssSelector("mat-option"))[0].Click();
        }

        public void ClickSendCode()
        {
            Wait.UntilClickable(_locators.GetBy("Mfa.SendCode.Button")).Click();
        }
    }
}