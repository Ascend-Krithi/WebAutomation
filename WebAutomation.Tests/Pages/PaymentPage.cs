using OpenQA.Selenium;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Pages;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class PaymentPage : BasePage
    {
        private readonly LocatorRepository _repo = new LocatorRepository("Locators.txt");

        public PaymentPage(IWebDriver driver) : base(driver) { }

        public void ContinueScheduledPaymentPopupIfPresent()
        {
            Popup.HandleIfPresent(_repo.GetBy("Dashboard.ContactContinue"));
        }

        public void OpenDatePicker()
        {
            Driver.FindElement(_repo.GetBy("Payment.DatePicker.Toggle")).Click();
            Wait.WaitForOverlay();
        }

        public void SelectPaymentDate(string date)
        {
            var dt = DateTime.ParseExact(date, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            // Defensive: open picker if not already open
            if (!Driver.FindElement(_repo.GetBy("Payment.DatePicker.Toggle")).Displayed)
                Driver.FindElement(_repo.GetBy("Payment.DatePicker.Toggle")).Click();
            // Select day
            Driver.FindElement(_repo.GetBy("Payment.Calendar.Day", dt.Day.ToString())).Click();
            Wait.WaitForOverlayToClose();
        }

        public bool IsLateFeeMessageDisplayed()
        {
            return Driver.FindElements(_repo.GetBy("Payment.LateFee.Message")).Count > 0;
        }
    }
}