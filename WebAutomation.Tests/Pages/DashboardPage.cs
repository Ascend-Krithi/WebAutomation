using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class DashboardPage : BasePage
    {
        private readonly LocatorRepository _locators = new LocatorRepository("Locators.txt");

        public By PageReadyLocator => _locators.GetBy("Dashboard.PageReady");
        private By LoanSelectorButtonLocator => _locators.GetBy("Dashboard.LoanSelector.Button");
        private By MakePaymentButtonLocator => _locators.GetBy("Dashboard.MakePayment.Button");

        public DashboardPage(IWebDriver driver) : base(driver) { }

        public bool IsPageReady()
        {
            return Wait.UntilVisible(PageReadyLocator) != null;
        }

        public void DismissAllPopups()
        {
            // Contact Update
            Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactUpdateLater"));
            // Scheduled Payment
            Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactContinue"));
            // Chatbot handled by framework
            Thread.Sleep(500); // Defensive wait for overlays
        }

        public void SelectLoanAccount(string loanNumber)
        {
            Wait.UntilClickable(LoanSelectorButtonLocator).Click();
            var loanCardLocator = _locators.GetBy("Dashboard.LoanCard.ByAccount", loanNumber);
            Wait.UntilClickable(loanCardLocator).Click();
        }

        public void ClickMakePayment()
        {
            Wait.UntilClickable(MakePaymentButtonLocator).Click();
        }
    }
}