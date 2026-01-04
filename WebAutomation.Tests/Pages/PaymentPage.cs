using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Utilities;
using System.Globalization;

namespace WebAutomation.Tests.Pages
{
    public class PaymentPage : BasePage
    {
        public PaymentPage(IWebDriver driver) : base(driver) { }

        public void WaitForPaymentPage()
        {
            Wait.UntilVisible(By.XPath("//span[contains(text(),'Make a Payment')]"));
        }

        public void SelectPaymentDate(string paymentDate)
        {
            var dt = DateTime.ParseExact(paymentDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Wait.UntilClickable(By.CssSelector("mat-datepicker-toggle button")).Click();
            Wait.WaitForOverlay();
            Wait.UntilClickable(By.CssSelector("button.mat-calendar-period-button")).Click();
            Wait.UntilClickable(By.XPath($"//div[contains(@class,'mat-calendar-body-cell-content') and text()='{dt.Year}']")).Click();
            Wait.UntilClickable(By.XPath($"//div[contains(@class,'mat-calendar-body-cell-content') and text()='{dt:MMM}']")).Click();
            Wait.UntilClickable(By.XPath($"//div[contains(@class,'mat-calendar-body-cell-content') and text()='{dt.Day}']")).Click();
            Wait.WaitForOverlayToClose();
        }

        public bool IsLateFeeMessageDisplayed()
        {
            return Wait.UntilPresent(By.Id("latefeeInfoMsg1"), 5);
        }
    }
}