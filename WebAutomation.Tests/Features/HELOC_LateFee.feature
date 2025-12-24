Feature: HELOC Late Fee Message Validation

  Scenario Outline: Validate late fee message for HELOC loan payment date
    Given the user launches the customer servicing application
    And logs in with valid credentials
    And completes MFA verification
    And the dashboard is displayed
    And all pop-ups are dismissed if present
    And the user selects the applicable loan account
    When the user clicks Make a Payment
    And continues past any scheduled payment popup if present
    And opens the payment date picker
    And selects the payment date from test data
    Then the late fee message area should <LateFeeMessageExpectation>

    Examples:
      | TestCaseId             | LateFeeMessageExpectation |
      | HAP-700 TS-001 TC-001  | not be displayed         |
      | HAP-700 TS-001 TC-002  | be displayed             |
      | HAP-700 TS-001 TC-003  | not be displayed         |