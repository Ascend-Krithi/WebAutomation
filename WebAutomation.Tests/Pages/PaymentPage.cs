using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using System;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class PaymentPage : BasePage
    {
        private readonly LocatorRepository _repo;

        public PaymentPage(IWebDriver driver) : base(driver)
        {
            _repo = new LocatorRepository("Locators.json");
        }

        public void ContinueScheduledPaymentIfPresent()
        {
            Popup.HandleIfPresent(_repo.GetBy("Dashboard.ContactContinue"));
        }

        public void OpenDatePicker()
        {
            Wait.UntilClickable(_repo.GetBy("Payment.DatePicker.Toggle")).Click();
        }

        public void SelectPaymentDate(string date)
        {
            DateTime dt = DateTime.Parse(date);
            // Open date picker if not already open
            OpenDatePicker();
            // Select day
            Wait.UntilClickable(_repo.GetBy("Payment.Calendar.Day", dt.Day.ToString())).Click();
        }

        public bool IsLateFeeMessageDisplayed()
        {
            try
            {
                return Driver.FindElement(_repo.GetBy("Payment.LateFee.Message")).Displayed;
            }
            catch
            {
                return false;
            }
        }
    }
}