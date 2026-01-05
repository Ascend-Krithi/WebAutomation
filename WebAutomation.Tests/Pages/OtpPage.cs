using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Configuration;

namespace WebAutomation.Tests.Pages
{
    public class OtpPage : BasePage
    {
        private readonly LocatorRepository _repo;

        public OtpPage(IWebDriver driver) : base(driver)
        {
            _repo = new LocatorRepository("Locators.txt");
        }

        public void EnterStaticOtpAndVerify()
        {
            var otp = ConfigManager.Settings.StaticOtp;
            Driver.FindElement(_repo.GetBy("Otp.Code.Input")).SendKeys(otp);
            Driver.FindElement(_repo.GetBy("Otp.Verify.Button")).Click();
        }
    }
}