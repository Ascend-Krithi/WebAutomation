using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Configuration;

namespace WebAutomation.Tests.Pages
{
    public class MfaPage : BasePage
    {
        public MfaPage(IWebDriver driver) : base(driver) { }

        public void CompleteMfaVerification()
        {
            Wait.UntilClickable(By.XPath("//select")).Click();
            Driver.FindElement(By.XPath("//select/option[1]")).Click();
            Driver.FindElement(By.XPath("//button[contains(text(),'Receive code via email')]")).Click();
            Wait.UntilVisible(By.Id("otp"));
            Driver.FindElement(By.Id("otp")).SendKeys(ConfigManager.Settings.StaticOtp);
            Driver.FindElement(By.XPath("//button[contains(text(),'Verify')]")).Click();
        }
    }
}