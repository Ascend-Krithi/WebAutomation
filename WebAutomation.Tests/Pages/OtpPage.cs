using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;

namespace WebAutomation.Tests.Pages
{
    public class OtpPage : BasePage
    {
        private readonly LocatorRepository _locators;

        public OtpPage(IWebDriver driver) : base(driver)
        {
            _locators = new LocatorRepository("Locators.json");
        }

        public void EnterOtpAndVerify()
        {
            var staticOtp = WebAutomation.Core.Configuration.ConfigManager.Settings.StaticOtp;
            Wait.UntilVisible(_locators.GetBy("Otp.Code.Input")).SendKeys(staticOtp);
            Driver.FindElement(_locators.GetBy("Otp.Verify.Button")).Click();
        }
    }
}