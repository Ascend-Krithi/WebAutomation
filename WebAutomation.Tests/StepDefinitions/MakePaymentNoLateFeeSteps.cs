using NUnit.Framework;
using TechTalk.SpecFlow;
using WebAutomation.Core.Utilities;
using WebAutomation.Tests.Pages;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace WebAutomation.Tests.StepDefinitions
{
    [Binding]
    public class MakePaymentNoLateFeeSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly string _excelFilePath = "TestData/LateFee.xlsx";
        private readonly string _sheetName = "Sheet1";
        private Dictionary<string, string> _testData;
        private IWebDriver _driver;
        private DashboardPage _dashboardPage;
        private PaymentPage _paymentPage;

        public MakePaymentNoLateFeeSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"the customer servicing application is launched")]
        public void GivenTheCustomerServicingApplicationIsLaunched()
        {
            _driver = _scenarioContext.Get<IWebDriver>("driver");
            _dashboardPage = new DashboardPage(_driver);
            _paymentPage = new PaymentPage(_driver);

            var baseUrl = WebAutomation.Core.Configuration.ConfigManager.Settings.BaseUrl;
            _driver.Navigate().GoToUrl(baseUrl);
            Assert.True(_dashboardPage.IsSignInScreenDisplayed(), "Sign-In screen was not displayed.");
        }

        [Given(@"I log in using valid customer credentials")]
        public void GivenILogInUsingValidCustomerCredentials()
        {
            _dashboardPage.LoginWithDefaultCredentials();
            Assert.True(_dashboardPage.IsMfaScreenDisplayed(), "MFA screen was not displayed after login.");
        }

        [Given(@"I complete MFA verification")]
        public void GivenICompleteMFAVerification()
        {
            _dashboardPage.CompleteMfaVerification();
            Assert.True(_dashboardPage.IsDashboardDisplayed(), "Dashboard was not displayed after MFA.");
        }

        [Given(@"I am on the dashboard")]
        public void GivenIAmOnTheDashboard()
        {
            Assert.True(_dashboardPage.IsDashboardDisplayed(), "Dashboard is not displayed.");
        }

        [Given(@"I dismiss any pop-ups if present")]
        public void GivenIDismissAnyPopUpsIfPresent()
        {
            _dashboardPage.DismissPopupsIfPresent();
        }

        [Given(@"I select the applicable loan account ""(.*)""")]
        public void GivenISelectTheApplicableLoanAccount(string loanNumber)
        {
            _dashboardPage.SelectLoanAccount(loanNumber);
            Assert.True(_dashboardPage.IsLoanDetailsDisplayed(loanNumber), "Loan details were not displayed.");
        }

        [When(@"I click Make a Payment")]
        public void WhenIClickMakeAPayment()
        {
            _dashboardPage.ClickMakePayment();
            Assert.True(_paymentPage.IsScheduledPaymentPopupPresent() || _paymentPage.IsMakePaymentScreenDisplayed(), "Scheduled payment popup or Make Payment screen was not displayed.");
        }

        [When(@"I continue past the scheduled payment popup if it appears")]
        public void WhenIContinuePastTheScheduledPaymentPopupIfItAppears()
        {
            _paymentPage.ContinueScheduledPaymentPopupIfPresent();
            Assert.True(_paymentPage.IsMakePaymentScreenDisplayed(), "Make Payment screen was not displayed after scheduled payment popup.");
        }

        [When(@"I open the payment date picker")]
        public void WhenIOpenThePaymentDatePicker()
        {
            _paymentPage.OpenPaymentDatePicker();
            Assert.True(_paymentPage.IsCalendarWidgetDisplayed(), "Calendar widget was not displayed.");
        }

        [When(@"I select the payment date ""(.*)""")]
        public void WhenISelectThePaymentDate(string paymentDate)
        {
            _paymentPage.SelectPaymentDate(paymentDate);
            Assert.True(_paymentPage.IsPaymentDateSelected(paymentDate), $"Payment date {paymentDate} was not selected.");
        }

        [Then(@"no late fee message is displayed")]
        public void ThenNoLateFeeMessageIsDisplayed()
        {
            Assert.False(_paymentPage.IsLateFeeMessageDisplayed(), "Late fee message was displayed but should not be.");
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            var testCaseId = _scenarioContext.ScenarioInfo.Arguments["TestCaseId"].ToString();
            _testData = ExcelReader.GetRow(_excelFilePath, _sheetName, "TestCaseId", testCaseId);
            _scenarioContext.Set(_testData, "testData");
        }
    }
}