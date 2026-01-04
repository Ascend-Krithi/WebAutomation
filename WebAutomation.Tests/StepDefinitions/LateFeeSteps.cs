using System;
using System.Collections.Generic;
using NUnit.Framework;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using WebAutomation.Core.Configuration;
using WebAutomation.Core.Utilities;
using WebAutomation.Tests.Pages;

namespace WebAutomation.Tests.StepDefinitions
{
    [Binding]
    public class LateFeeSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly string _excelPath;
        private readonly string _sheetName = "Sheet1";
        private Dictionary<string, string> _testData;
        private IWebDriver _driver;
        private SmartWait _wait;
        private PopupHandler _popup;

        public LateFeeSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _excelPath = System.IO.Path.Combine(AppContext.BaseDirectory, "LateFee", "LateFee.xlsx");
        }

        [Given(@"the user launches the customer servicing application")]
        public void GivenTheUserLaunchesTheCustomerServicingApplication()
        {
            _driver = _scenarioContext.Get<IWebDriver>("driver");
            _wait = _scenarioContext.Get<SmartWait>("wait");
            _popup = _scenarioContext.Get<PopupHandler>("popup");
            _driver.Navigate().GoToUrl(ConfigManager.Settings.BaseUrl);
            _wait.UntilVisible(By.CssSelector("form"));
        }

        [Given(@"logs in with valid credentials")]
        public void GivenLogsInWithValidCredentials()
        {
            var loginPage = new LoginPage(_driver);
            loginPage.LoginWithDefaultCredentials();
        }

        [Given(@"completes MFA verification")]
        public void GivenCompletesMFAVerification()
        {
            var mfaPage = new MfaPage(_driver);
            mfaPage.CompleteMfa();
        }

        [Given(@"the dashboard is loaded")]
        public void GivenTheDashboardIsLoaded()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.WaitForDashboard();
        }

        [Given(@"all pop-ups are dismissed")]
        public void GivenAllPopupsAreDismissed()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.DismissAllPopups();
        }

        [Given(@"the user selects the applicable loan account")]
        public void GivenTheUserSelectsTheApplicableLoanAccount(Table table)
        {
            var testCaseId = table.Rows[0]["TestCaseId"];
            _testData = ExcelReader.GetRow(_excelPath, _sheetName, "TestCaseId", testCaseId);
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.SelectLoanAccount(_testData["LoanNumber"]);
        }

        [When(@"the user clicks Make a Payment")]
        public void WhenTheUserClicksMakeAPayment()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.ClickMakePayment();
        }

        [When(@"handles the scheduled payment popup if present")]
        public void WhenHandlesTheScheduledPaymentPopupIfPresent()
        {
            var paymentPage = new PaymentPage(_driver);
            paymentPage.HandleScheduledPaymentPopup();
        }

        [When(@"opens the payment date picker")]
        public void WhenOpensThePaymentDatePicker()
        {
            var paymentPage = new PaymentPage(_driver);
            paymentPage.OpenDatePicker();
        }

        [When(@"selects the payment date \"(.*)\"")]
        public void WhenSelectsThePaymentDate(string paymentDate)
        {
            var paymentPage = new PaymentPage(_driver);
            paymentPage.SelectPaymentDate(paymentDate);
        }

        [Then(@"the late fee message area should display late fee message: (.*)")]
        public void ThenTheLateFeeMessageAreaShouldDisplayLateFeeMessage(bool expectedLateFee)
        {
            var paymentPage = new PaymentPage(_driver);
            bool isLateFeeDisplayed = paymentPage.IsLateFeeMessageDisplayed();
            if (expectedLateFee)
                Assert.True(isLateFeeDisplayed, "Expected late fee message to be displayed, but it was not.");
            else
                Assert.False(isLateFeeDisplayed, "Expected no late fee message, but it was displayed.");
        }
    }
}