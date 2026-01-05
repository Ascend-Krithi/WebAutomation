Feature: Late Fee Message Display

  Scenario Outline: Late fee message display for HELOC loan payment date
    Given the user launches the customer servicing application
    And the user logs in with valid credentials
    And the user completes MFA verification
    And the user is on the dashboard
    And all pop-ups are dismissed if present
    And the user selects the applicable loan account
    When the user clicks Make a Payment
    And the user continues past the scheduled payment popup if it appears
    And the user opens the payment date picker
    And the user selects the payment date from test data
    Then the late fee message area should <LateFeeExpectation>

    Examples:
      | TestCaseId | LateFeeExpectation      |
      | TC01       | not be displayed       |
      | TC02       | be displayed           |
      | TC03       | not be displayed       |