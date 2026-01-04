using System;
using System.Collections.Generic;
using NUnit.Framework;
using TechTalk.SpecFlow;
using WebAutomation.Core.Utilities;
using WebAutomation.Tests.Pages;

namespace WebAutomation.Tests.StepDefinitions
{
    [Binding]
    public class LateFeeSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private Dictionary<string, string> _testData;
        private IWebDriver _driver;
        private DashboardPage _dashboardPage;
        private LoginPage _loginPage;
        private MfaPage _mfaPage;
        private OtpPage _otpPage;
        private PaymentPage _paymentPage;

        public LateFeeSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"the customer logs in")]
        public void GivenTheCustomerLogsIn()
        {
            _driver = _scenarioContext.Get<IWebDriver>("driver");
            _loginPage = new LoginPage(_driver);
            _testData = ExcelReader.GetRow(
                "CUSTP-3799-TestData/CUSTP-3799-TestData.xlsx",
                "Sheet1",
                "TestCaseId",
                _scenarioContext.ScenarioInfo.Arguments["TestCaseId"].ToString()
            );
            _loginPage.LoginWithDefaultCredentials();
        }

        [Given(@"completes identity verification")]
        public void GivenCompletesIdentityVerification()
        {
            _mfaPage = new MfaPage(_driver);
            _mfaPage.SelectFirstEmailAndSendCode();
        }

        [Given(@"enters OTP and verifies")]
        public void GivenEntersOTPAndVerifies()
        {
            _otpPage = new OtpPage(_driver);
            _otpPage.EnterOtpAndVerify();
        }

        [Given(@"navigates to the dashboard")]
        public void GivenNavigatesToTheDashboard()
        {
            _dashboardPage = new DashboardPage(_driver);
            _dashboardPage.WaitForDashboard();
        }

        [Given(@"closes any popups if present")]
        public void GivenClosesAnyPopupsIfPresent()
        {
            _dashboardPage.ClosePopupsIfPresent();
        }

        [When(@"the customer selects loan number (.*)")]
        public void WhenTheCustomerSelectsLoanNumber(string loanNumber)
        {
            _dashboardPage.SelectLoanCard(loanNumber);
        }

        [When(@"clicks on Make a Payment")]
        public void WhenClicksOnMakeAPayment()
        {
            _dashboardPage.ClickMakePayment();
            _paymentPage = new PaymentPage(_driver);
            _paymentPage.WaitForPaymentPage();
        }

        [When(@"selects payment date (.*)")]
        public void WhenSelectsPaymentDate(string paymentDate)
        {
            _paymentPage.SelectPaymentDate(paymentDate);
        }

        [Then(@"the late fee message popup should be (.*)")]
        public void ThenTheLateFeeMessagePopupShouldBe(string expectedPopup)
        {
            bool isLateFeeMessageDisplayed = _paymentPage.IsLateFeeMessageDisplayed();
            if (expectedPopup.Equals("True", StringComparison.OrdinalIgnoreCase))
            {
                Assert.True(isLateFeeMessageDisplayed, "Late fee message should be displayed.");
            }
            else
            {
                Assert.False(isLateFeeMessageDisplayed, "Late fee message should not be displayed.");
            }
        }
    }
}