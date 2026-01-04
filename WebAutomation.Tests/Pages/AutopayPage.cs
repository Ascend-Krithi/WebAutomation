using OpenQA.Selenium;
using WebAutomation.Core.Pages;

namespace WebAutomation.Tests.Pages
{
    public class AutopayPage : BasePage
    {
        public AutopayPage(IWebDriver driver) : base(driver) { }

        public bool IsLoaded()
        {
            return Driver.FindElements(By.XPath("//h1[contains(text(),'Setup Autopay')]")).Count > 0;
        }
    }
}