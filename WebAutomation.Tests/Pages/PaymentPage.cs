using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Utilities;

namespace WebAutomation.Tests.Pages
{
    public class PaymentPage : BasePage
    {
        private readonly LocatorRepository _locators = new LocatorRepository("Locators.txt");

        public PaymentPage(IWebDriver driver) : base(driver) { }

        public void WaitForPageReady()
        {
            Wait.UntilVisible(_locators.GetBy("Payment.PageReady"));
        }

        public void OpenDatePicker()
        {
            Wait.UntilClickable(_locators.GetBy("Payment.DatePicker.Toggle")).Click();
        }

        public void SelectPaymentDate(string paymentDate)
        {
            var datePicker = new DatePickerHelper(Driver);
            datePicker.SelectDate(paymentDate);
        }

        public bool IsLateFeeMessageDisplayed()
        {
            return Wait.UntilPresent(_locators.GetBy("Payment.LateFee.Message"), 5);
        }
    }
}