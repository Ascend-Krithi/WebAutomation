using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Configuration;

namespace WebAutomation.Tests.Pages
{
    public class MfaPage : BasePage
    {
        private readonly LocatorRepository _repo;

        public MfaPage(IWebDriver driver) : base(driver)
        {
            _repo = new LocatorRepository("Locators.json");
        }

        public void CompleteMfa()
        {
            Wait.UntilVisible(_repo.GetBy("Mfa.Dialog"));
            Driver.FindElement(_repo.GetBy("Mfa.EmailMethod.Select")).Click();
            Driver.FindElement(_repo.GetBy("Mfa.SendCode.Button")).Click();
            Wait.UntilVisible(_repo.GetBy("Otp.Code.Input"));
            Driver.FindElement(_repo.GetBy("Otp.Code.Input")).SendKeys(ConfigManager.Settings.StaticOtp);
            Driver.FindElement(_repo.GetBy("Otp.Verify.Button")).Click();
        }
    }
}