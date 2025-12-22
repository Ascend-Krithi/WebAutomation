Feature: Late Fee Message Validation

  Scenario Outline: Validate late fee message is not displayed for payment date less than 15 days past due
    Given the user launches the customer servicing application
    When the user logs in with valid credentials
    And the user completes MFA verification
    And the user navigates to the dashboard
    And the user dismisses any pop-ups if present
    And the user selects the applicable loan account
    And the user clicks Make a Payment
    And the user continues past any scheduled payment popup if present
    And the user opens the payment date picker
    And the user selects the payment date from test data
    Then the late-fee message area should not be displayed

    Examples:
      | TestCaseId | LoanNumber | PaymentDate  | ExpectedLateFee |
      | TC01       | 3616       | 2025-12-20   | False           |