using NUnit.Framework;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
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
        private LoginPage _loginPage;
        private MfaPage _mfaPage;
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
            _loginPage = new LoginPage(_driver);
            _mfaPage = new MfaPage(_driver);
            _dashboardPage = new DashboardPage(_driver);
            _paymentPage = new PaymentPage(_driver);

            var baseUrl = WebAutomation.Core.Configuration.ConfigManager.Settings.BaseUrl;
            _driver.Navigate().GoToUrl(baseUrl);
            Assert.True(_loginPage.IsPageReady(), "Login page did not load successfully.");
        }

        [When(@"the user logs in with valid credentials")]
        public void WhenTheUserLogsInWithValidCredentials()
        {
            _loginPage.LoginWithDefaultCredentials();
            Assert.True(_mfaPage.IsPageReady(), "MFA page did not load after login.");
        }

        [When(@"completes MFA verification")]
        public void WhenCompletesMFAVerification()
        {
            _mfaPage.CompleteMfa();
            Assert.True(_mfaPage.IsOtpPageReady(), "OTP page did not load after MFA.");
            _mfaPage.EnterOtpAndVerify();
            Assert.True(_dashboardPage.IsPageReady(), "Dashboard did not load after OTP verification.");
        }

        [When(@"navigates to the dashboard")]
        public void WhenNavigatesToTheDashboard()
        {
            Assert.True(_dashboardPage.IsPageReady(), "Dashboard page did not load.");
        }

        [When(@"dismisses any pop-ups if present")]
        public void WhenDismissesAnyPopupsIfPresent()
        {
            _dashboardPage.DismissPopupsIfPresent();
        }

        [When(@"selects the applicable loan account")]
        public void WhenSelectsTheApplicableLoanAccount()
        {
            // Read test data
            var featureFile = "LateFee/LateFee.xlsx";
            var sheet = "Sheet1";
            var testCaseId = _scenarioContext.ScenarioInfo.Arguments["TestCaseId"].ToString();
            _testData = ExcelReader.GetRow(featureFile, sheet, "TestCaseId", testCaseId);

            var loanNumber = _testData["LoanNumber"];
            _dashboardPage.SelectLoanAccount(loanNumber);
            Assert.True(_dashboardPage.IsLoanDetailsLoaded(loanNumber), "Loan details did not load.");
        }

        [When(@"clicks Make a Payment")]
        public void WhenClicksMakeAPayment()
        {
            _dashboardPage.ClickMakePayment();
        }

        [When(@"continues past scheduled payment popup if present")]
        public void WhenContinuesPastScheduledPaymentPopupIfPresent()
        {
            _dashboardPage.ContinueScheduledPaymentIfPresent();
            Assert.True(_paymentPage.IsPageReady(), "Make a Payment page did not load.");
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

        [Then(@"no late fee message is displayed")]
        public void ThenNoLateFeeMessageIsDisplayed()
        {
            Assert.False(_paymentPage.IsLateFeeMessageDisplayed(), "Late fee message was displayed but should not be.");
        }
    }
}