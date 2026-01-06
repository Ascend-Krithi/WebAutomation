Feature: Late Fee Message Display

  Scenario Outline: Late fee message display based on payment date
    Given the user launches the customer servicing application
    And logs in with valid credentials
    And completes MFA verification
    And navigates to the dashboard
    And dismisses any pop-ups if present
    And selects the applicable loan account
    When the user clicks Make a Payment
    And handles the scheduled payment popup if present
    And opens the payment date picker
    And selects the payment date from test data
    Then the late fee message area should <LateFeeExpectation>

    Examples:
      | TestCaseId | ExpectedLateFee |
      | TC01       | False           |
      | TC02       | True            |
      | TC03       | False           |