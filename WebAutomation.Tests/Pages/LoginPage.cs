using OpenQA.Selenium;
using WebAutomation.Core.Pages;

namespace WebAutomation.Tests.Pages
{
    public class LoginPage : BasePage
    {
        public LoginPage(IWebDriver driver) : base(driver) { }

        public void Login(string username, string password)
        {
            Driver.FindElement(By.Id("username")).SendKeys(username);
            Driver.FindElement(By.Id("password")).SendKeys(password);
            Driver.FindElement(By.XPath("//button[contains(text(),'Sign in')]")).Click();
        }
    }
}