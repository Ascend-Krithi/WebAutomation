using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class PopupPage : BasePage
    {
        public PopupPage(IWebDriver driver) : base(driver) { }

        public bool IsPopupDisplayed()
        {
            return Driver.FindElements(By.XPath("//div[contains(@class,'popup')]")).Count > 0;
        }

        public bool IsContinueButtonDisplayed()
        {
            return Driver.FindElements(By.XPath("//button[contains(text(),'Continue')]")).Count > 0;
        }

        public bool IsCancelButtonDisplayed()
        {
            return Driver.FindElements(By.XPath("//button[contains(text(),'Cancel')]")).Count > 0;
        }

        public void ClickContinue()
        {
            Driver.FindElement(By.XPath("//button[contains(text(),'Continue')]")).Click();
            Thread.Sleep(1000);
        }

        public void ClickCancel()
        {
            Driver.FindElement(By.XPath("//button[contains(text(),'Cancel')]")).Click();
            Thread.Sleep(1000);
        }
    }
}