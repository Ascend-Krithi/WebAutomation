using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Utilities;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class PaymentPage : BasePage
    {
        public PaymentPage(IWebDriver driver) : base(driver) { }

        public void ContinueScheduledPaymentIfPresent()
        {
            Popup.HandleIfPresent(By.XPath("//button[normalize-space()='Continue']"));
        }

        public void OpenPaymentDatePicker()
        {
            Wait.UntilClickable(By.CssSelector("mat-datepicker-toggle button")).Click();
        }

        public void SelectPaymentDate(string date)
        {
            var datePicker = new DatePickerHelper(Driver);
            datePicker.SelectDate(date);
        }

        public bool IsLateFeeMessageDisplayed()
        {
            return Driver.FindElements(By.XPath("//div[contains(@class,'late-fee-message')]")).Count > 0;
        }
    }
}