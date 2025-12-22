Feature: Late Fee Message Validation

  Scenario Outline: Validate late fee message is not displayed for payment date < 15 days past due
    Given the user launches the customer servicing application
    When the user logs in with valid credentials
    And completes MFA verification
    And navigates to the dashboard
    And dismisses any pop-ups if present
    And selects the applicable loan account
    And clicks Make a Payment
    And continues past scheduled payment popup if present
    And opens the payment date picker
    And selects the payment date from test data
    Then no late fee message is displayed

    Examples:
      | TestCaseId                | ExpectedLateFee |
      | HAP-700 TS-001 TC-001     | False           |