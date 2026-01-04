Feature: Late Fee and Autopay Pending OTP Pop-up Handling

  Scenario Outline: Customer with pending OTP sees pop-up on Make a Payment and Setup Autopay actions
    Given the Customer Portal is launched
    And the user logs in with valid credentials for TestCaseId "<TestCaseId>"
    When the user navigates to the Account Dashboard
    And the user clicks on "<Action>"
    Then a pop-up with 'Continue' and 'Cancel' is displayed

    Examples:
      | TestCaseId | Action           |
      | TC01       | Make a Payment   |
      | TC07       | Setup Autopay    |

  Scenario Outline: Customer clicks Continue on pop-up and is routed to the correct page
    Given the Customer Portal is launched
    And the user logs in with valid credentials for TestCaseId "<TestCaseId>"
    When the user navigates to the Account Dashboard
    And the user clicks on "<Action>"
    And the user clicks 'Continue' on the pop-up
    Then the user is routed to the "<ExpectedPage>" page

    Examples:
      | TestCaseId | Action           | ExpectedPage      |
      | TC02       | Make a Payment   | Make a Payment    |
      | TC08       | Setup Autopay    | Setup Autopay     |

  Scenario Outline: Customer clicks Cancel on pop-up and remains on Dashboard
    Given the Customer Portal is launched
    And the user logs in with valid credentials for TestCaseId "<TestCaseId>"
    When the user navigates to the Account Dashboard
    And the user clicks on "<Action>"
    And the user clicks 'Cancel' on the pop-up
    Then the pop-up is dismissed and user remains on Account Dashboard

    Examples:
      | TestCaseId | Action           |
      | TC03       | Make a Payment   |
      | TC09       | Setup Autopay    |