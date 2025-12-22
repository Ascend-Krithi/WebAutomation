using OpenQA.Selenium;
using WebAutomation.Core.Utilities;
using System;
using System.Globalization;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class PaymentPage
    {
        private readonly IWebDriver _driver;
        private readonly SmartWait _wait;

        public PaymentPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new SmartWait(driver);
        }

        public bool IsPageReady()
        {
            // Use a unique element on the Make a Payment page
            return _wait.UntilPresent(By.CssSelector("span"), 10);
        }

        public void OpenDatePicker()
        {
            _wait.UntilClickable(By.CssSelector("mat-datepicker-toggle button")).Click();
            _wait.WaitForOverlay();
        }

        public void SelectPaymentDate(string date)
        {
            var dt = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            // Assumes calendar is already open
            _wait.UntilClickable(By.XPath($"//div[contains(@class,'mat-calendar-body-cell-content') and normalize-space(text())='{dt.Day}']")).Click();
            _wait.WaitForOverlayToClose();
        }

        public bool IsLateFeeMessageDisplayed()
        {
            return _wait.UntilPresent(By.Id("latefeeInfoMsg1"), 2);
        }
    }
}