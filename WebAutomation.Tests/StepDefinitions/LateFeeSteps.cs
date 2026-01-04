using System;
using System.Collections.Generic;
using NUnit.Framework;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
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
        private DashboardPage _dashboardPage;
        private LoginPage _loginPage;
        private PopupPage _popupPage;

        public LateFeeSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"the Customer Portal is launched")]
        public void GivenTheCustomerPortalIsLaunched()
        {
            _driver = _scenarioContext.Get<IWebDriver>("driver");
            _driver.Navigate().GoToUrl(WebAutomation.Core.Configuration.ConfigManager.Settings.BaseUrl);
        }

        [Given(@"the user logs in with valid credentials for TestCaseId "(.*)"")]
        public void GivenTheUserLogsInWithValidCredentialsForTestCaseId(string testCaseId)
        {
            _driver = _scenarioContext.Get<IWebDriver>("driver");
            _loginPage = new LoginPage(_driver);

            string testDataPath = WebAutomation.Core.Configuration.ConfigManager.Settings.TestDataPath + "/CUSTP-3799-TestData.xlsx";
            _testData = ExcelReader.GetRow(testDataPath, "Sheet1", "TestCaseId", testCaseId);

            string username = _testData.ContainsKey("Username") ? _testData["Username"] : "customer1";
            string password = _testData.ContainsKey("Password") ? _testData["Password"] : "Pass@123";

            _loginPage.Login(username, password);
        }

        [When(@"the user navigates to the Account Dashboard")]
        public void WhenTheUserNavigatesToTheAccountDashboard()
        {
            _dashboardPage = new DashboardPage(_driver);
            Assert.True(_dashboardPage.IsLoaded(), "Dashboard page did not load successfully.");
        }

        [When(@"the user clicks on "(.*)"")]
        public void WhenTheUserClicksOn(string action)
        {
            _dashboardPage = new DashboardPage(_driver);
            if (action == "Make a Payment")
                _dashboardPage.ClickMakeAPayment();
            else if (action == "Setup Autopay")
                _dashboardPage.ClickSetupAutopay();
        }

        [Then(@"a pop-up with 'Continue' and 'Cancel' is displayed")]
        public void ThenAPopUpWithContinueAndCancelIsDisplayed()
        {
            _popupPage = new PopupPage(_driver);
            Assert.True(_popupPage.IsPopupDisplayed(), "Expected pop-up was not displayed.");
            Assert.True(_popupPage.IsContinueButtonDisplayed(), "'Continue' button not displayed.");
            Assert.True(_popupPage.IsCancelButtonDisplayed(), "'Cancel' button not displayed.");
        }

        [When(@"the user clicks 'Continue' on the pop-up")]
        public void WhenTheUserClicksContinueOnThePopUp()
        {
            _popupPage = new PopupPage(_driver);
            _popupPage.ClickContinue();
        }

        [Then(@"the user is routed to the "(.*)" page")]
        public void ThenTheUserIsRoutedToThePage(string expectedPage)
        {
            if (expectedPage == "Make a Payment")
            {
                var paymentPage = new PaymentPage(_driver);
                Assert.True(paymentPage.IsLoaded(), "Make a Payment page did not load.");
            }
            else if (expectedPage == "Setup Autopay")
            {
                var autopayPage = new AutopayPage(_driver);
                Assert.True(autopayPage.IsLoaded(), "Setup Autopay page did not load.");
            }
        }

        [When(@"the user clicks 'Cancel' on the pop-up")]
        public void WhenTheUserClicksCancelOnThePopUp()
        {
            _popupPage = new PopupPage(_driver);
            _popupPage.ClickCancel();
        }

        [Then(@"the pop-up is dismissed and user remains on Account Dashboard")]
        public void ThenThePopUpIsDismissedAndUserRemainsOnAccountDashboard()
        {
            _popupPage = new PopupPage(_driver);
            Assert.False(_popupPage.IsPopupDisplayed(), "Pop-up was not dismissed.");
            _dashboardPage = new DashboardPage(_driver);
            Assert.True(_dashboardPage.IsLoaded(), "User is not on Account Dashboard.");
        }
    }
}