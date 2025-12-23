Feature: HELOC Late Fee Message

  Scenario Outline: No late fee message is displayed for payment date <15 days past due
    Given the user launches the customer servicing application
    And logs in with valid credentials
    And completes MFA verification
    And navigates to the dashboard
    And dismisses any pop-ups if present
    And selects the applicable loan account "<LoanNumber>"
    And clicks Make a Payment
    And continues past the scheduled payment popup if it appears
    And opens the payment date picker
    When the user selects the payment date "<PaymentDate>"
    Then no late fee message is displayed

    Examples:
      | TestCaseId              | LoanNumber | PaymentDate | 
      | HAP-700 TS-001 TC-001   | <LoanNum1> | <Date1>     |

  Scenario Outline: Late fee message is displayed for payment date >15 days past due
    Given the user launches the customer servicing application
    And logs in with valid credentials
    And completes MFA verification
    And navigates to the dashboard
    And dismisses any pop-ups if present
    And selects the applicable loan account "<LoanNumber>"
    And clicks Make a Payment
    And continues past the scheduled payment popup if it appears
    And opens the payment date picker
    When the user selects the payment date "<PaymentDate>"
    Then a late fee message is displayed

    Examples:
      | TestCaseId              | LoanNumber | PaymentDate | 
      | HAP-700 TS-001 TC-002   | <LoanNum2> | <Date2>     |