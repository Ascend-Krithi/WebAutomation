using OpenQA.Selenium;
using WebAutomation.Core.Pages;

namespace WebAutomation.Tests.Pages
{
    public class PaymentPage : BasePage
    {
        public PaymentPage(IWebDriver driver) : base(driver) { }

        public bool IsLoaded()
        {
            return Driver.FindElements(By.XPath("//h1[contains(text(),'Make a Payment')]")).Count > 0;
        }
    }
}