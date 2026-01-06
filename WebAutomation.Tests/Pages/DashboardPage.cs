using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;

namespace WebAutomation.Tests.Pages
{
    public class DashboardPage : BasePage
    {
        private readonly LocatorRepository _locators = new LocatorRepository("Locators.txt");

        public DashboardPage(IWebDriver driver) : base(driver) { }

        public void WaitForPageReady()
        {
            Wait.UntilVisible(_locators.GetBy("Dashboard.PageReady"));
        }

        public void HandlePopupsIfPresent()
        {
            Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactUpdateLater"));
            Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactContinue"));
        }

        public void SelectLoanAccount(string loanNumber)
        {
            Wait.UntilClickable(_locators.GetBy("Dashboard.LoanSelector.Button")).Click();
            Wait.UntilClickable(_locators.GetBy("Dashboard.LoanCard.ByAccount", loanNumber)).Click();
        }

        public void ClickMakePayment()
        {
            Wait.UntilClickable(_locators.GetBy("Dashboard.MakePayment.Button")).Click();
        }

        public void HandleScheduledPaymentPopupIfPresent()
        {
            Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactContinue"));
        }
    }
}