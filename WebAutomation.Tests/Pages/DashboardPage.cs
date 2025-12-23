using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Security;
using WebAutomation.Core.Utilities;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class DashboardPage : BasePage
    {
        private readonly LocatorRepository _locators;

        public DashboardPage(IWebDriver driver) : base(driver)
        {
            _locators = new LocatorRepository("Locators.txt");
        }

        public bool IsSignInScreenDisplayed()
        {
            return Wait.UntilPresent(_locators.GetBy("Login.PageReady"));
        }

        public void LoginWithDefaultCredentials()
        {
            var creds = CredentialProvider.GetDefaultCredentials();
            Wait.UntilVisible(_locators.GetBy("Login.Username")).SendKeys(creds.Username);
            Wait.UntilVisible(_locators.GetBy("Login.Password")).SendKeys(creds.Password);
            Wait.UntilClickable(_locators.GetBy("Login.Submit.Button")).Click();
        }

        public bool IsMfaScreenDisplayed()
        {
            return Wait.UntilPresent(_locators.GetBy("Mfa.Dialog"));
        }

        public void CompleteMfaVerification()
        {
            Wait.UntilClickable(_locators.GetBy("Mfa.EmailMethod.Select")).Click();
            Wait.UntilClickable(_locators.GetBy("Mfa.SendCode.Button")).Click();
            Wait.UntilVisible(_locators.GetBy("Otp.Code.Input")).SendKeys(WebAutomation.Core.Configuration.ConfigManager.Settings.StaticOtp);
            Wait.UntilClickable(_locators.GetBy("Otp.Verify.Button")).Click();
            Wait.WaitForOverlayToClose();
        }

        public bool IsDashboardDisplayed()
        {
            return Wait.UntilPresent(_locators.GetBy("Dashboard.PageReady"));
        }

        public void DismissPopupsIfPresent()
        {
            Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactPopup"));
            Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactUpdateLater"));
            Popup.HandleIfPresent(_locators.GetBy("Chatbot.Iframe"));
        }

        public void SelectLoanAccount(string loanNumber)
        {
            Wait.UntilClickable(_locators.GetBy("Dashboard.LoanSelector.Button")).Click();
            Wait.UntilClickable(_locators.GetBy("Dashboard.LoanCard.ByAccount", loanNumber)).Click();
            Thread.Sleep(1000);
        }

        public bool IsLoanDetailsDisplayed(string loanNumber)
        {
            return Wait.UntilPresent(_locators.GetBy("Dashboard.LoanCard.ByAccount", loanNumber));
        }

        public void ClickMakePayment()
        {
            Wait.UntilClickable(_locators.GetBy("Dashboard.MakePayment.Button")).Click();
        }
    }
}