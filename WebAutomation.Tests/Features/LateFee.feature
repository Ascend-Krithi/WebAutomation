Feature: Late Fee Message Display

  Scenario Outline: Late fee message display logic for HELOC payment
    Given the user launches the customer servicing application
    And logs in with valid credentials
    And completes MFA verification
    And the dashboard is loaded
    And all pop-ups are dismissed
    And the user selects the applicable loan account
    When the user clicks Make a Payment
    And handles the scheduled payment popup if present
    And opens the payment date picker
    And selects the payment date "<PaymentDate>"
    Then the late fee message area should display late fee message: <ExpectedLateFee>

    Examples:
      | TestCaseId | LoanNumber | PaymentDate | State | ExpectedLateFee |
      | TC01       | 3616       | 2025-12-20  | TX    | False           |
      | TC02       | 3616       | 2026-01-23  | TX    | True            |
      | TC03       | 3616       | 2026-01-16  | TX    | False           |