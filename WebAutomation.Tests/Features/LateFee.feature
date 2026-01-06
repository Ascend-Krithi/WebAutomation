Feature: Late Fee Message Validation

  Scenario Outline: Validate late fee message based on payment date
    Given the user launches the customer servicing application
    And logs in using valid customer credentials
    And completes MFA verification
    And navigates to the dashboard
    And dismisses any pop-ups if present
    And selects the applicable loan account
    And clicks Make a Payment
    And continues past any scheduled payment popup if present
    And opens the payment date picker
    And selects the payment date from test data
    Then the late fee message area should <LateFeeExpectation>

    Examples:
      | TestCaseId | LoanNumber | PaymentDate  | State | ExpectedLateFee | LateFeeExpectation         |
      | TC01       | 8409       | 2026-01-06   | TX    | False           | not be displayed          |
      | TC02       | 8409       | 2026-01-23   | TX    | True            | be displayed              |
      | TC03       | 8409       | 2026-01-16   | TX    | False           | not be displayed          |