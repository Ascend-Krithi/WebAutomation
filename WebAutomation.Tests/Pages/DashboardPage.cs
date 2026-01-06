using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class DashboardPage : BasePage
    {
        public DashboardPage(IWebDriver driver) : base(driver) { }

        public void WaitForDashboardToLoad()
        {
            Wait.UntilVisible(By.XPath("//h1[contains(text(),'Dashboard')]"));
        }

        public void DismissPopupsIfPresent()
        {
            // Contact Update popup
            Popup.HandleIfPresent(By.XPath("//button[normalize-space()='Update Later']"));
            // Chatbot iframe handled by framework
            // Scheduled Payment popup
            Popup.HandleIfPresent(By.XPath("//button[normalize-space()='Continue']"));
            // Angular overlays handled by framework
        }

        public void SelectLoanAccount(string loanNumber)
        {
            Wait.UntilClickable(By.XPath($"//div[contains(@class,'loan-account') and contains(text(),'{loanNumber}')]")).Click();
        }

        public void ClickMakeAPayment()
        {
            Wait.UntilClickable(By.XPath("//span[contains(text(),'Make a Payment')]")).Click();
        }
    }
}