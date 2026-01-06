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
        private IWebDriver _driver;
        private Dictionary<string, string> _testData;
        private string _testDataPath;
        private string _sheetName = "Sheet1"; // Adjust if sheet name differs
        private DashboardPage _dashboardPage;
        private LoginPage _loginPage;
        private MfaPage _mfaPage;
        private PaymentPage _paymentPage;

        public LateFeeSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"I launch the customer servicing application")]
        public void GivenILaunchTheCustomerServicingApplication()
        {
            _driver = _scenarioContext.Get<IWebDriver>("driver");
            _testDataPath = ConfigManager.Settings.TestDataPath;
            _driver.Navigate().GoToUrl(ConfigManager.Settings.BaseUrl);
            _loginPage = new LoginPage(_driver);
        }

        [Given(@"I log in using valid customer credentials")]
        public void GivenILogInUsingValidCustomerCredentials()
        {
            _loginPage.LoginWithDefaultCredentials();
            _mfaPage = new MfaPage(_driver);
        }

        [Given(@"I complete MFA verification")]
        public void GivenICompleteMFAVerification()
        {
            _mfaPage.CompleteMfaVerification();
            _dashboardPage = new DashboardPage(_driver);
        }

        [Given(@"I navigate to the dashboard")]
        public void GivenINavigateToTheDashboard()
        {
            _dashboardPage.WaitForDashboardToLoad();
        }

        [Given(@"I dismiss any pop-ups if present")]
        public void GivenIDismissAnyPopUpsIfPresent()
        {
            _dashboardPage.DismissPopupsIfPresent();
        }

        [Given(@"I select the applicable loan account from test data "(.*)"")]
        public void GivenISelectTheApplicableLoanAccountFromTestData(string testCaseId)
        {
            _testData = ExcelReader.GetRow(_testDataPath, _sheetName, "TestCaseId", testCaseId);
            string loanNumber = _testData["LoanNumber"];
            _dashboardPage.SelectLoanAccount(loanNumber);
        }

        [Given(@"I click Make a Payment")]
        public void GivenIClickMakeAPayment()
        {
            _dashboardPage.ClickMakeAPayment();
            _paymentPage = new PaymentPage(_driver);
        }

        [Given(@"I continue past the scheduled payment popup if it appears")]
        public void GivenIContinuePastTheScheduledPaymentPopupIfItAppears()
        {
            _paymentPage.ContinueScheduledPaymentIfPresent();
        }

        [Given(@"I open the payment date picker")]
        public void GivenIOpenThePaymentDatePicker()
        {
            _paymentPage.OpenPaymentDatePicker();
        }

        [Given(@"I select the payment date from test data "(.*)"")]
        public void GivenISelectThePaymentDateFromTestData(string testCaseId)
        {
            string paymentDate = _testData["PaymentDate"];
            _paymentPage.SelectPaymentDate(paymentDate);
        }

        [Then(@"the late fee message area should be "(.*)"")]
        public void ThenTheLateFeeMessageAreaShouldBe(string expectedLateFeeMessage)
        {
            bool isLateFeeDisplayed = _paymentPage.IsLateFeeMessageDisplayed();
            if (expectedLateFeeMessage == "Displayed")
            {
                Assert.True(isLateFeeDisplayed, "Expected late fee message to be displayed, but it was not.");
            }
            else
            {
                Assert.False(isLateFeeDisplayed, "Expected late fee message to not be displayed, but it was.");
            }
        }
    }
}