using OpenQA.Selenium;
using WebAutomation.Core.Locators;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class DashboardPage
    {
        private readonly IWebDriver _driver;
        private readonly LocatorRepository _locators;

        public DashboardPage(IWebDriver driver)
        {
            _driver = driver;
            _locators = new LocatorRepository("Locators.txt");
        }

        public bool IsPageReady()
        {
            return _driver.FindElements(_locators.GetBy("Dashboard.PageReady")).Count > 0;
        }

        public void DismissAllPopups()
        {
            // Contact Update
            if (_driver.FindElements(_locators.GetBy("Dashboard.ContactPopup")).Count > 0)
            {
                if (_driver.FindElements(_locators.GetBy("Dashboard.ContactUpdateLater")).Count > 0)
                {
                    _driver.FindElement(_locators.GetBy("Dashboard.ContactUpdateLater")).Click();
                }
                else if (_driver.FindElements(_locators.GetBy("Dashboard.ContactContinue")).Count > 0)
                {
                    _driver.FindElement(_locators.GetBy("Dashboard.ContactContinue")).Click();
                }
            }
            // Chatbot iframe
            if (_driver.FindElements(_locators.GetBy("Chatbot.Iframe")).Count > 0)
            {
                try
                {
                    ((IJavaScriptExecutor)_driver).ExecuteScript("document.getElementById('servisbot-messenger-iframe-roundel').style.display='none';");
                }
                catch { }
            }
            // Angular overlays
            Thread.Sleep(500);
        }

        public void SelectLoanByAccount(string loanNumber)
        {
            _driver.FindElement(_locators.GetBy("Dashboard.LoanSelector.Button")).Click();
            _driver.FindElement(_locators.GetBy("Dashboard.LoanCard.ByAccount", loanNumber)).Click();
        }

        public bool AreLoanDetailsVisible()
        {
            // Assume loan details are visible if Make Payment button is present
            return _driver.FindElements(_locators.GetBy("Dashboard.MakePayment.Button")).Count > 0;
        }

        public void ClickMakePayment()
        {
            _driver.FindElement(_locators.GetBy("Dashboard.MakePayment.Button")).Click();
        }
    }
}