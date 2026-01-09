Feature: Late Fee Message Display

  Scenario Outline: Late fee message display based on payment date
    Given the customer servicing application is launched
    And the user logs in with valid credentials
    And the user completes MFA verification
    And the user is on the dashboard
    And all pop-ups are dismissed
    And the user selects the applicable loan account
    When the user clicks Make a Payment
    And the user continues past any scheduled payment popup
    And the user opens the payment date picker
    And the user selects the payment date from test data
    Then the late-fee message area should display the expected late fee message

    Examples:
      | TestCaseId | LoanNumber | PaymentDate  | ExpectedLateFee |
      | TC01       | 3616       | 2025-12-20   | False           |
      | TC02       | 3616       | 2026-01-23   | True            |
      | TC03       | 3616       | 2026-01-16   | False           |