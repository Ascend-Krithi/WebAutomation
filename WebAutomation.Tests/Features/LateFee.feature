Feature: Late Fee Message Display

  Scenario Outline: Validate late fee message display for HELOC loan payment date
    Given the user launches the customer servicing application
    And the user logs in with valid credentials
    And the user completes MFA verification
    And the user is on the dashboard
    And the user dismisses any pop-ups if present
    And the user selects the applicable loan account
    When the user clicks Make a Payment
    And the user continues past the scheduled payment popup if it appears
    And the user opens the payment date picker
    And the user selects the payment date from test data
    Then the late fee message area should <LateFeeExpectation>

    Examples:
      | TestCaseId | Scenario                                 | LoanNumber | PaymentDate | State | ExpectedLateFee | LateFeeExpectation         |
      | TC01       | < 15 days – no late fee message          | 3616       | 2025-12-20  | TX    | False           | not be displayed          |
      | TC02       | > 15 days – late fee message should show | 3616       | 2026-01-23  | TX    | True            | be displayed              |
      | TC03       | = 15 days – no late fee message          | 3616       | 2026-01-16  | TX    | False           | not be displayed          |