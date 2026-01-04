Feature: Late Fee and Payment Pop-up Handling

  Scenario Outline: Customer with pending OTP sees payment/autopay pop-up
    Given the Customer Portal is launched
    And the user logs in with credentials for a pending OTP account
    And the user is on the Account Dashboard
    When the user clicks "<Action>"
    Then a pop-up with 'Continue' and 'Cancel' appears

    Examples:
      | TestCaseId                | Action             |
      | HAP-903 TS-001 TC-001     | Make a Payment     |
      | HAP-903 TS-004 TC-001     | Setup Autopay      |

  Scenario Outline: Customer clicks Continue on payment/autopay pop-up
    Given the Customer Portal is launched
    And the user logs in with credentials for a pending OTP account
    And the user is on the Account Dashboard
    When the user clicks "<Action>"
    And the user clicks 'Continue' on the pop-up
    Then the user is routed to the "<ExpectedPage>" page

    Examples:
      | TestCaseId                | Action             | ExpectedPage      |
      | HAP-903 TS-002 TC-001     | Make a Payment     | Make a Payment    |
      | HAP-903 TS-005 TC-001     | Setup Autopay      | Setup Autopay     |

  Scenario Outline: Customer clicks Cancel on payment/autopay pop-up
    Given the Customer Portal is launched
    And the user logs in with credentials for a pending OTP account
    And the user is on the Account Dashboard
    When the user clicks "<Action>"
    And the user clicks 'Cancel' on the pop-up
    Then the pop-up is dismissed and user remains on Account Dashboard

    Examples:
      | TestCaseId                | Action             |
      | HAP-903 TS-003 TC-001     | Make a Payment     |