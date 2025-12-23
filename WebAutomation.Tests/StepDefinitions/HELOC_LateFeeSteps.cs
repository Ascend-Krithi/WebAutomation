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
        private Dictionary<string, string> _testData;
        private string _testDataPath;
        private string _sheetName = "Sheet1";
        private string _selectedDate;

        public HELOC_LateFeeSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I launch the customer servicing application")]
        public void GivenILaunchTheCustomerServicingApplication()
        {
            _driver = _scenarioContext.Get<IWebDriver>("driver");
            _testDataPath = ConfigManager.Settings.TestDataPath;
            _driver.Navigate().GoToUrl(ConfigManager.Settings.BaseUrl);
            var loginPage = new LoginPage(_driver);
            Assert.True(loginPage.IsPageReady(), "Sign-In screen did not load.");
        }

        [Given(@"I log in as a valid customer")]
        public void GivenILogInAsAValidCustomer()
        {
            var testCaseId = _scenarioContext.ScenarioInfo.Arguments["TestCaseId"].ToString();
            _testData = ExcelReader.GetRow(_testDataPath, _sheetName, "TestCaseId", testCaseId);
            var loginPage = new LoginPage(_driver);
            loginPage.LoginWithDefaultCredentials();
        }

        [Given(@"I complete MFA verification")]
        public void GivenICompleteMFAVerification()
        {
            var mfaPage = new MfaPage(_driver);
            mfaPage.CompleteMfa();
        }

        [Given(@"I am on the dashboard")]
        public void GivenIAmOnTheDashboard()
        {
            var dashboardPage = new DashboardPage(_driver);
            Assert.True(dashboardPage.IsPageReady(), "Dashboard did not load.");
        }

        [Given(@"I dismiss all popups if present")]
        public void GivenIDismissAllPopupsIfPresent()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.DismissAllPopups();
        }

        [Given(@"I select the applicable loan account")]
        public void GivenISelectTheApplicableLoanAccount()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.SelectLoanByAccount(_testData["LoanNumber"]);
            Assert.True(dashboardPage.AreLoanDetailsVisible(), "Loan details did not load.");
        }

        [When(@"I click Make a Payment")]
        public void WhenIClickMakeAPayment()
        {
            var dashboardPage = new DashboardPage(_driver);
            dashboardPage.ClickMakePayment();
        }

        [When(@"I continue past the scheduled payment popup if it appears")]
        public void WhenIContinuePastTheScheduledPaymentPopupIfItAppears()
        {
            var paymentPage = new PaymentPage(_driver);
            paymentPage.ContinueScheduledPaymentPopupIfPresent();
        }

        [When(@"I open the payment date picker")]
        public void WhenIOpenThePaymentDatePicker()
        {
            var paymentPage = new PaymentPage(_driver);
            paymentPage.OpenDatePicker();
        }

        [When(@"I select the payment date from test data")]
        public void WhenISelectThePaymentDateFromTestData()
        {
            var paymentPage = new PaymentPage(_driver);
            _selectedDate = _testData["PaymentDate"];
            paymentPage.SelectPaymentDate(_selectedDate);
        }

        [Then(@"the payment date field should display the selected date")]
        public void ThenThePaymentDateFieldShouldDisplayTheSelectedDate()
        {
            var paymentPage = new PaymentPage(_driver);
            Assert.True(paymentPage.IsPaymentDateDisplayed(_selectedDate), "Selected date is not displayed in the payment date field.");
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
    }
}