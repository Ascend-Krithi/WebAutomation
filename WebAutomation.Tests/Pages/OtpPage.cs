using OpenQA.Selenium;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Pages;
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

        public bool IsPageReady()
        {
            return Wait.UntilPresent(_repo.GetBy("Otp.Code.Input"));
        }

        public void EnterStaticOtpAndVerify()
        {
            Driver.FindElement(_repo.GetBy("Otp.Code.Input")).SendKeys(ConfigManager.Settings.StaticOtp);
            Driver.FindElement(_repo.GetBy("Otp.Verify.Button")).Click();
        }
    }
}