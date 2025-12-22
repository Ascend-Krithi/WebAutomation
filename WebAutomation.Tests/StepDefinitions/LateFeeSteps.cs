using System;
using System.Collections.Generic;
using NUnit.Framework;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using WebAutomation.Tests.Pages;
using WebAutomation.Core.Utilities;
using WebAutomation.Core.Configuration;

namespace WebAutomation.Tests.StepDefinitions
{
    [Binding]
    public class LateFeeSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private IWebDriver _driver;
        private Dictionary<string, string> _testData;
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
            _driver.Navigate().GoToUrl(ConfigManager.Settings.BaseUrl);
            _loginPage = new LoginPage(_driver);
        }

        [When(@"the user logs in with valid credentials")]
        public void WhenTheUserLogsInWithValidCredentials()
        {
            _loginPage.LoginWithDefaultCredentials();
            _mfaPage = new MfaPage(_driver);
        }

        [When(@"the user completes MFA verification")]
        public void WhenTheUserCompletesMFAVerification()
        {
            _mfaPage.CompleteMfa();
        }

        [When(@"the user navigates to the dashboard")]
        public void WhenTheUserNavigatesToTheDashboard()
        {
            _dashboardPage = new DashboardPage(_driver);
            _dashboardPage.WaitForDashboardReady();
        }

        [When(@"the user dismisses any pop-ups if present")]
        public void WhenTheUserDismissesAnyPopUpsIfPresent()
        {
            _dashboardPage.DismissPopupsIfPresent();
        }

        [When(@"the user selects the applicable loan account")]
        public void WhenTheUserSelectsTheApplicableLoanAccount(Table table)
        {
            string testCaseId = table.Rows[0]["TestCaseId"];
            _testData = ExcelReader.GetRow(
                filePath: "LateFee/LateFee.xlsx",
                sheetName: "Sheet1",
                keyColumn: "TestCaseId",
                keyValue: testCaseId
            );
            string loanNumber = _testData["LoanNumber"];
            _dashboardPage.SelectLoanAccount(loanNumber);
        }

        [When(@"the user clicks Make a Payment")]
        public void WhenTheUserClicksMakeAPayment()
        {
            _dashboardPage.ClickMakePayment();
            _paymentPage = new PaymentPage(_driver);
        }

        [When(@"the user continues past any scheduled payment popup if present")]
        public void WhenTheUserContinuesPastAnyScheduledPaymentPopupIfPresent()
        {
            _paymentPage.ContinueScheduledPaymentIfPresent();
        }

        [When(@"the user opens the payment date picker")]
        public void WhenTheUserOpensThePaymentDatePicker()
        {
            _paymentPage.OpenDatePicker();
        }

        [When(@"the user selects the payment date from test data")]
        public void WhenTheUserSelectsThePaymentDateFromTestData()
        {
            string paymentDate = _testData["PaymentDate"];
            _paymentPage.SelectPaymentDate(paymentDate);
        }

        [Then(@"the late-fee message area should not be displayed")]
        public void ThenTheLateFeeMessageAreaShouldNotBeDisplayed()
        {
            Assert.False(_paymentPage.IsLateFeeMessageDisplayed(), "Late fee message should not be displayed.");
        }
    }
}