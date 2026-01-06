using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Configuration;

namespace WebAutomation.Tests.Pages
{
    public class MfaPage : BasePage
    {
        private readonly LocatorRepository _locators = new LocatorRepository("Locators.txt");

        public MfaPage(IWebDriver driver) : base(driver) { }

        public void CompleteMfa()
        {
            Wait.UntilVisible(_locators.GetBy("Mfa.Dialog"));
            Driver.FindElement(_locators.GetBy("Mfa.EmailMethod.Select")).Click();
            Driver.FindElement(_locators.GetBy("Mfa.SendCode.Button")).Click();
            Wait.UntilVisible(_locators.GetBy("Otp.Code.Input"));
            Driver.FindElement(_locators.GetBy("Otp.Code.Input")).SendKeys(ConfigManager.Settings.StaticOtp);
            Driver.FindElement(_locators.GetBy("Otp.Verify.Button")).Click();
        }
    }
}