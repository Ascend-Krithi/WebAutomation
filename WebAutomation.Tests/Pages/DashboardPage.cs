using OpenQA.Selenium;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Pages;
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

        public bool IsPageReady()
        {
            return Wait.UntilPresent(_repo.GetBy("Dashboard.PageReady"));
        }

        public void DismissAllPopups()
        {
            // Contact Update Popup
            if (Popup.IsPresent(_repo.GetBy("Dashboard.ContactPopup")))
            {
                Popup.HandleIfPresent(_repo.GetBy("Dashboard.ContactUpdateLater"));
                Popup.HandleIfPresent(_repo.GetBy("Dashboard.ContactContinue"));
            }
            // Chatbot iframe handled by framework
        }

        public void SelectLoanAccount(string loanNumber)
        {
            Driver.FindElement(_repo.GetBy("Dashboard.LoanSelector.Button")).Click();
            Wait.UntilPresent(_repo.GetBy("Dashboard.LoanCard.ByAccount", loanNumber));
            Driver.FindElement(_repo.GetBy("Dashboard.LoanCard.ByAccount", loanNumber)).Click();
            Thread.Sleep(1000); // Wait for loan details to load
        }

        public void ClickMakePayment()
        {
            Wait.UntilClickable(_repo.GetBy("Dashboard.MakePayment.Button")).Click();
        }
    }
}