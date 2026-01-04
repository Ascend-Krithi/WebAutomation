using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class DashboardPage : BasePage
    {
        private readonly LocatorRepository _repo = new LocatorRepository("Locators.json");

        public DashboardPage(IWebDriver driver) : base(driver) { }

        public void WaitForDashboard()
        {
            Wait.UntilVisible(_repo.GetBy("Dashboard.PageReady"));
        }

        public void DismissPopups()
        {
            // Contact Update
            Popup.HandleIfPresent(_repo.GetBy("Dashboard.ContactUpdateLater"));
            // Chatbot iframe is handled by framework
            // Scheduled Payment
            Popup.HandleIfPresent(_repo.GetBy("Dashboard.ContactContinue"));
        }

        public void SelectLoanAccount(string loanNumber)
        {
            Wait.UntilClickable(_repo.GetBy("Dashboard.LoanSelector.Button")).Click();
            Wait.UntilClickable(_repo.GetBy("Dashboard.LoanCard.ByAccount", loanNumber)).Click();
        }

        public void ClickMakePayment()
        {
            Wait.UntilClickable(_repo.GetBy("Dashboard.MakePayment.Button")).Click();
        }
    }
}