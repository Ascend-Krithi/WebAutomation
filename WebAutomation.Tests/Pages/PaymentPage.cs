using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class PaymentPage : BasePage
    {
        private readonly LocatorRepository _repo;

        public PaymentPage(IWebDriver driver) : base(driver)
        {
            _repo = new LocatorRepository("Locators.txt");
        }

        public void ContinueScheduledPaymentPopupIfPresent()
        {
            Popup.HandleIfPresent(_repo.GetBy("Dashboard.ContactContinue"), 3);
        }

        public void OpenDatePicker()
        {
            Wait.UntilClickable(_repo.GetBy("Payment.DatePicker.Toggle")).Click();
        }

        public void SelectPaymentDate(string paymentDate)
        {
            // Use DatePickerHelper if available in Core, otherwise select day directly
            var dt = System.DateTime.ParseExact(paymentDate, "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            // Year and month selection omitted for brevity, as per reference pattern
            Wait.UntilClickable(_repo.GetBy("Payment.Calendar.Day", dt.Day.ToString())).Click();
        }

        public bool IsLateFeeMessageDisplayed()
        {
            try
            {
                return Driver.FindElement(_repo.GetBy("Payment.LateFee.Message")).Displayed;
            }
            catch
            {
                return false;
            }
        }
    }
}