using System;
using System.Collections.Generic;
using NUnit.Framework;
using TechTalk.SpecFlow;
using WebAutomation.Core.Utilities;
using WebAutomation.Tests.Pages;

namespace WebAutomation.Tests.StepDefinitions
{
    [Binding]
    public class CustomerPortalPendingOtpSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private readonly string _excelFilePath = "LateFee 1/LateFee.xlsx";
        private readonly string _sheetName = "Sheet1";
        private Dictionary<string, string> _testData;
        private IWebDriver _driver;
        private DashboardPage _dashboardPage;

        public CustomerPortalPendingOtpSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"the Customer Portal is launched")]
        public void GivenTheCustomerPortalIsLaunched()
        {
            _driver = _scenarioContext.Get<IWebDriver>("driver");
            _dashboardPage = new DashboardPage(_driver);
            var baseUrl = WebAutomation.Core.Configuration.ConfigManager.Settings.BaseUrl;
            _driver.Navigate().GoToUrl(baseUrl);
            Assert.True(_dashboardPage.IsLoginPageDisplayed(), "Login page is not displayed.");
        }

        [Given(@"the user logs in with valid credentials for a pending OTP account")]
        public void GivenTheUserLogsInWithValidCredentialsForPendingOtpAccount()
        {
            var testCaseId = _scenarioContext.Get<string>("TestCaseId");
            _testData = ExcelReader.GetRow(_excelFilePath, _sheetName, "TestCaseId", testCaseId);
            _dashboardPage.Login(_testData["Username"], _testData["Password"]);
            Assert.True(_dashboardPage.IsDashboardDisplayed(), "Account Dashboard is not displayed.");
        }

        [Given(@"the user navigates to the Account Dashboard")]
        public void GivenTheUserNavigatesToTheAccountDashboard()
        {
            Assert.True(_dashboardPage.IsDashboardDisplayed(), "Account Dashboard did not load successfully.");
        }

        [When(@"the user clicks on \"(.*)\"")]
        public void WhenTheUserClicksOnAction(string action)
        {
            if (action == "Make a Payment")
            {
                _dashboardPage.ClickMakePayment();
            }
            else if (action == "Setup Autopay")
            {
                _dashboardPage.ClickSetupAutopay();
            }
        }

        [Then(@"a pop-up with 'Continue' and 'Cancel' buttons appears")]
        public void ThenAPopupWithContinueAndCancelButtonsAppears()
        {
            Assert.True(_dashboardPage.IsPendingOtpPopupDisplayed(), "Pending OTP pop-up is not displayed.");
        }

        [When(@"the user clicks 'Continue' on the pop-up")]
        public void WhenTheUserClicksContinueOnThePopup()
        {
            _dashboardPage.ClickPopupContinue();
        }

        [Then(@"the user is routed to the \"(.*)\" page")]
        public void ThenTheUserIsRoutedToThePage(string routedPage)
        {
            if (routedPage == "Make a Payment")
            {
                Assert.True(_dashboardPage.IsMakePaymentPageDisplayed(), "Make a Payment page is not displayed.");
            }
            else if (routedPage == "Setup Autopay")
            {
                Assert.True(_dashboardPage.IsSetupAutopayPageDisplayed(), "Setup Autopay page is not displayed.");
            }
        }

        [When(@"the user clicks 'Cancel' on the pop-up")]
        public void WhenTheUserClicksCancelOnThePopup()
        {
            _dashboardPage.ClickPopupCancel();
        }

        [Then(@"the pop-up is dismissed and user remains on Account Dashboard")]
        public void ThenThePopupIsDismissedAndUserRemainsOnAccountDashboard()
        {
            Assert.False(_dashboardPage.IsPendingOtpPopupDisplayed(), "Pending OTP pop-up is still displayed.");
            Assert.True(_dashboardPage.IsDashboardDisplayed(), "User is not on Account Dashboard.");
        }
    }
}