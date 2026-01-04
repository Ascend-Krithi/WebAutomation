Feature: Customer Portal - Pending OTP Scenarios

  Scenario Outline: Customer with pending OTP attempts payment and autopay actions
    Given the Customer Portal is launched
    And the user logs in with valid credentials for a pending OTP account
    And the user navigates to the Account Dashboard
    When the user clicks on "<Action>"
    Then a pop-up with 'Continue' and 'Cancel' buttons appears

    Examples:
      | TestCaseId                | Action             |
      | HAP-903 TS-001 TC-001     | Make a Payment     |
      | HAP-903 TS-004 TC-001     | Setup Autopay      |

  Scenario Outline: Customer with pending OTP continues payment/autopay
    Given the Customer Portal is launched
    And the user logs in with valid credentials for a pending OTP account
    And the user navigates to the Account Dashboard
    When the user clicks on "<Action>"
    And the user clicks 'Continue' on the pop-up
    Then the user is routed to the "<RoutedPage>" page

    Examples:
      | TestCaseId                | Action             | RoutedPage        |
      | HAP-903 TS-002 TC-001     | Make a Payment     | Make a Payment    |
      | HAP-903 TS-005 TC-001     | Setup Autopay      | Setup Autopay     |

  Scenario Outline: Customer with pending OTP cancels payment/autopay
    Given the Customer Portal is launched
    And the user logs in with valid credentials for a pending OTP account
    And the user navigates to the Account Dashboard
    When the user clicks on "<Action>"
    And the user clicks 'Cancel' on the pop-up
    Then the pop-up is dismissed and user remains on Account Dashboard

    Examples:
      | TestCaseId                | Action             |
      | HAP-903 TS-003 TC-001     | Make a Payment     |