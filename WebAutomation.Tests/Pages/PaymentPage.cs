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

        public void HandleScheduledPaymentPopup()
        {
            // Defensive: handle scheduled payment popup if present
            Popup.HandleIfPresent(_repo.GetBy("Dashboard.ContactContinue"));
        }

        public void OpenDatePicker()
        {
            Wait.UntilClickable(_repo.GetBy("Payment.DatePicker.Toggle")).Click();
        }

        public void SelectPaymentDate(string date)
        {
            // Use DatePickerHelper if available in Core, otherwise select by day
            var dt = System.DateTime.Parse(date);
            Wait.UntilClickable(_repo.GetBy("Payment.Calendar.Day", dt.Day.ToString())).Click();
        }

        public bool IsLateFeeMessageDisplayed()
        {
            try
            {
                return Wait.UntilVisible(_repo.GetBy("Payment.LateFee.Message"), 5).Displayed;
            }
            catch
            {
                return false;
            }
        }
    }
}