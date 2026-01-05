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
        private LoginPage _loginPage;
        private MfaPage _mfaPage;
        private OtpPage _otpPage;
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

            _loginPage = new LoginPage(_driver);
            _mfaPage = new MfaPage(_driver);
            _otpPage = new OtpPage(_driver);
            _dashboardPage = new DashboardPage(_driver);
            _paymentPage = new PaymentPage(_driver);

            _driver.Navigate().GoToUrl(ConfigManager.Settings.BaseUrl);
            _wait.UntilVisible(_loginPage.PageReadyLocator());
        }

        [Given(@"logs in with valid credentials")]
        public void GivenLogsInWithValidCredentials()
        {
            _loginPage.LoginWithDefaultCredentials();
            _wait.UntilVisible(_mfaPage.PageReadyLocator());
        }

        [Given(@"completes MFA verification")]
        public void GivenCompletesMFAVerification()
        {
            _mfaPage.SelectFirstEmailAndSendCode();
            _otpPage.EnterStaticOtpAndVerify();
            _wait.UntilVisible(_dashboardPage.PageReadyLocator());
        }

        [Given(@"navigates to the dashboard")]
        public void GivenNavigatesToTheDashboard()
        {
            // Already on dashboard after OTP
            Assert.True(_dashboardPage.IsPageReady());
        }

        [Given(@"dismisses any pop-ups if present")]
        public void GivenDismissesAnyPopupsIfPresent()
        {
            _dashboardPage.DismissPopups();
        }

        [Given(@"selects the applicable loan account")]
        public void GivenSelectsTheApplicableLoanAccount(Table table)
        {
            var testCaseId = GetTestCaseId();
            _testData = ExcelReader.GetRow(
                filePath: "LateFee/LateFee.xlsx",
                sheetName: "Sheet1",
                keyColumn: "TestCaseId",
                keyValue: testCaseId
            );
            var loanNumber = _testData["LoanNumber"];
            _dashboardPage.SelectLoanAccount(loanNumber);
        }

        [When(@"the user clicks Make a Payment")]
        public void WhenTheUserClicksMakeAPayment()
        {
            _dashboardPage.ClickMakePayment();
        }

        [When(@"continues past the scheduled payment popup if it appears")]
        public void WhenContinuesPastTheScheduledPaymentPopupIfItAppears()
        {
            _paymentPage.ContinueScheduledPaymentIfPresent();
        }

        [When(@"opens the payment date picker")]
        public void WhenOpensThePaymentDatePicker()
        {
            _paymentPage.OpenDatePicker();
        }

        [When(@"selects the payment date from test data")]
        public void WhenSelectsThePaymentDateFromTestData()
        {
            var paymentDate = _testData["PaymentDate"];
            _paymentPage.SelectPaymentDate(paymentDate);
        }

        [Then(@"the late fee message area should (.*)")]
        public void ThenTheLateFeeMessageAreaShould(string expectation)
        {
            bool isLateFeeDisplayed = _paymentPage.IsLateFeeMessageDisplayed();
            if (expectation == "be displayed")
                Assert.True(isLateFeeDisplayed, "Expected late fee message to be displayed.");
            else
                Assert.False(isLateFeeDisplayed, "Expected late fee message to NOT be displayed.");
        }

        private string GetTestCaseId()
        {
            return _scenarioContext.ScenarioInfo.Arguments["TestCaseId"].ToString();
        }
    }
}