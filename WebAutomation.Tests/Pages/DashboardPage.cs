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
            _repo = new LocatorRepository("Locators/Locators.txt");
        }

        public void WaitForDashboardReady()
        {
            Wait.UntilVisible(_repo.GetBy("Dashboard.PageReady"));
        }

        public void DismissPopupsIfPresent()
        {
            // Contact Update Popup
            if (Popup.IsPresent(_repo.GetBy("Dashboard.ContactPopup")))
            {
                if (Popup.IsPresent(_repo.GetBy("Dashboard.ContactUpdateLater")))
                {
                    Popup.HandleIfPresent(_repo.GetBy("Dashboard.ContactUpdateLater"));
                }
                else if (Popup.IsPresent(_repo.GetBy("Dashboard.ContactContinue")))
                {
                    Popup.HandleIfPresent(_repo.GetBy("Dashboard.ContactContinue"));
                }
            }
            // Chatbot iframe
            try
            {
                var iframe = Driver.FindElements(_repo.GetBy("Chatbot.Iframe"));
                if (iframe.Count > 0)
                {
                    ((IJavaScriptExecutor)Driver).ExecuteScript(
                        "arguments[0].style.display='none'; arguments[0].style.visibility='hidden';", iframe[0]);
                }
            }
            catch { }
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