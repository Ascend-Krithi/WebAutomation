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
        private IWebDriver _driver;
        private SmartWait _wait;
        private PopupHandler _popup;
        private Dictionary<string, string> _testData;
        private string _testDataPath;
        private string _sheetName = "Sheet1";

        public LateFeeSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _driver = _scenarioContext.Get<IWebDriver>("driver");
            _wait = _scenarioContext.Get<SmartWait>("wait");
            _popup = _scenarioContext.Get<PopupHandler>("popup");
            _testDataPath = System.IO.Path.Combine(AppContext.BaseDirectory, "TestData", "LateFee.xlsx");
        }

        [Given(@"the user launches the customer servicing application")]
        public void GivenTheUserLaunchesTheCustomerServicingApplication()
        {
            _driver.Navigate().GoToUrl(ConfigManager.Settings.BaseUrl);
            var loginPage = new LoginPage(_driver);
            loginPage.WaitForPageReady();
        }

        [Given(@"the user logs in with valid credentials")]
        public void GivenTheUserLogsInWithValidCredentials()
        {
            var loginPage = new LoginPage(_driver);
            loginPage.LoginWithDefaultCredentials();
        }

        [Given(@"the user completes MFA verification")]
        public void GivenTheUserCompletesMFAVerification()
        {
            var mfaPage = new MfaPage(_driver);
            mfaPage.CompleteMfa();
        }

        [Given(@"the user is on the dashboard")]
        public void GivenTheUserIsOnTheDashboard()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.WaitForPageReady();
        }

        [Given(@"all pop-ups are dismissed if present")]
        public void GivenAllPopupsAreDismissedIfPresent()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.DismissPopups();
        }

        [Given(@"the user selects the applicable loan account")]
        public void GivenTheUserSelectsTheApplicableLoanAccount()
        {
            var testCaseId = GetTestCaseId();
            _testData = ExcelReader.GetRow(_testDataPath, _sheetName, "TestCaseId", testCaseId);
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.SelectLoanAccount(_testData["LoanNumber"]);
        }

        [When(@"the user clicks Make a Payment")]
        public void WhenTheUserClicksMakeAPayment()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.ClickMakePayment();
        }

        [When(@"the user continues past the scheduled payment popup if it appears")]
        public void WhenTheUserContinuesPastTheScheduledPaymentPopupIfItAppears()
        {
            var paymentPage = new PaymentPage(_driver);
            paymentPage.ContinueScheduledPaymentIfPresent();
        }

        [When(@"the user opens the payment date picker")]
        public void WhenTheUserOpensThePaymentDatePicker()
        {
            var paymentPage = new PaymentPage(_driver);
            paymentPage.OpenDatePicker();
        }

        [When(@"the user selects the payment date from test data")]
        public void WhenTheUserSelectsThePaymentDateFromTestData()
        {
            var paymentPage = new PaymentPage(_driver);
            paymentPage.SelectPaymentDate(_testData["PaymentDate"]);
        }

        [Then(@"the late fee message area should (.*)")]
        public void ThenTheLateFeeMessageAreaShould(string expectation)
        {
            var paymentPage = new PaymentPage(_driver);
            bool isLateFeeDisplayed = paymentPage.IsLateFeeMessageDisplayed();
            var testCaseId = GetTestCaseId();
            var expectedLateFee = _testData["ExpectedLateFee"].Equals("True", StringComparison.OrdinalIgnoreCase);

            if (expectation == "be displayed")
            {
                Assert.True(isLateFeeDisplayed, "Late fee message should be displayed.");
                Assert.True(expectedLateFee, "ExpectedLateFee should be True in test data.");
            }
            else
            {
                Assert.False(isLateFeeDisplayed, "Late fee message should not be displayed.");
                Assert.False(expectedLateFee, "ExpectedLateFee should be False in test data.");
            }
        }

        private string GetTestCaseId()
        {
            return _scenarioContext.ScenarioInfo.Arguments["TestCaseId"].ToString();
        }
    }
}