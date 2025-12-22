using OpenQA.Selenium;
using WebAutomation.Core.Utilities;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class DashboardPage
    {
        private readonly IWebDriver _driver;
        private readonly SmartWait _wait;

        public DashboardPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new SmartWait(driver);
        }

        public bool IsPageReady()
        {
            return _wait.UntilPresent(By.CssSelector("header"), 15);
        }

        public void DismissPopupsIfPresent()
        {
            // Contact Update popup
            if (_wait.UntilPresent(By.CssSelector("mat-dialog-container"), 2))
            {
                if (_wait.UntilPresent(By.XPath("//button[normalize-space()='Update Later']"), 1))
                {
                    _wait.UntilClickable(By.XPath("//button[normalize-space()='Update Later']")).Click();
                }
                else if (_wait.UntilPresent(By.XPath("//button[normalize-space()='Continue']"), 1))
                {
                    _wait.UntilClickable(By.XPath("//button[normalize-space()='Continue']")).Click();
                }
            }
            // Chatbot iframe
            try
            {
                var iframes = _driver.FindElements(By.Id("servisbot-messenger-iframe-roundel"));
                if (iframes.Count > 0)
                {
                    ((IJavaScriptExecutor)_driver).ExecuteScript(
                        "arguments[0].style.display='none'; arguments[0].style.visibility='hidden';", iframes[0]);
                }
            }
            catch { }
        }

        public void SelectLoanAccount(string loanNumber)
        {
            _wait.UntilClickable(By.CssSelector("button[logaction='Open Loan Selection Window']")).Click();
            _wait.UntilClickable(By.XPath($"//p[contains(normalize-space(.),'Account - {loanNumber}')]")).Click();
        }

        public bool IsLoanDetailsLoaded(string loanNumber)
        {
            // Assume loan details section contains the loan number
            return _wait.UntilPresent(By.XPath($"//p[contains(normalize-space(.),'Account - {loanNumber}')]"), 10);
        }

        public void ClickMakePayment()
        {
            _wait.UntilClickable(By.CssSelector("p.make-payment")).Click();
        }

        public void ContinueScheduledPaymentIfPresent()
        {
            if (_wait.UntilPresent(By.CssSelector("mat-dialog-container"), 2))
            {
                if (_wait.UntilPresent(By.XPath("//button[normalize-space()='Continue']"), 1))
                {
                    _wait.UntilClickable(By.XPath("//button[normalize-space()='Continue']")).Click();
                }
            }
        }
    }
}