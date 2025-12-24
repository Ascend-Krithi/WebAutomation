using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Configuration;

namespace WebAutomation.Tests.Pages
{
    public class OtpPage : BasePage
    {
        private readonly LocatorRepository _locators = new LocatorRepository("Locators.txt");

        public By CodeInputLocator => _locators.GetBy("Otp.Code.Input");
        private By VerifyButtonLocator => _locators.GetBy("Otp.Verify.Button");

        public OtpPage(IWebDriver driver) : base(driver) { }

        public void EnterStaticOtpAndVerify()
        {
            var otp = ConfigManager.Settings.StaticOtp;
            Wait.UntilVisible(CodeInputLocator).SendKeys(otp);
            Wait.UntilClickable(VerifyButtonLocator).Click();
        }
    }
}