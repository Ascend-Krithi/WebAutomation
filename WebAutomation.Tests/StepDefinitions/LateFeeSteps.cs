using System;
using System.Collections.Generic;
using NUnit.Framework;
using TechTalk.SpecFlow;
using WebAutomation.Core.Configuration;
using WebAutomation.Core.Utilities;
using WebAutomation.Tests.Pages;

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
        private DashboardPage _dashboardPage;
        private PaymentPage _paymentPage;

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
            var loginPage = new LoginPage(_driver);
            loginPage.WaitForPageReady();
        }

        [Given(@"logs in using valid customer credentials")]
        public void GivenLogsInUsingValidCustomerCredentials()
        {
            var loginPage = new LoginPage(_driver);
            loginPage.LoginWithDefaultCredentials();
        }

        [Given(@"completes MFA verification")]
        public void GivenCompletesMFAVerification()
        {
            var mfaPage = new MfaPage(_driver);
            mfaPage.SelectFirstEmailMethod();
            mfaPage.ClickReceiveCode();
            var otpPage = new OtpPage(_driver);
            otpPage.EnterStaticOtpAndVerify();
        }

        [Given(@"navigates to the dashboard")]
        public void GivenNavigatesToTheDashboard()
        {
            _dashboardPage = new DashboardPage(_driver);
            _dashboardPage.WaitForPageReady();
        }

        [Given(@"closes any pop-ups if present")]
        public void GivenClosesAnyPopupsIfPresent()
        {
            _dashboardPage.HandlePopupsIfPresent();
        }

        [Given(@"selects the applicable loan account")]
        public void GivenSelectsTheApplicableLoanAccount()
        {
            var featureFilePath = "TestData/LateFee.xlsx";
            var sheetName = "Sheet1";
            var testCaseId = _scenarioContext.Get<string>("TestCaseId");
            _testData = ExcelReader.GetRow(featureFilePath, sheetName, "TestCaseId", testCaseId);
            _dashboardPage.SelectLoanAccount(_testData["LoanNumber"]);
        }

        [Given(@"clicks Make a Payment")]
        public void GivenClicksMakeAPayment()
        {
            _dashboardPage.ClickMakePayment();
        }

        [Given(@"continues past any scheduled payment popup if present")]
        public void GivenContinuesPastAnyScheduledPaymentPopupIfPresent()
        {
            _dashboardPage.HandleScheduledPaymentPopupIfPresent();
            _paymentPage = new PaymentPage(_driver);
            _paymentPage.WaitForPageReady();
        }

        [Given(@"opens the payment date picker")]
        public void GivenOpensThePaymentDatePicker()
        {
            _paymentPage.OpenDatePicker();
        }

        [Given(@"selects the payment date from test data")]
        public void GivenSelectsThePaymentDateFromTestData()
        {
            _paymentPage.SelectPaymentDate(_testData["PaymentDate"]);
        }

        [Then(@"the late fee message area should display the expected result")]
        public void ThenTheLateFeeMessageAreaShouldDisplayTheExpectedResult()
        {
            bool expectedLateFee = bool.Parse(_testData["ExpectedLateFee"]);
            bool isLateFeeDisplayed = _paymentPage.IsLateFeeMessageDisplayed();
            if (expectedLateFee)
            {
                Assert.True(isLateFeeDisplayed, "Late fee message should be displayed.");
            }
            else
            {
                Assert.False(isLateFeeDisplayed, "Late fee message should NOT be displayed.");
            }
        }
    }
}