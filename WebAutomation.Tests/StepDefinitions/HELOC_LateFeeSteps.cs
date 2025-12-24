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
    public class HELOC_LateFeeSteps
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

        public HELOC_LateFeeSteps(ScenarioContext scenarioContext)
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
            _wait.UntilVisible(_loginPage.PageReadyLocator);
        }

        [Given(@"logs in with valid credentials")]
        public void GivenLogsInWithValidCredentials()
        {
            _loginPage.LoginWithDefaultCredentials();
            _wait.UntilVisible(_mfaPage.DialogLocator);
        }

        [Given(@"completes MFA verification")]
        public void GivenCompletesMFAVerification()
        {
            _mfaPage.SelectFirstEmailAndSendCode();
            _wait.UntilVisible(_otpPage.CodeInputLocator);
            _otpPage.EnterStaticOtpAndVerify();
            _wait.UntilVisible(_dashboardPage.PageReadyLocator);
        }

        [Given(@"the dashboard is displayed")]
        public void GivenTheDashboardIsDisplayed()
        {
            Assert.True(_dashboardPage.IsPageReady(), "Dashboard page did not load.");
        }

        [Given(@"all pop-ups are dismissed if present")]
        public void GivenAllPopupsAreDismissedIfPresent()
        {
            _dashboardPage.DismissAllPopups();
        }

        [Given(@"the user selects the applicable loan account")]
        public void GivenTheUserSelectsTheApplicableLoanAccount(Table table)
        {
            // TestCaseId is always present in Examples
            var testCaseId = _scenarioContext.ScenarioInfo.Arguments["TestCaseId"].ToString();
            var testDataPath = ConfigManager.Settings.TestDataPath;
            _testData = ExcelReader.GetRow($"{testDataPath}/HELOC_LateFee.xlsx", "Sheet1", "TestCaseId", testCaseId);

            var loanNumber = _testData["LoanNumber"];
            _dashboardPage.SelectLoanAccount(loanNumber);
        }

        [When(@"the user clicks Make a Payment")]
        public void WhenTheUserClicksMakeAPayment()
        {
            _dashboardPage.ClickMakePayment();
        }

        [When(@"continues past any scheduled payment popup if present")]
        public void WhenContinuesPastAnyScheduledPaymentPopupIfPresent()
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
                Assert.True(isLateFeeDisplayed, "Late fee message was not displayed but expected.");
            else
                Assert.False(isLateFeeDisplayed, "Late fee message was displayed but not expected.");
        }
    }
}