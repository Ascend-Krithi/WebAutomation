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

        public void WaitForDashboard()
        {
            Wait.UntilVisible(_repo.GetBy("Dashboard.PageReady"));
        }

        public void DismissAllPopups()
        {
            // Contact Update
            if (Popup.IsPresent(_repo.GetBy("Dashboard.ContactPopup")))
            {
                Popup.HandleIfPresent(_repo.GetBy("Dashboard.ContactUpdateLater"));
            }
            // Chatbot iframe (handled by framework, but defensively here)
            try
            {
                Driver.SwitchTo().Frame(_repo.GetBy("Chatbot.Iframe"));
                Driver.SwitchTo().DefaultContent();
            }
            catch { }
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