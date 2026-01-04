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
        private SmartWait _wait;
        private PopupHandler _popup;
        private LoginPage _loginPage;
        private DashboardPage _dashboardPage;

        public LateFeeSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given(@"the Customer Portal is launched")]
        public void GivenTheCustomerPortalIsLaunched()
        {
            _driver = _scenarioContext.Get<IWebDriver>("driver");
            _wait = _scenarioContext.Get<SmartWait>("wait");
            _popup = _scenarioContext.Get<PopupHandler>("popup");
            _loginPage = new LoginPage(_driver);
            _dashboardPage = new DashboardPage(_driver);

            // Read test data
            var testCaseId = _scenarioContext.ScenarioInfo.Arguments["TestCaseId"].ToString();
            var testDataPath = ConfigManager.Settings.TestDataPath + "/LateFee/LateFee.xlsx";
            _testData = ExcelReader.GetRow(testDataPath, "Sheet1", "TestCaseId", testCaseId);

            var baseUrl = ConfigManager.Settings.BaseUrl;
            _driver.Navigate().GoToUrl(baseUrl);
            Assert.IsTrue(_loginPage.IsPageReady(), "Login page is not displayed.");
        }

        [Given(@"the user logs in with credentials for a pending OTP account")]
        public void GivenTheUserLogsInWithCredentialsForPendingOtpAccount()
        {
            var username = _testData.ContainsKey("Username") ? _testData["Username"] : "customer1";
            var password = _testData.ContainsKey("Password") ? _testData["Password"] : "Pass@123";
            _loginPage.Login(username, password);
            _loginPage.HandleMfa();
            _loginPage.EnterOtpAndVerify();
            Assert.IsTrue(_dashboardPage.IsPageReady(), "Account Dashboard is not displayed.");
        }

        [Given(@"the user is on the Account Dashboard")]
        public void GivenTheUserIsOnTheAccountDashboard()
        {
            Assert.IsTrue(_dashboardPage.IsPageReady(), "Account Dashboard did not load successfully.");
        }

        [When(@"the user clicks \"(.*)\"")]
        public void WhenTheUserClicksAction(string action)
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

        [Then(@"a pop-up with 'Continue' and 'Cancel' appears")]
        public void ThenAPopupWithContinueAndCancelAppears()
        {
            Assert.IsTrue(_dashboardPage.IsPaymentPopupPresent(), "Expected pop-up with 'Continue' and 'Cancel' did not appear.");
        }

        [When(@"the user clicks 'Continue' on the pop-up")]
        public void WhenTheUserClicksContinueOnThePopup()
        {
            _dashboardPage.ClickPopupContinue();
        }

        [Then(@"the user is routed to the \"(.*)\" page")]
        public void ThenTheUserIsRoutedToThePage(string expectedPage)
        {
            if (expectedPage == "Make a Payment")
            {
                Assert.IsTrue(_dashboardPage.IsOnMakePaymentPage(), "User was not routed to the Make a Payment page.");
            }
            else if (expectedPage == "Setup Autopay")
            {
                Assert.IsTrue(_dashboardPage.IsOnSetupAutopayPage(), "User was not routed to the Setup Autopay page.");
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
            Assert.IsFalse(_dashboardPage.IsPaymentPopupPresent(), "Pop-up was not dismissed.");
            Assert.IsTrue(_dashboardPage.IsPageReady(), "User is not on the Account Dashboard.");
        }
    }
}