using OpenQA.Selenium;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Utilities;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class DashboardPage : BasePage
    {
        private readonly LocatorRepository _locators;

        public DashboardPage(IWebDriver driver) : base(driver)
        {
            _locators = new LocatorRepository("Locators.txt");
        }

        public bool IsPageReady()
        {
            return Wait.UntilPresent(_locators.GetBy("Dashboard.PageReady"));
        }

        public void DismissPopups()
        {
            // Contact Update
            if (Popup.IsPresent(_locators.GetBy("Dashboard.ContactPopup")))
            {
                Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactUpdateLater"));
            }
            // Chatbot iframe
            try
            {
                Driver.SwitchTo().Frame(_locators.GetBy("Chatbot.Iframe"));
                Driver.SwitchTo().DefaultContent();
            }
            catch { }
            // Angular overlays handled by SmartWait
        }

        public void SelectLoanAccount(string loanNumber)
        {
            Wait.UntilClickable(_locators.GetBy("Dashboard.LoanSelector.Button")).Click();
            Wait.UntilClickable(_locators.GetBy("Dashboard.LoanCard.ByAccount", loanNumber)).Click();
        }

        public bool IsLoanDetailsLoaded()
        {
            // Assume loan details loaded if Make Payment button is present
            return Wait.UntilPresent(_locators.GetBy("Dashboard.MakePayment.Button"));
        }

        public void ClickMakePayment()
        {
            Wait.UntilClickable(_locators.GetBy("Dashboard.MakePayment.Button")).Click();
        }
    }
}