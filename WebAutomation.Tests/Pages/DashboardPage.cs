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
            _repo = new LocatorRepository("Locators.json");
        }

        public void WaitForPageReady()
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
            Driver.FindElement(_repo.GetBy("Dashboard.LoanSelector.Button")).Click();
            Wait.UntilVisible(_repo.GetBy("Dashboard.LoanCard.ByAccount", loanNumber));
            Driver.FindElement(_repo.GetBy("Dashboard.LoanCard.ByAccount", loanNumber)).Click();
        }

        public void ClickMakePayment()
        {
            Wait.UntilClickable(_repo.GetBy("Dashboard.MakePayment.Button")).Click();
        }
    }
}