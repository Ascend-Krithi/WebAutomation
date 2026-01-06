using System;
using System.Collections.Generic;
using NUnit.Framework;
using TechTalk.SpecFlow;
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

            var baseUrl = WebAutomation.Core.Configuration.ConfigManager.Settings.BaseUrl;
            _driver.Navigate().GoToUrl(baseUrl);
            _loginPage.WaitForPageReady();
        }

        [Given(@"logs in using valid customer credentials")]
        public void GivenLogsInUsingValidCustomerCredentials()
        {
            _loginPage.LoginWithDefaultCredentials();
        }

        [Given(@"completes MFA verification")]
        public void GivenCompletesMFAVerification()
        {
            _mfaPage.CompleteMfa();
            _otpPage.EnterOtpAndVerify();
        }

        [Given(@"navigates to the dashboard")]
        public void GivenNavigatesToTheDashboard()
        {
            _dashboardPage.WaitForPageReady();
        }

        [Given(@"dismisses any pop-ups if present")]
        public void GivenDismissesAnyPopUpsIfPresent()
        {
            _dashboardPage.HandlePopups();
        }

        [Given(@"selects the applicable loan account")]
        public void GivenSelectsTheApplicableLoanAccount()
        {
            var featureFilePath = "LateFee/LateFee.xlsx";
            var sheetName = "Sheet1";
            var testCaseId = _scenarioContext.Get<string>("TestCaseId");
            _testData = ExcelReader.GetRow(featureFilePath, sheetName, "TestCaseId", testCaseId);

            var loanNumber = _testData["LoanNumber"];
            _dashboardPage.SelectLoanAccount(loanNumber);
        }

        [Given(@"clicks Make a Payment")]
        public void GivenClicksMakeAPayment()
        {
            _dashboardPage.ClickMakePayment();
        }

        [Given(@"continues past any scheduled payment popup if present")]
        public void GivenContinuesPastAnyScheduledPaymentPopupIfPresent()
        {
            _paymentPage.HandleScheduledPaymentPopup();
        }

        [Given(@"opens the payment date picker")]
        public void GivenOpensThePaymentDatePicker()
        {
            _paymentPage.OpenDatePicker();
        }

        [Given(@"selects the payment date from test data")]
        public void GivenSelectsThePaymentDateFromTestData()
        {
            var paymentDate = _testData["PaymentDate"];
            _paymentPage.SelectPaymentDate(paymentDate);
        }

        [Then(@"the late fee message area should (.*)")]
        public void ThenTheLateFeeMessageAreaShould(string expectation)
        {
            var expectedLateFee = _testData["ExpectedLateFee"].Equals("True", StringComparison.OrdinalIgnoreCase);

            bool isLateFeeDisplayed = _paymentPage.IsLateFeeMessageDisplayed();

            if (expectedLateFee)
            {
                Assert.True(isLateFeeDisplayed, "Late fee message should be displayed but was not.");
            }
            else
            {
                Assert.False(isLateFeeDisplayed, "Late fee message should not be displayed but was.");
            }
        }
    }
}