using System;
using System.Collections.Generic;
using NUnit.Framework;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using WebAutomation.Core.Utilities;
using WebAutomation.Core.Configuration;
using WebAutomation.Tests.Pages;

namespace WebAutomation.Tests.StepDefinitions
{
    [Binding]
    public class HELOC_LateFeeSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver _driver;
        private Dictionary<string, string> _testData;
        private string _testDataPath;
        private string _sheetName = "Sheet1";

        public HELOC_LateFeeSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"the user launches the customer servicing application")]
        public void GivenTheUserLaunchesTheCustomerServicingApplication()
        {
            _driver = _scenarioContext.Get<IWebDriver>("driver");
            _testDataPath = ConfigManager.Settings.TestDataPath;
            _driver.Navigate().GoToUrl(ConfigManager.Settings.BaseUrl);
            var loginPage = new LoginPage(_driver);
            Assert.True(loginPage.IsPageReady(), "Sign-In screen did not load.");
        }

        [Given(@"logs in with valid credentials")]
        public void GivenLogsInWithValidCredentials()
        {
            var loginPage = new LoginPage(_driver);
            loginPage.LoginWithDefaultCredentials();
            var mfaPage = new MfaPage(_driver);
            Assert.True(mfaPage.IsDialogPresent(), "MFA verification screen did not appear.");
        }

        [Given(@"completes MFA verification")]
        public void GivenCompletesMFAVerification()
        {
            var mfaPage = new MfaPage(_driver);
            mfaPage.CompleteMfa();
            var dashboardPage = new DashboardPage(_driver);
            Assert.True(dashboardPage.IsPageReady(), "Dashboard did not load after MFA.");
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

        [Given(@"selects the applicable loan account ""(.*)""")]
        public void GivenSelectsTheApplicableLoanAccount(string loanNumber)
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.SelectLoanAccount(loanNumber);
            Assert.True(dashboardPage.IsLoanDetailsLoaded(), "Loan details did not load.");
        }

        [Given(@"clicks Make a Payment")]
        public void GivenClicksMakeAPayment()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.ClickMakePayment();
        }

        [Given(@"continues past the scheduled payment popup if it appears")]
        public void GivenContinuesPastTheScheduledPaymentPopupIfItAppears()
        {
            var paymentPage = new PaymentPage(_driver);
            paymentPage.ContinueScheduledPaymentPopupIfPresent();
        }

        [Given(@"opens the payment date picker")]
        public void GivenOpensThePaymentDatePicker()
        {
            var paymentPage = new PaymentPage(_driver);
            paymentPage.OpenDatePicker();
        }

        [When(@"the user selects the payment date ""(.*)""")]
        public void WhenTheUserSelectsThePaymentDate(string paymentDate)
        {
            var paymentPage = new PaymentPage(_driver);
            paymentPage.SelectPaymentDate(paymentDate);
        }

        [Then(@"no late fee message is displayed")]
        public void ThenNoLateFeeMessageIsDisplayed()
        {
            var paymentPage = new PaymentPage(_driver);
            Assert.False(paymentPage.IsLateFeeMessageDisplayed(), "Late fee message was unexpectedly displayed.");
        }

        [Then(@"a late fee message is displayed")]
        public void ThenALateFeeMessageIsDisplayed()
        {
            var paymentPage = new PaymentPage(_driver);
            Assert.True(paymentPage.IsLateFeeMessageDisplayed(), "Late fee message was not displayed.");
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _driver = _scenarioContext.Get<IWebDriver>("driver");
        }

        [StepArgumentTransformation]
        public string TransformTestCaseId(string testCaseId)
        {
            _testDataPath = ConfigManager.Settings.TestDataPath;
            _testData = ExcelReader.GetRow(_testDataPath, _sheetName, "TestCaseId", testCaseId);
            return testCaseId;
        }
    }
}