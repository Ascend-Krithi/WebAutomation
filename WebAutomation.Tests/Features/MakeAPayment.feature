Feature: Make a Payment

  Scenario Outline: No late fee message is displayed for payment date <15 days past due
    Given the user launches the customer servicing application
    And logs in with valid customer credentials
    And completes MFA verification
    And navigates to the dashboard
    And dismisses any pop-ups if present
    And selects the applicable loan account
    When the user clicks Make a Payment
    And continues past the scheduled payment popup if it appears
    And opens the payment date picker
    And selects the payment date "<PaymentDate>" from test data
    Then no late fee message is displayed

    Examples:
      | TestCaseId              | LoanNumber | PaymentDate  |
      | HAP-700 TS-001 TC-001   | <LoanNum>  | <PayDate>    |