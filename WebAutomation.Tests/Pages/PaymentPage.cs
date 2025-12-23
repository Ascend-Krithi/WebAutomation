using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Utilities;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class PaymentPage : BasePage
    {
        private readonly LocatorRepository _locators;

        public PaymentPage(IWebDriver driver) : base(driver)
        {
            _locators = new LocatorRepository("Locators.txt");
        }

        public bool IsScheduledPaymentPopupPresent()
        {
            return Popup.IsPresent(_locators.GetBy("Dashboard.ContactPopup"));
        }

        public void ContinueScheduledPaymentPopupIfPresent()
        {
            Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactContinue"));
        }

        public bool IsMakePaymentScreenDisplayed()
        {
            return Wait.UntilPresent(_locators.GetBy("Payment.PageReady"));
        }

        public void OpenPaymentDatePicker()
        {
            Wait.UntilClickable(_locators.GetBy("Payment.DatePicker.Toggle")).Click();
            Wait.WaitForOverlay();
        }

        public bool IsCalendarWidgetDisplayed()
        {
            return Wait.UntilPresent(_locators.GetBy("Payment.DatePicker.Toggle"));
        }

        public void SelectPaymentDate(string paymentDate)
        {
            var dt = System.DateTime.Parse(paymentDate);
            Wait.UntilClickable(_locators.GetBy("Payment.Calendar.Day", dt.Day.ToString())).Click();
            Wait.WaitForOverlayToClose();
        }

        public bool IsPaymentDateSelected(string paymentDate)
        {
            var dt = System.DateTime.Parse(paymentDate);
            var selectedDay = Wait.UntilVisible(_locators.GetBy("Payment.Calendar.Day", dt.Day.ToString()));
            return selectedDay != null;
        }

        public bool IsLateFeeMessageDisplayed()
        {
            return Wait.UntilPresent(_locators.GetBy("Payment.LateFee.Message"));
        }
    }
}