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
    public class MakeAPaymentSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly string _testDataPath;
        private Dictionary<string, string> _testData;
        private IWebDriver _driver;
        private SmartWait _wait;
        private PopupHandler _popup;

        public MakeAPaymentSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _testDataPath = ConfigManager.Settings.TestDataPath;
        }

        [Given(@"the user launches the customer servicing application")]
        public void GivenTheUserLaunchesTheCustomerServicingApplication()
        {
            _driver = _scenarioContext.Get<IWebDriver>("driver");
            _wait = _scenarioContext.Get<SmartWait>("wait");
            _popup = _scenarioContext.Get<PopupHandler>("popup");
            _driver.Navigate().GoToUrl(ConfigManager.Settings.BaseUrl);
            var loginPage = new LoginPage(_driver);
            Assert.True(loginPage.IsPageReady(), "Login page did not load successfully.");
        }

        [Given(@"logs in with valid customer credentials")]
        public void GivenLogsInWithValidCustomerCredentials()
        {
            var loginPage = new LoginPage(_driver);
            loginPage.LoginWithDefaultCredentials();
            var mfaPage = new MfaPage(_driver);
            Assert.True(mfaPage.IsDialogDisplayed(), "MFA dialog was not displayed after login.");
        }

        [Given(@"completes MFA verification")]
        public void GivenCompletesMFAVerification()
        {
            var mfaPage = new MfaPage(_driver);
            mfaPage.SelectFirstEmailAndSendCode();
            var otpPage = new OtpPage(_driver);
            otpPage.EnterStaticOtpAndVerify();
            var dashboardPage = new DashboardPage(_driver);
            Assert.True(dashboardPage.IsPageReady(), "Dashboard page did not load after MFA.");
        }

        [Given(@"navigates to the dashboard")]
        public void GivenNavigatesToTheDashboard()
        {
            var dashboardPage = new DashboardPage(_driver);
            Assert.True(dashboardPage.IsPageReady(), "Dashboard page did not load.");
        }

        [Given(@"dismisses any pop-ups if present")]
        public void GivenDismissesAnyPopupsIfPresent()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.DismissPopups();
        }

        [Given(@"selects the applicable loan account")]
        public void GivenSelectsTheApplicableLoanAccount(Table table)
        {
            var testCaseId = table.Rows[0]["TestCaseId"];
            _testData = ExcelReader.GetRow(
                _testDataPath + "/MakeAPayment.xlsx",
                "Sheet1",
                "TestCaseId",
                testCaseId
            );
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.SelectLoanAccount(_testData["LoanNumber"]);
        }

        [When(@"the user clicks Make a Payment")]
        public void WhenTheUserClicksMakeAPayment()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.ClickMakePayment();
        }

        [When(@"continues past the scheduled payment popup if it appears")]
        public void WhenContinuesPastScheduledPaymentPopupIfItAppears()
        {
            var paymentPage = new PaymentPage(_driver);
            paymentPage.ContinueScheduledPaymentPopupIfPresent();
        }

        [When(@"opens the payment date picker")]
        public void WhenOpensThePaymentDatePicker()
        {
            var paymentPage = new PaymentPage(_driver);
            paymentPage.OpenDatePicker();
        }

        [When(@"selects the payment date ""(.*)"" from test data")]
        public void WhenSelectsThePaymentDateFromTestData(string paymentDate)
        {
            var paymentPage = new PaymentPage(_driver);
            paymentPage.SelectPaymentDate(paymentDate);
        }

        [Then(@"no late fee message is displayed")]
        public void ThenNoLateFeeMessageIsDisplayed()
        {
            var paymentPage = new PaymentPage(_driver);
            Assert.False(paymentPage.IsLateFeeMessageDisplayed(), "Late fee message was displayed when it should not be.");
        }
    }
}