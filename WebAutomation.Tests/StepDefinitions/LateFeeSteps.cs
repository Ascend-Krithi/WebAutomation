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

        [Given(@"the customer servicing application is launched")]
        public void GivenTheCustomerServicingApplicationIsLaunched()
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

        [Given(@"the user logs in with valid credentials")]
        public void GivenTheUserLogsInWithValidCredentials()
        {
            _loginPage.LoginWithDefaultCredentials();
            _wait.UntilVisible(_mfaPage.DialogLocator);
        }

        [Given(@"the user completes MFA verification")]
        public void GivenTheUserCompletesMFAVerification()
        {
            _mfaPage.SelectFirstEmailMethod();
            _mfaPage.ClickSendCode();
            _wait.UntilVisible(_otpPage.CodeInputLocator);
            _otpPage.EnterStaticOtpAndVerify();
            _wait.UntilVisible(_dashboardPage.PageReadyLocator);
        }

        [Given(@"the user is on the dashboard")]
        public void GivenTheUserIsOnTheDashboard()
        {
            Assert.True(_dashboardPage.IsPageReady());
        }

        [Given(@"all pop-ups are dismissed")]
        public void GivenAllPopUpsAreDismissed()
        {
            _dashboardPage.DismissAllPopups();
        }

        [Given(@"the user selects the applicable loan account")]
        public void GivenTheUserSelectsTheApplicableLoanAccount()
        {
            var featureContext = _scenarioContext.ScenarioInfo;
            var testCaseId = featureContext.Arguments["TestCaseId"].ToString();
            var filePath = ConfigManager.Settings.TestDataPath + "/LateFee.xlsx";
            _testData = ExcelReader.GetRow(filePath, "Sheet1", "TestCaseId", testCaseId);

            var loanNumber = _testData["LoanNumber"];
            _dashboardPage.SelectLoanAccount(loanNumber);
        }

        [When(@"the user clicks Make a Payment")]
        public void WhenTheUserClicksMakeAPayment()
        {
            _dashboardPage.ClickMakePayment();
        }

        [When(@"the user continues past any scheduled payment popup")]
        public void WhenTheUserContinuesPastAnyScheduledPaymentPopup()
        {
            _paymentPage.ContinueScheduledPaymentPopupIfPresent();
        }

        [When(@"the user opens the payment date picker")]
        public void WhenTheUserOpensThePaymentDatePicker()
        {
            _paymentPage.OpenDatePicker();
        }

        [When(@"the user selects the payment date from test data")]
        public void WhenTheUserSelectsThePaymentDateFromTestData()
        {
            var paymentDate = _testData["PaymentDate"];
            _paymentPage.SelectPaymentDate(paymentDate);
        }

        [Then(@"the late-fee message area should display the expected late fee message")]
        public void ThenTheLateFeeMessageAreaShouldDisplayTheExpectedLateFeeMessage()
        {
            var expectedLateFee = _testData["ExpectedLateFee"];
            bool isLateFeeDisplayed = _paymentPage.IsLateFeeMessageDisplayed();

            if (expectedLateFee.Equals("True", StringComparison.OrdinalIgnoreCase))
            {
                Assert.True(isLateFeeDisplayed, "Late fee message should be displayed.");
            }
            else
            {
                Assert.False(isLateFeeDisplayed, "Late fee message should not be displayed.");
            }
        }
    }
}