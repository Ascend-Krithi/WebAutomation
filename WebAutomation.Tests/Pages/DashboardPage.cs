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

        public void WaitForPageReady()
        {
            Wait.UntilVisible(_repo.GetBy("Dashboard.PageReady"));
        }

        public void DismissPopupsIfPresent()
        {
            // Contact Update Popup
            if (Popup.IsPresent(_repo.GetBy("Dashboard.ContactPopup"), 3))
            {
                Popup.HandleIfPresent(_repo.GetBy("Dashboard.ContactUpdateLater"), 3);
                Popup.HandleIfPresent(_repo.GetBy("Dashboard.ContactContinue"), 3);
            }
            // Chatbot iframe handled by framework
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