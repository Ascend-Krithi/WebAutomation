using System;
using System.Collections.Generic;
using NUnit.Framework;
using TechTalk.SpecFlow;
using WebAutomation.Core.Configuration;
using WebAutomation.Core.Utilities;
using WebAutomation.Tests.Pages;
using OpenQA.Selenium;

namespace WebAutomation.Tests.StepDefinitions
{
    [Binding]
    public class LateFeeSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private Dictionary<string, string> _testData;
        private IWebDriver _driver;
        private SmartWait _wait;
        private PopupHandler _popup;

        public LateFeeSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"the user launches the customer servicing application")]
        public void GivenTheUserLaunchesTheCustomerServicingApplication()
        {
            _driver = _scenarioContext.Get<IWebDriver>("driver");
            _wait = _scenarioContext.Get<SmartWait>("wait");
            _popup = _scenarioContext.Get<PopupHandler>("popup");
            _driver.Navigate().GoToUrl(ConfigManager.Settings.BaseUrl);
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

        [Given(@"navigates to the dashboard")]
        public void GivenNavigatesToTheDashboard()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.WaitForDashboard();
        }

        [Given(@"dismisses any pop-ups if present")]
        public void GivenDismissesAnyPopupsIfPresent()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.DismissPopups();
        }

        [Given(@"selects the applicable loan account")]
        public void GivenSelectsTheApplicableLoanAccount()
        {
            var dashboardPage = new DashboardPage(_driver);
            string loanNumber = _testData["LoanNumber"];
            dashboardPage.SelectLoanAccount(loanNumber);
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

        [When(@"selects the payment date from test data")]
        public void WhenSelectsThePaymentDateFromTestData()
        {
            var paymentPage = new PaymentPage(_driver);
            string paymentDate = _testData["PaymentDate"];
            paymentPage.SelectPaymentDate(paymentDate);
        }

        [Then(@"the late fee message area should (.*)")]
        public void ThenTheLateFeeMessageAreaShouldBe(string expectation)
        {
            var paymentPage = new PaymentPage(_driver);
            bool isLateFeeDisplayed = paymentPage.IsLateFeeMessageDisplayed();
            bool expected = bool.Parse(expectation);
            if (expected)
                Assert.True(isLateFeeDisplayed, "Late fee message should be displayed.");
            else
                Assert.False(isLateFeeDisplayed, "Late fee message should not be displayed.");
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            var testCaseId = _scenarioContext.ScenarioInfo.Arguments["TestCaseId"].ToString();
            string excelPath = System.IO.Path.Combine(AppContext.BaseDirectory, "LateFee", "LateFee.xlsx");
            _testData = ExcelReader.GetRow(excelPath, "Sheet1", "TestCaseId", testCaseId);
        }
    }
}