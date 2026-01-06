Feature: Late Fee Message Display

  Scenario Outline: Late fee message display based on payment date
    Given the user launches the customer servicing application
    And logs in using valid customer credentials
    And completes MFA verification
    And navigates to the dashboard
    And closes any pop-ups if present
    And selects the applicable loan account
    And clicks Make a Payment
    And continues past any scheduled payment popup if present
    And opens the payment date picker
    And selects the payment date from test data
    Then the late fee message area should display the expected result

    Examples:
      | TestCaseId | LoanNumber | PaymentDate  | State | ExpectedLateFee |
      | HAP-700 TS-001 TC-001 | 8409 | 2026-01-06 | TX | False |
      | HAP-700 TS-001 TC-002 | 8409 | 2026-01-23 | TX | True  |
      | HAP-700 TS-001 TC-003 | 8409 | 2026-01-16 | TX | False |