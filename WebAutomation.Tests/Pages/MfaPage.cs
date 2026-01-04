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
            _repo = new LocatorRepository("Locators.txt");
        }

        public void CompleteMfa()
        {
            Wait.UntilVisible(_repo.GetBy("Mfa.Dialog"));
            Wait.UntilClickable(_repo.GetBy("Mfa.EmailMethod.Select")).Click();
            Wait.UntilClickable(_repo.GetBy("Mfa.SendCode.Button")).Click();
            Wait.UntilVisible(_repo.GetBy("Otp.Code.Input")).SendKeys(ConfigManager.Settings.StaticOtp);
            Wait.UntilClickable(_repo.GetBy("Otp.Verify.Button")).Click();
        }
    }
}