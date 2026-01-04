using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class DashboardPage : BasePage
    {
        public DashboardPage(IWebDriver driver) : base(driver) { }

        public void WaitForDashboard()
        {
            Wait.UntilVisible(By.CssSelector("header"));
        }

        public void ClosePopupsIfPresent()
        {
            // Contact Info Update Popup
            if (Popup.IsPresent(By.CssSelector("mat-dialog-container")))
            {
                Popup.HandleIfPresent(By.XPath("//button[normalize-space()='Update Later']"));
                Popup.HandleIfPresent(By.XPath("//button[normalize-space()='Continue']"));
            }
            // Chatbot iframe handled by framework
        }

        public void SelectLoanCard(string loanNumber)
        {
            Wait.UntilClickable(By.XPath($"//p[contains(normalize-space(.),'Account - {loanNumber}')]")).Click();
        }

        public void ClickMakePayment()
        {
            Wait.UntilClickable(By.CssSelector("p.make-payment")).Click();
            Thread.Sleep(1000); // Defensive wait for overlay
        }
    }
}