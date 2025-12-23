using OpenQA.Selenium;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Utilities;
using System;
using System.Globalization;

namespace WebAutomation.Tests.Pages
{
    public class PaymentPage : BasePage
    {
        private readonly LocatorRepository _locators;

        public PaymentPage(IWebDriver driver) : base(driver)
        {
            _locators = new LocatorRepository("Locators.txt");
        }

        public void ContinueScheduledPaymentPopupIfPresent()
        {
            if (Popup.IsPresent(_locators.GetBy("Dashboard.ContactPopup")))
            {
                Popup.HandleIfPresent(_locators.GetBy("Dashboard.ContactContinue"));
            }
        }

        public void OpenDatePicker()
        {
            Wait.UntilClickable(_locators.GetBy("Payment.DatePicker.Toggle")).Click();
        }

        public void SelectPaymentDate(string date)
        {
            var dt = DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            // Year selection
            Wait.UntilClickable(By.CssSelector("button.mat-calendar-period-button")).Click();
            Wait.UntilClickable(By.XPath($"//div[contains(@class,'mat-calendar-body-cell-content') and text()='{dt.Year}']")).Click();
            // Month selection
            Wait.UntilClickable(By.XPath($"//div[contains(@class,'mat-calendar-body-cell-content') and text()='{dt.ToString("MMM", CultureInfo.InvariantCulture)}']")).Click();
            // Day selection
            Wait.UntilClickable(_locators.GetBy("Payment.Calendar.Day", dt.Day.ToString())).Click();
        }

        public bool IsLateFeeMessageDisplayed()
        {
            return Wait.UntilPresent(_locators.GetBy("Payment.LateFee.Message"), 3);
        }
    }
}