using OpenQA.Selenium;
using WebAutomation.Core.Locators;
using System;
using System.Globalization;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class PaymentPage
    {
        private readonly IWebDriver _driver;
        private readonly LocatorRepository _locators;

        public PaymentPage(IWebDriver driver)
        {
            _driver = driver;
            _locators = new LocatorRepository("Locators.txt");
        }

        public void ContinueScheduledPaymentPopupIfPresent()
        {
            var continueButtons = _driver.FindElements(_locators.GetBy("Dashboard.ContactContinue"));
            if (continueButtons.Count > 0)
            {
                continueButtons[0].Click();
            }
        }

        public void OpenDatePicker()
        {
            _driver.FindElement(_locators.GetBy("Payment.DatePicker.Toggle")).Click();
            Thread.Sleep(500);
        }

        public void SelectPaymentDate(string date)
        {
            DateTime dt = DateTime.ParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            // Open date picker if not already open
            if (_driver.FindElements(_locators.GetBy("Payment.Calendar.Day", dt.Day.ToString())).Count == 0)
            {
                OpenDatePicker();
            }
            _driver.FindElement(_locators.GetBy("Payment.Calendar.Day", dt.Day.ToString())).Click();
            Thread.Sleep(500);
        }

        public bool IsPaymentDateDisplayed(string date)
        {
            // Assume the date field is an input with the selected value
            var input = _driver.FindElement(By.CssSelector("input[formcontrolname='paymentDate']"));
            return input.GetAttribute("value").Contains(date);
        }

        public bool IsLateFeeMessageDisplayed()
        {
            return _driver.FindElements(_locators.GetBy("Payment.LateFee.Message")).Count > 0;
        }
    }
}