using OpenQA.Selenium;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Utilities;
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
            Popup.HandleIfPresent(_repo.GetBy("Dashboard.ContactContinue"));
        }

        public void OpenDatePicker()
        {
            Wait.UntilClickable(_repo.GetBy("Payment.DatePicker.Toggle")).Click();
            Wait.WaitForOverlay();
        }

        public void SelectPaymentDate(string date)
        {
            var helper = new DatePickerHelper(Driver);
            helper.SelectDate(date);
            Thread.Sleep(500); // Allow UI to update
        }

        public bool IsLateFeeMessageDisplayed()
        {
            return Wait.UntilPresent(_repo.GetBy("Payment.LateFee.Message"), 3);
        }
    }
}