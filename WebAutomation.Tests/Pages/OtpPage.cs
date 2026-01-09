using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Configuration;

namespace WebAutomation.Tests.Pages
{
    public class OtpPage : BasePage
    {
        private readonly LocatorRepository _locators;

        public By CodeInputLocator => _locators.GetBy("Otp.Code.Input");

        public OtpPage(IWebDriver driver) : base(driver)
        {
            _locators = new LocatorRepository("Locators.json");
        }

        public void EnterStaticOtpAndVerify()
        {
            Wait.UntilVisible(_locators.GetBy("Otp.Code.Input")).SendKeys(ConfigManager.Settings.StaticOtp);
            Wait.UntilClickable(_locators.GetBy("Otp.Verify.Button")).Click();
        }
    }
}