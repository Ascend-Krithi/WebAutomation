using OpenQA.Selenium;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Pages;

namespace WebAutomation.Tests.Pages
{
    public class PaymentPage : BasePage
    {
        private readonly LocatorRepository _repo;

        public PaymentPage(IWebDriver driver) : base(driver)
        {
            _repo = new LocatorRepository("Locators.txt");
        }

        public void EnterPaymentDate(string paymentDate)
        {
            // Assume date picker logic is handled in DatePickerHelper if needed
            // For simplicity, just a placeholder for entering payment date
        }

        public bool IsLateFeeMessageDisplayed()
        {
            return Wait.UntilPresent(_repo.GetBy("Payment.LateFee.Message"));
        }
    }
}