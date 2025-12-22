using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Locators;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class PaymentPage : BasePage
    {
        private readonly LocatorRepository _repo;

        public PaymentPage(IWebDriver driver) : base(driver)
        {
            _repo = new LocatorRepository("Locators/Locators.txt");
        }

        public void ContinueScheduledPaymentIfPresent()
        {
            // Scheduled payment popup
            if (Popup.IsPresent(_repo.GetBy("Dashboard.ContactPopup")))
            {
                if (Popup.IsPresent(_repo.GetBy("Dashboard.ContactContinue")))
                {
                    Popup.HandleIfPresent(_repo.GetBy("Dashboard.ContactContinue"));
                }
            }
        }

        public void OpenDatePicker()
        {
            Wait.UntilClickable(_repo.GetBy("Payment.DatePicker.Toggle")).Click();
        }

        public void SelectPaymentDate(string paymentDate)
        {
            var date = System.DateTime.Parse(paymentDate);
            string day = date.Day.ToString();
            Wait.UntilClickable(_repo.GetBy("Payment.Calendar.Day", day)).Click();
        }

        public bool IsLateFeeMessageDisplayed()
        {
            try
            {
                return Driver.FindElement(_repo.GetBy("Payment.LateFee.Message")).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}