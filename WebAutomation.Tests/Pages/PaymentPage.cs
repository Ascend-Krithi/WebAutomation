using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class PaymentPage : BasePage
    {
        private readonly LocatorRepository _locators = new LocatorRepository("Locators.txt");

        private By ScheduledPaymentContinueLocator => _locators.GetBy("Dashboard.ContactContinue");
        private By DatePickerToggleLocator => _locators.GetBy("Payment.DatePicker.Toggle");
        private By LateFeeMessageLocator => _locators.GetBy("Payment.LateFee.Message");

        public PaymentPage(IWebDriver driver) : base(driver) { }

        public void ContinueScheduledPaymentIfPresent()
        {
            Popup.HandleIfPresent(ScheduledPaymentContinueLocator);
        }

        public void OpenDatePicker()
        {
            Wait.UntilClickable(DatePickerToggleLocator).Click();
            Thread.Sleep(500); // Defensive wait for overlay
        }

        public void SelectPaymentDate(string date)
        {
            var dt = DateTime.ParseExact(date, "MM/dd/yyyy", null);
            var dayLocator = _locators.GetBy("Payment.Calendar.Day", dt.Day.ToString());
            Wait.UntilClickable(dayLocator).Click();
        }

        public bool IsLateFeeMessageDisplayed()
        {
            try
            {
                return Wait.UntilVisible(LateFeeMessageLocator, 3).Displayed;
            }
            catch
            {
                return false;
            }
        }
    }
}