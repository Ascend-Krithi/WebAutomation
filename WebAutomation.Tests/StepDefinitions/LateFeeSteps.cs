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
        private readonly string _testDataPath;
        private readonly string _sheetName = "TestCases";
        private Dictionary<string, string> _testData;
        private IWebDriver _driver;
        private LoginPage _loginPage;
        private MfaPage _mfaPage;
        private OtpPage _otpPage;
        private DashboardPage _dashboardPage;
        private PaymentPage _paymentPage;

        public LateFeeSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _testDataPath = ConfigManager.Settings.TestDataPath + "/Latefee.xlsx";
        }

        [Given(@"the customer servicing application is launched")]
        public void GivenTheCustomerServicingApplicationIsLaunched()
        {
            _driver = _scenarioContext.Get<IWebDriver>("driver");
            _driver.Navigate().GoToUrl(ConfigManager.Settings.BaseUrl);
            _loginPage = new LoginPage(_driver);
            Assert.True(_loginPage.IsPageReady(), "Login page did not load.");
        }

        [Given(@"the user logs in with valid credentials")]
        public void GivenTheUserLogsInWithValidCredentials()
        {
            _loginPage.LoginWithDefaultCredentials();
            _mfaPage = new MfaPage(_driver);
            Assert.True(_mfaPage.IsPageReady(), "MFA page did not load.");
        }

        [Given(@"the user completes MFA verification")]
        public void GivenTheUserCompletesMFAVerification()
        {
            _mfaPage.SelectFirstEmailAndSendCode();
            _otpPage = new OtpPage(_driver);
            Assert.True(_otpPage.IsPageReady(), "OTP page did not load.");
            _otpPage.EnterStaticOtpAndVerify();
            _dashboardPage = new DashboardPage(_driver);
            Assert.True(_dashboardPage.IsPageReady(), "Dashboard page did not load.");
        }

        [Given(@"the dashboard is loaded")]
        public void GivenTheDashboardIsLoaded()
        {
            // Already asserted in previous step
        }

        [Given(@"all pop-ups are dismissed if present")]
        public void GivenAllPopupsAreDismissedIfPresent()
        {
            _dashboardPage.DismissAllPopups();
        }

        [Given(@"the user selects the applicable loan account")]
        public void GivenTheUserSelectsTheApplicableLoanAccount()
        {
            string testCaseId = GetTestCaseId();
            _testData = ExcelReader.GetRow(_testDataPath, _sheetName, "TestCaseId", testCaseId);
            string loanNumber = _testData["LoanNumber"];
            _dashboardPage.SelectLoanAccount(loanNumber);
        }

        [Given(@"the user clicks Make a Payment")]
        public void GivenTheUserClicksMakeAPayment()
        {
            _dashboardPage.ClickMakePayment();
            _paymentPage = new PaymentPage(_driver);
        }

        [Given(@"the user continues past the scheduled payment popup if present")]
        public void GivenTheUserContinuesPastTheScheduledPaymentPopupIfPresent()
        {
            _paymentPage.ContinueScheduledPaymentPopupIfPresent();
        }

        [Given(@"the user opens the payment date picker")]
        public void GivenTheUserOpensThePaymentDatePicker()
        {
            _paymentPage.OpenDatePicker();
        }

        [Given(@"the user selects the payment date from test data")]
        public void GivenTheUserSelectsThePaymentDateFromTestData()
        {
            string paymentDate = _testData["PaymentDate"];
            _paymentPage.SelectPaymentDate(paymentDate);
        }

        [Then(@"the late fee message area should (.*)")]
        public void ThenTheLateFeeMessageAreaShould(string expectation)
        {
            bool isLateFeeDisplayed = _paymentPage.IsLateFeeMessageDisplayed();
            if (expectation == "be displayed")
            {
                Assert.True(isLateFeeDisplayed, "Late fee message was expected but not displayed.");
            }
            else
            {
                Assert.False(isLateFeeDisplayed, "Late fee message was not expected but was displayed.");
            }
        }

        private string GetTestCaseId()
        {
            var table = _scenarioContext.ScenarioInfo.Arguments;
            return table.ContainsKey("TestCaseId") ? table["TestCaseId"].ToString() : string.Empty;
        }
    }
}