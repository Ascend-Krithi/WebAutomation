Feature: Late Fee Message Display

  Scenario Outline: Late fee message display based on payment date
    Given the customer servicing application is launched
    And the user logs in with valid credentials
    And the user completes MFA verification
    And the dashboard is loaded
    And all pop-ups are dismissed if present
    And the user selects the applicable loan account
    And the user clicks Make a Payment
    And the user continues past the scheduled payment popup if present
    And the user opens the payment date picker
    And the user selects the payment date from test data
    Then the late fee message area should <LateFeeMessageExpectation>

    Examples:
      | TestCaseId              | LateFeeMessageExpectation |
      | HAP-700 TS-001 TC-001   | not be displayed         |
      | HAP-700 TS-001 TC-002   | be displayed             |
      | HAP-700 TS-001 TC-003   | not be displayed         |