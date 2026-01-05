using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class DashboardPage : BasePage
    {
        private readonly LocatorRepository _repo;

        public DashboardPage(IWebDriver driver) : base(driver)
        {
            _repo = new LocatorRepository("Locators.txt");
        }

        public By PageReadyLocator() => _repo.GetBy("Dashboard.PageReady");

        public bool IsPageReady()
        {
            return Driver.FindElements(_repo.GetBy("Dashboard.PageReady")).Count > 0;
        }

        public void DismissPopups()
        {
            // Contact Update
            Popup.HandleIfPresent(_repo.GetBy("Dashboard.ContactUpdateLater"));
            // Chatbot iframe is handled by framework
            // Scheduled Payment popup handled in PaymentPage
        }

        public void SelectLoanAccount(string loanNumber)
        {
            Driver.FindElement(_repo.GetBy("Dashboard.LoanSelector.Button")).Click();
            Thread.Sleep(500); // Wait for modal
            Driver.FindElement(_repo.GetBy("Dashboard.LoanCard.ByAccount", loanNumber)).Click();
        }

        public void ClickMakePayment()
        {
            Driver.FindElement(_repo.GetBy("Dashboard.MakePayment.Button")).Click();
        }
    }
}