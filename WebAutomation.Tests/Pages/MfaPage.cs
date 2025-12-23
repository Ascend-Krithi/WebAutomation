using OpenQA.Selenium;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Utilities;
using WebAutomation.Core.Configuration;

namespace WebAutomation.Tests.Pages
{
    public class MfaPage : BasePage
    {
        private readonly LocatorRepository _locators;

        public MfaPage(IWebDriver driver) : base(driver)
        {
            _locators = new LocatorRepository("Locators.txt");
        }

        public bool IsDialogPresent()
        {
            return Wait.UntilPresent(_locators.GetBy("Mfa.Dialog"));
        }

        public void CompleteMfa()
        {
            Wait.UntilClickable(_locators.GetBy("Mfa.EmailMethod.Select")).Click();
            Wait.UntilClickable(_locators.GetBy("Mfa.SendCode.Button")).Click();
            Wait.UntilVisible(_locators.GetBy("Otp.Code.Input")).SendKeys(ConfigManager.Settings.StaticOtp);
            Wait.UntilClickable(_locators.GetBy("Otp.Verify.Button")).Click();
        }
    }
}