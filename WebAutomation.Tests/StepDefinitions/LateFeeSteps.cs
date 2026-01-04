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
        private readonly string _excelPath = $"{ConfigManager.Settings.TestDataPath}/LateFee.xlsx";
        private readonly string _sheetName = "Sheet1";
        private Dictionary<string, string> _testData;
        private IWebDriver _driver;
        private SmartWait _wait;
        private PopupHandler _popup;

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
            _wait.UntilVisible(By.CssSelector("form"));
        }

        [Given(@"the user logs in with valid credentials")]
        public void GivenTheUserLogsInWithValidCredentials()
        {
            var loginPage = new LoginPage(_driver);
            loginPage.LoginWithDefaultCredentials();
        }

        [Given(@"the user completes MFA verification")]
        public void GivenTheUserCompletesMFAVerification()
        {
            var mfaPage = new MfaPage(_driver);
            mfaPage.CompleteMfa();
        }

        [Given(@"the user is on the dashboard")]
        public void GivenTheUserIsOnTheDashboard()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.WaitForDashboard();
        }

        [Given(@"the user dismisses any pop-ups if present")]
        public void GivenTheUserDismissesAnyPopupsIfPresent()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.DismissPopups();
        }

        [Given(@"the user selects the applicable loan account")]
        public void GivenTheUserSelectsTheApplicableLoanAccount()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.SelectLoanAccount(_testData["LoanNumber"]);
        }

        [When(@"the user clicks Make a Payment")]
        public void WhenTheUserClicksMakeAPayment()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.ClickMakePayment();
        }

        [When(@"the user continues past the scheduled payment popup if it appears")]
        public void WhenTheUserContinuesPastTheScheduledPaymentPopupIfItAppears()
        {
            var paymentPage = new PaymentPage(_driver);
            paymentPage.ContinueScheduledPaymentPopupIfPresent();
        }

        [When(@"the user opens the payment date picker")]
        public void WhenTheUserOpensThePaymentDatePicker()
        {
            var paymentPage = new PaymentPage(_driver);
            paymentPage.OpenDatePicker();
        }

        [When(@"the user selects the payment date from test data")]
        public void WhenTheUserSelectsThePaymentDateFromTestData()
        {
            var paymentPage = new PaymentPage(_driver);
            paymentPage.SelectPaymentDate(_testData["PaymentDate"]);
        }

        [Then(@"the late fee message area should (.*)")]
        public void ThenTheLateFeeMessageAreaShould(string expectation)
        {
            var paymentPage = new PaymentPage(_driver);
            bool isLateFeeDisplayed = paymentPage.IsLateFeeMessageDisplayed();

            if (expectation == "be displayed")
            {
                Assert.True(isLateFeeDisplayed, "Late fee message should be displayed but was not.");
            }
            else
            {
                Assert.False(isLateFeeDisplayed, "Late fee message should not be displayed but was.");
            }
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            // Get TestCaseId from SpecFlow context
            var testCaseId = _scenarioContext.ScenarioInfo.Arguments["TestCaseId"].ToString();
            _testData = ExcelReader.GetRow(_excelPath, _sheetName, "TestCaseId", testCaseId);
        }
    }
}