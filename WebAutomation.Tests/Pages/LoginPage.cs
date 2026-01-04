using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Utilities;
using WebAutomation.Core.Security;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class LoginPage : BasePage
    {
        private readonly LocatorRepository _repo;

        public LoginPage(IWebDriver driver) : base(driver)
        {
            _repo = new LocatorRepository("Locators.txt");
        }

        public bool IsPageReady()
        {
            return Wait.UntilPresent(_repo.GetBy("Login.PageReady"));
        }

        public void Login(string username, string password)
        {
            Wait.UntilVisible(_repo.GetBy("Login.Username")).SendKeys(username);
            Wait.UntilVisible(_repo.GetBy("Login.Password")).SendKeys(password);
            Wait.UntilClickable(_repo.GetBy("Login.Submit.Button")).Click();
        }

        public void HandleMfa()
        {
            if (Wait.UntilPresent(_repo.GetBy("Mfa.Dialog"), 5))
            {
                Wait.UntilClickable(_repo.GetBy("Mfa.EmailMethod.Select")).Click();
                Thread.Sleep(500);
                Wait.UntilClickable(_repo.GetBy("Mfa.SendCode.Button")).Click();
            }
        }

        public void EnterOtpAndVerify()
        {
            var otp = ConfigManager.Settings.StaticOtp;
            Wait.UntilVisible(_repo.GetBy("Otp.Code.Input")).SendKeys(otp);
            Wait.UntilClickable(_repo.GetBy("Otp.Verify.Button")).Click();
        }
    }
}