using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using System;
using System.Globalization;

namespace WebAutomation.Tests.Pages
{
    public class PaymentPage : BasePage
    {
        private readonly LocatorRepository _repo = new LocatorRepository("Locators.json");

        public PaymentPage(IWebDriver driver) : base(driver) { }

        public void ContinueScheduledPaymentPopupIfPresent()
        {
            Popup.HandleIfPresent(_repo.GetBy("Dashboard.ContactContinue"));
        }

        public void OpenDatePicker()
        {
            Wait.UntilClickable(_repo.GetBy("Payment.DatePicker.Toggle")).Click();
        }

        public void SelectPaymentDate(string date)
        {
            DateTime dt = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            // Open date picker if not already open
            Wait.UntilClickable(_repo.GetBy("Payment.DatePicker.Toggle")).Click();
            Wait.WaitForOverlay();
            // Select year
            Wait.UntilClickable(By.CssSelector("button.mat-calendar-period-button")).Click();
            Wait.UntilClickable(By.XPath($"//div[contains(@class,'mat-calendar-body-cell-content') and text()='{dt.Year}']")).Click();
            // Select month
            Wait.UntilClickable(By.XPath($"//div[contains(@class,'mat-calendar-body-cell-content') and text()='{dt.ToString("MMM", CultureInfo.InvariantCulture)}']")).Click();
            // Select day
            Wait.UntilClickable(_repo.GetBy("Payment.Calendar.Day", dt.Day.ToString())).Click();
            Wait.WaitForOverlayToClose();
        }

        public bool IsLateFeeMessageDisplayed()
        {
            try
            {
                return Wait.UntilVisible(_repo.GetBy("Payment.LateFee.Message"), 3).Displayed;
            }
            catch
            {
                return false;
            }
        }
    }
}