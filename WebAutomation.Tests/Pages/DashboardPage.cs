using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Utilities;
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

        public bool IsPageReady()
        {
            return Wait.UntilPresent(_repo.GetBy("Dashboard.PageReady"));
        }

        public void ClickMakePayment()
        {
            Wait.UntilClickable(_repo.GetBy("Dashboard.MakePayment.Button")).Click();
            Thread.Sleep(500);
        }

        public void ClickSetupAutopay()
        {
            // Assuming Setup Autopay button locator is similar, otherwise add correct key
            Wait.UntilClickable(By.XPath("//span[contains(text(),'Setup Autopay')]")).Click();
            Thread.Sleep(500);
        }

        public bool IsPaymentPopupPresent()
        {
            return Popup.IsPresent(By.XPath("//button[normalize-space()='Continue']")) &&
                   Popup.IsPresent(By.XPath("//button[normalize-space()='Cancel']"));
        }

        public void ClickPopupContinue()
        {
            Popup.HandleIfPresent(By.XPath("//button[normalize-space()='Continue']"));
            Thread.Sleep(500);
        }

        public void ClickPopupCancel()
        {
            Popup.HandleIfPresent(By.XPath("//button[normalize-space()='Cancel']"));
            Thread.Sleep(500);
        }

        public bool IsOnMakePaymentPage()
        {
            // Use Payment.PageReady locator
            return Wait.UntilPresent(By.XPath("//span[contains(text(),'Make a Payment')]"));
        }

        public bool IsOnSetupAutopayPage()
        {
            // Use Setup Autopay page ready locator if available, otherwise fallback
            return Wait.UntilPresent(By.XPath("//span[contains(text(),'Setup Autopay')]"));
        }
    }
}