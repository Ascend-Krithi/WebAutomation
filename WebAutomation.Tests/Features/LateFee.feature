Feature: Late Fee Message Display

  Scenario Outline: Late fee message display logic for HELOC loan
    Given the user is on the login page
    When the user logs in with valid credentials for "<TestCaseId>"
    And the user navigates to the loan dashboard
    And the user selects loan account "<LoanNumber>"
    And the user enters payment date "<PaymentDate>"
    Then the late fee message is <ExpectedLateFee> for "<TestCaseId>"

    Examples:
      | TestCaseId | LoanNumber | PaymentDate  | State | ExpectedLateFee |
      | TC01       | 3616       | 2025-12-20   | TX    | False           |
      | TC02       | 3616       | 2026-01-23   | TX    | True            |
      | TC03       | 3616       | 2026-01-16   | TX    | False           |