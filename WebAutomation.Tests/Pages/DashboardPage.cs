using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class DashboardPage : BasePage
    {
        private readonly LocatorRepository _locators = new LocatorRepository("Locators.txt");

        public DashboardPage(IWebDriver driver) : base(driver) { }

        public void WaitForDashboard()
        {
            Wait.UntilVisible(_locators.GetBy("Dashboard.PageReady"));
        }

        public void DismissPopups()
        {
            // Contact Update
            if (Popup.IsPresent(_locators.GetBy("Dashboard.ContactPopup")))
            {
                Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactUpdateLater"));
            }
            // Chatbot handled by framework
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
    }
}