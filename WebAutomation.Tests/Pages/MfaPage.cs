using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;

namespace WebAutomation.Tests.Pages
{
    public class MfaPage : BasePage
    {
        private readonly LocatorRepository _repo;

        public MfaPage(IWebDriver driver) : base(driver)
        {
            _repo = new LocatorRepository("Locators.txt");
        }

        public By PageReadyLocator() => _repo.GetBy("Mfa.Dialog");

        public void SelectFirstEmailAndSendCode()
        {
            Driver.FindElement(_repo.GetBy("Mfa.EmailMethod.Select")).Click();
            // Select first option in dropdown
            Driver.FindElements(By.CssSelector("mat-option"))[0].Click();
            Driver.FindElement(_repo.GetBy("Mfa.SendCode.Button")).Click();
        }
    }
}