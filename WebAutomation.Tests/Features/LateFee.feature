Feature: Late Fee Payment

  Scenario Outline: Customer makes a payment and late fee message is displayed
    Given the customer logs in
    And completes identity verification
    And enters OTP and verifies
    And navigates to the dashboard
    And closes any popups if present
    When the customer selects loan number <LoanNumber>
    And clicks on Make a Payment
    And selects payment date <PaymentDate>
    Then the late fee message popup should be <POP UP Message>

    Examples:
      | TestCaseId | LoanNumber | PaymentDate | POP UP Message |
      | TC01       | 3616       | 2025-12-20  | True           |
      | TC02       | 3616       | 2026-01-23  | True           |
      | TC03       | 3616       | 2026-01-16  | True           |
      | TC04       | 3616       | 2026-01-16  | True           |
      | TC05       | 3616       | 2026-01-16  | True           |
      | TC06       | 3616       | 2026-01-16  | True           |
      | TC07       | 3616       | 2026-01-16  | True           |
      | TC08       | 3616       | 2026-01-16  | True           |
      | TC09       | 3616       | 2026-01-16  | True           |
      | TC10       | 3616       | 2026-01-16  | False          |