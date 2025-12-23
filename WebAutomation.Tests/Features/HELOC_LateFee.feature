Feature: HELOC Late Fee Message Display

  As a HELOC customer
  I want to see late fee messages only when my payment date is more than 15 days past due
  So that I am accurately informed about late fees

  Scenario Outline: Late fee message display based on payment date
    Given I launch the customer servicing application
    And I log in as a valid customer
    And I complete MFA verification
    And I am on the dashboard
    And I dismiss all popups if present
    And I select the applicable loan account
    When I click Make a Payment
    And I continue past the scheduled payment popup if it appears
    And I open the payment date picker
    And I select the payment date from test data
    Then the payment date field should display the selected date
    And the late fee message area should <LateFeeExpectation>

    Examples:
      | TestCaseId             | PaymentDate | LateFeeExpectation         |
      | HAP-700 TS-001 TC-001  | 05/01/2024  | not be displayed          |
      | HAP-700 TS-001 TC-002  | 04/10/2024  | be displayed              |
      | HAP-700 TS-001 TC-003  | 04/15/2024  | not be displayed          |