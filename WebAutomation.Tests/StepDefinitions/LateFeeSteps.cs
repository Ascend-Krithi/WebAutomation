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
        private string _testCaseId;
        private IWebDriver _driver;
        private SmartWait _wait;
        private PopupHandler _popup;

        public LateFeeSteps(ScenarioContext scenarioContext)
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
        }

        [Given(@"logs in with valid customer credentials")]
        public void GivenLogsInWithValidCustomerCredentials()
        {
            var loginPage = new LoginPage(_driver);
            loginPage.WaitForPageReady();
            loginPage.LoginWithDefaultCredentials();
        }

        [Given(@"completes MFA verification")]
        public void GivenCompletesMFAVerification()
        {
            var mfaPage = new MfaPage(_driver);
            mfaPage.CompleteMfa();
        }

        [Given(@"navigates to the dashboard")]
        public void GivenNavigatesToTheDashboard()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.WaitForPageReady();
        }

        [Given(@"dismisses any pop-ups if present")]
        public void GivenDismissesAnyPopupsIfPresent()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.DismissPopupsIfPresent();
        }

        [Given(@"selects the applicable loan account")]
        public void GivenSelectsTheApplicableLoanAccount()
        {
            _testCaseId = _scenarioContext.Get<string>("TestCaseId");
            _testData = ExcelReader.GetRow(_testDataPath, _sheetName, "TestCaseId", _testCaseId);
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.SelectLoanAccount(_testData["LoanNumber"]);
        }

        [Given(@"clicks Make a Payment")]
        public void GivenClicksMakeAPayment()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.ClickMakePayment();
        }

        [Given(@"continues past the scheduled payment popup if present")]
        public void GivenContinuesPastTheScheduledPaymentPopupIfPresent()
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

        [Given(@"selects the payment date from test data")]
        public void GivenSelectsThePaymentDateFromTestData()
        {
            var paymentPage = new PaymentPage(_driver);
            paymentPage.SelectPaymentDate(_testData["PaymentDate"]);
        }

        [When(@"the user observes the late-fee message area")]
        public void WhenTheUserObservesTheLateFeeMessageArea()
        {
            // No action needed, assertion in next step
        }

        [Then(@"the late fee message display should be "(.*)"")]
        public void ThenTheLateFeeMessageDisplayShouldBe(string expected)
        {
            var paymentPage = new PaymentPage(_driver);
            bool isLateFeeDisplayed = paymentPage.IsLateFeeMessageDisplayed();

            if (expected == "Displayed")
            {
                Assert.True(isLateFeeDisplayed, "Late fee message should be displayed.");
            }
            else
            {
                Assert.False(isLateFeeDisplayed, "Late fee message should not be displayed.");
            }
        }

        [BeforeScenario]
        public void BeforeScenario(ScenarioContext scenarioContext)
        {
            if (scenarioContext.ScenarioInfo.Arguments.ContainsKey("TestCaseId"))
            {
                scenarioContext.Set(scenarioContext.ScenarioInfo.Arguments["TestCaseId"].ToString(), "TestCaseId");
            }
        }
    }
}