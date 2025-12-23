using OpenQA.Selenium;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Configuration;

namespace WebAutomation.Tests.Pages
{
    public class MfaPage
    {
        private readonly IWebDriver _driver;
        private readonly LocatorRepository _locators;

        public MfaPage(IWebDriver driver)
        {
            _driver = driver;
            _locators = new LocatorRepository("Locators.txt");
        }

        public void CompleteMfa()
        {
            if (_driver.FindElements(_locators.GetBy("Mfa.Dialog")).Count > 0)
            {
                _driver.FindElement(_locators.GetBy("Mfa.EmailMethod.Select")).Click();
                _driver.FindElement(_locators.GetBy("Mfa.SendCode.Button")).Click();
                _driver.FindElement(_locators.GetBy("Otp.Code.Input")).SendKeys(ConfigManager.Settings.StaticOtp);
                _driver.FindElement(_locators.GetBy("Otp.Verify.Button")).Click();
            }
        }
    }
}