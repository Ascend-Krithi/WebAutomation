Feature: Late Fee Message Display

  Scenario Outline: Late fee message display based on payment date
    Given the user launches the customer servicing application
    And logs in with valid customer credentials
    And completes MFA verification
    And navigates to the dashboard
    And dismisses any pop-ups if present
    And selects the applicable loan account
    And clicks Make a Payment
    And continues past the scheduled payment popup if present
    And opens the payment date picker
    And selects the payment date from test data
    When the user observes the late-fee message area
    Then the late fee message display should be "<LateFeeMessageExpected>"

    Examples:
      | TestCaseId              | LateFeeMessageExpected |
      | HAP-700 TS-001 TC-001   | NotDisplayed          |
      | HAP-700 TS-001 TC-002   | Displayed             |
      | HAP-700 TS-001 TC-003   | NotDisplayed          |