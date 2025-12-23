Feature: Make a Payment with No Late Fee

  Scenario Outline: Customer makes a payment for a loan account with payment date less than 15 days past due
    Given the customer servicing application is launched
    And I log in using valid customer credentials
    And I complete MFA verification
    And I am on the dashboard
    And I dismiss any pop-ups if present
    And I select the applicable loan account "<LoanNumber>"
    When I click Make a Payment
    And I continue past the scheduled payment popup if it appears
    And I open the payment date picker
    And I select the payment date "<PaymentDate>"
    Then no late fee message is displayed

    Examples:
      | TestCaseId | LoanNumber | PaymentDate  | State | ExpectedLateFee |
      | TC01       | 3616       | 2025-12-20   | TX    | False           |