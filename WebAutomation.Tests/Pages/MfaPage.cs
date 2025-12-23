using OpenQA.Selenium;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Pages;

namespace WebAutomation.Tests.Pages
{
    public class MfaPage : BasePage
    {
        private readonly LocatorRepository _repo = new LocatorRepository("Locators.txt");

        public MfaPage(IWebDriver driver) : base(driver) { }

        public bool IsDialogDisplayed()
        {
            return Wait.UntilPresent(_repo.GetBy("Mfa.Dialog"));
        }

        public void SelectFirstEmailAndSendCode()
        {
            var select = Driver.FindElement(_repo.GetBy("Mfa.EmailMethod.Select"));
            select.Click();
            select.SendKeys(Keys.ArrowDown);
            select.SendKeys(Keys.Enter);
            Driver.FindElement(_repo.GetBy("Mfa.SendCode.Button")).Click();
        }
    }
}