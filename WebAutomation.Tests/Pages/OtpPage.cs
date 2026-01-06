using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Configuration;

namespace WebAutomation.Tests.Pages
{
    public class OtpPage : BasePage
    {
        private readonly LocatorRepository _locators = new LocatorRepository("Locators.txt");

        public OtpPage(IWebDriver driver) : base(driver) { }

        public void EnterStaticOtpAndVerify()
        {
            Wait.UntilVisible(_locators.GetBy("Otp.Code.Input")).SendKeys(ConfigManager.Settings.StaticOtp);
            Wait.UntilClickable(_locators.GetBy("Otp.Verify.Button")).Click();
        }
    }
}