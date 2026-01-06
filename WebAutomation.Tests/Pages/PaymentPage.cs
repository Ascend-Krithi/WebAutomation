using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using System;
using System.Globalization;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class PaymentPage : BasePage
    {
        private readonly LocatorRepository _locators = new LocatorRepository("Locators.txt");

        public PaymentPage(IWebDriver driver) : base(driver) { }

        public void HandleScheduledPaymentPopup()
        {
            if (Popup.IsPresent(_locators.GetBy("Dashboard.ContactContinue")))
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
            var dt = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            // Open date picker if not already open
            Wait.UntilClickable(_locators.GetBy("Payment.DatePicker.Toggle")).Click();
            Wait.WaitForOverlay();
            // Select year
            Driver.FindElement(By.CssSelector("button.mat-calendar-period-button")).Click();
            Wait.UntilClickable(By.XPath($"//div[contains(@class,'mat-calendar-body-cell-content') and text()='{dt.Year}']")).Click();
            // Select month
            Wait.UntilClickable(By.XPath($"//div[contains(@class,'mat-calendar-body-cell-content') and text()='{dt.ToString("MMM", CultureInfo.InvariantCulture)}']")).Click();
            // Select day
            Wait.UntilClickable(_locators.GetBy("Payment.Calendar.Day", dt.Day.ToString())).Click();
            Wait.WaitForOverlayToClose();
        }

        public bool IsLateFeeMessageDisplayed()
        {
            try
            {
                return Driver.FindElement(_locators.GetBy("Payment.LateFee.Message")).Displayed;
            }
            catch
            {
                return false;
            }
        }
    }
}