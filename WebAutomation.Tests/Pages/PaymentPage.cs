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
            _repo = new LocatorRepository("Locators.txt");
        }

        public void ContinueScheduledPaymentIfPresent()
        {
            Popup.HandleIfPresent(_repo.GetBy("Dashboard.ContactContinue"));
        }

        public void OpenDatePicker()
        {
            Driver.FindElement(_repo.GetBy("Payment.DatePicker.Toggle")).Click();
            Thread.Sleep(500); // Wait for calendar overlay
        }

        public void SelectPaymentDate(string date)
        {
            var dt = System.DateTime.Parse(date);
            // Select year
            Driver.FindElement(By.CssSelector("button.mat-calendar-period-button")).Click();
            Thread.Sleep(200);
            Driver.FindElement(By.XPath($"//div[contains(@class,'mat-calendar-body-cell-content') and text()='{dt.Year}']")).Click();
            Thread.Sleep(200);
            // Select month
            Driver.FindElement(By.XPath($"//div[contains(@class,'mat-calendar-body-cell-content') and text()='{dt.ToString("MMM")}'"])).Click();
            Thread.Sleep(200);
            // Select day
            Driver.FindElement(_repo.GetBy("Payment.Calendar.Day", dt.Day.ToString())).Click();
            Thread.Sleep(500);
        }

        public bool IsLateFeeMessageDisplayed()
        {
            return Driver.FindElements(_repo.GetBy("Payment.LateFee.Message")).Count > 0;
        }
    }
}