using System;
using System.Collections.Generic;
using NUnit.Framework;
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
        private DashboardPage _dashboardPage;
        private PaymentPage _paymentPage;

        public LateFeeSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"the user is on the login page")]
        public void GivenTheUserIsOnTheLoginPage()
        {
            _driver = _scenarioContext.Get<IWebDriver>("driver");
            _loginPage = new LoginPage(_driver);
            _dashboardPage = new DashboardPage(_driver);
            _paymentPage = new PaymentPage(_driver);
            _loginPage.NavigateToLoginPage();
        }

        [When(@"the user logs in with valid credentials for \"(.*)\"")]
        public void WhenTheUserLogsInWithValidCredentialsFor(string testCaseId)
        {
            string testDataPath = "TestData/LateFee/LateFee.xlsx";
            string sheetName = "Sheet1";
            _testData = ExcelReader.GetRow(testDataPath, sheetName, "TestCaseId", testCaseId);

            _loginPage.Login(_testData["LoanNumber"], _testData["State"]);
        }

        [When(@"the user navigates to the loan dashboard")]
        public void WhenTheUserNavigatesToTheLoanDashboard()
        {
            _dashboardPage.WaitForDashboard();
        }

        [When(@"the user selects loan account \"(.*)\"")]
        public void WhenTheUserSelectsLoanAccount(string loanNumber)
        {
            _dashboardPage.SelectLoanAccount(loanNumber);
        }

        [When(@"the user enters payment date \"(.*)\"")]
        public void WhenTheUserEntersPaymentDate(string paymentDate)
        {
            _paymentPage.EnterPaymentDate(paymentDate);
        }

        [Then(@"the late fee message is (.*) for \"(.*)\"")]
        public void ThenTheLateFeeMessageIsFor(string expectedLateFee, string testCaseId)
        {
            bool expected = bool.Parse(expectedLateFee);
            bool actual = _paymentPage.IsLateFeeMessageDisplayed();
            Assert.AreEqual(expected, actual, "Late fee message display status does not match expected.");
        }
    }
}