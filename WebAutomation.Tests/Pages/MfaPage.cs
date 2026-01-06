using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;

namespace WebAutomation.Tests.Pages
{
    public class MfaPage : BasePage
    {
        private readonly LocatorRepository _locators = new LocatorRepository("Locators.txt");

        public MfaPage(IWebDriver driver) : base(driver) { }

        public void SelectFirstEmailMethod()
        {
            Wait.UntilClickable(_locators.GetBy("Mfa.EmailMethod.Select")).Click();
            Driver.FindElement(_locators.GetBy("Mfa.EmailMethod.Select")).SendKeys(Keys.ArrowDown + Keys.Enter);
        }

        public void ClickReceiveCode()
        {
            Wait.UntilClickable(_locators.GetBy("Mfa.SendCode.Button")).Click();
        }
    }
}