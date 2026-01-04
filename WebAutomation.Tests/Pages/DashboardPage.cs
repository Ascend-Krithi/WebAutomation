using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class DashboardPage : BasePage
    {
        public DashboardPage(IWebDriver driver) : base(driver) { }

        public bool IsLoaded()
        {
            return Driver.FindElements(By.XPath("//h1[contains(text(),'Dashboard')]")).Count > 0;
        }

        public void ClickMakeAPayment()
        {
            Driver.FindElement(By.XPath("//span[contains(text(),'Make a Payment')]")).Click();
            Thread.Sleep(1000);
        }

        public void ClickSetupAutopay()
        {
            Driver.FindElement(By.XPath("//span[contains(text(),'Setup Autopay')]")).Click();
            Thread.Sleep(1000);
        }
    }
}