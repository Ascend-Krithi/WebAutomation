using OpenQA.Selenium;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Pages;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class DashboardPage : BasePage
    {
        private readonly LocatorRepository _repo = new LocatorRepository("Locators.txt");

        public DashboardPage(IWebDriver driver) : base(driver) { }

        public bool IsPageReady()
        {
            return Wait.UntilPresent(_repo.GetBy("Dashboard.PageReady"));
        }

        public void DismissPopups()
        {
            // Contact Update
            Popup.HandleIfPresent(_repo.GetBy("Dashboard.ContactUpdateLater"));
            // Chatbot iframe is handled by framework
            // Scheduled Payment handled in PaymentPage
        }

        public void SelectLoanAccount(string loanNumber)
        {
            Driver.FindElement(_repo.GetBy("Dashboard.LoanSelector.Button")).Click();
            Thread.Sleep(500); // Defensive wait for overlay
            Driver.FindElement(_repo.GetBy("Dashboard.LoanCard.ByAccount", loanNumber)).Click();
        }

        public void ClickMakePayment()
        {
            Driver.FindElement(_repo.GetBy("Dashboard.MakePayment.Button")).Click();
        }
    }
}