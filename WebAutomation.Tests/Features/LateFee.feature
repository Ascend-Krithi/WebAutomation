Feature: Late Fee Message Display for HELOC Payments

  Scenario Outline: Validate late fee message display for HELOC loan payments
    Given the user launches the customer servicing application
    And logs in with valid credentials
    And completes MFA verification
    And navigates to the dashboard
    And dismisses any pop-ups if present
    And selects the applicable loan account
    When the user clicks Make a Payment
    And continues past the scheduled payment popup if it appears
    And opens the payment date picker
    And selects the payment date from test data
    Then the late fee message area should <LateFeeExpectation>

    Examples:
      | TestCaseId                | LateFeeExpectation      |
      | HAP-700 TS-001 TC-001     | not be displayed        |
      | HAP-700 TS-001 TC-002     | be displayed            |
      | HAP-700 TS-001 TC-003     | not be displayed        |