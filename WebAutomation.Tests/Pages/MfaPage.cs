using OpenQA.Selenium;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Pages;

namespace WebAutomation.Tests.Pages
{
    public class MfaPage : BasePage
    {
        private readonly LocatorRepository _repo;

        public MfaPage(IWebDriver driver) : base(driver)
        {
            _repo = new LocatorRepository("Locators.txt");
        }

        public bool IsPageReady()
        {
            return Wait.UntilPresent(_repo.GetBy("Mfa.Dialog"));
        }

        public void SelectFirstEmailAndSendCode()
        {
            Driver.FindElement(_repo.GetBy("Mfa.EmailMethod.Select")).Click();
            var options = Driver.FindElements(By.CssSelector("mat-option"));
            if (options.Count > 0)
            {
                options[0].Click();
            }
            Driver.FindElement(_repo.GetBy("Mfa.SendCode.Button")).Click();
        }
    }
}