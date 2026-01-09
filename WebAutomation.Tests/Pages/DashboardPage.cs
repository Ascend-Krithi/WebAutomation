using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class DashboardPage : BasePage
    {
        private readonly LocatorRepository _locators;

        public By PageReadyLocator => _locators.GetBy("Dashboard.PageReady");

        public DashboardPage(IWebDriver driver) : base(driver)
        {
            _locators = new LocatorRepository("Locators.json");
        }

        public bool IsPageReady()
        {
            return Wait.UntilVisible(PageReadyLocator) != null;
        }

        public void DismissAllPopups()
        {
            // Contact Update
            Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactUpdateLater"));
            Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactContinue"));
            // Chatbot iframe
            try
            {
                var iframe = Driver.FindElement(_locators.GetBy("Chatbot.Iframe"));
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].style.display='none';", iframe);
            }
            catch { }
            // Angular overlays
            Thread.Sleep(500);
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