using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Utilities;
using WebAutomation.Core.Security;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class DashboardPage : BasePage
    {
        private readonly LocatorRepository _locators;

        public DashboardPage(IWebDriver driver)
            : base(driver)
        {
            _locators = new LocatorRepository("Locators.txt");
        }

        public bool IsLoginPageDisplayed()
        {
            return Wait.UntilPresent(_locators.GetBy("Login.PageReady"));
        }

        public void Login(string username, string password)
        {
            Wait.UntilVisible(_locators.GetBy("Login.Username")).SendKeys(username);
            Wait.UntilVisible(_locators.GetBy("Login.Password")).SendKeys(password);
            Wait.UntilClickable(_locators.GetBy("Login.Submit.Button")).Click();

            // MFA Page
            Wait.UntilVisible(_locators.GetBy("Mfa.Dialog"));
            Wait.UntilClickable(_locators.GetBy("Mfa.EmailMethod.Select")).Click();
            Wait.UntilClickable(_locators.GetBy("Mfa.SendCode.Button")).Click();

            // OTP Page
            var staticOtp = WebAutomation.Core.Configuration.ConfigManager.Settings.StaticOtp;
            Wait.UntilVisible(_locators.GetBy("Otp.Code.Input")).SendKeys(staticOtp);
            Wait.UntilClickable(_locators.GetBy("Otp.Verify.Button")).Click();

            // Handle popups defensively
            Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactUpdateLater"));
            Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactContinue"));
        }

        public bool IsDashboardDisplayed()
        {
            return Wait.UntilPresent(_locators.GetBy("Dashboard.PageReady"));
        }

        public void ClickMakePayment()
        {
            Wait.UntilClickable(_locators.GetBy("Dashboard.MakePayment.Button")).Click();
        }

        public void ClickSetupAutopay()
        {
            // Assuming Setup Autopay button locator is similar to MakePayment, update if needed
            Wait.UntilClickable(_locators.GetBy("Dashboard.SetupAutopay.Button")).Click();
        }

        public bool IsPendingOtpPopupDisplayed()
        {
            return Popup.IsPresent(_locators.GetBy("Dashboard.ContactPopup"));
        }

        public void ClickPopupContinue()
        {
            Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactContinue"));
        }

        public void ClickPopupCancel()
        {
            Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactUpdateLater"));
        }

        public bool IsMakePaymentPageDisplayed()
        {
            // Use Payment.PageReady locator
            return Wait.UntilPresent(_locators.GetBy("Payment.PageReady"));
        }

        public bool IsSetupAutopayPageDisplayed()
        {
            // Assuming Setup Autopay page has a unique locator, update if needed
            return Wait.UntilPresent(_locators.GetBy("Autopay.PageReady"));
        }
    }
}