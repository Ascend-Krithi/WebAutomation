Feature: Late Fee Message Display

  Scenario Outline: Late fee message display based on payment date
    Given I launch the customer servicing application
    And I log in using valid customer credentials
    And I complete MFA verification
    And I navigate to the dashboard
    And I dismiss any pop-ups if present
    And I select the applicable loan account from test data "<TestCaseId>"
    And I click Make a Payment
    And I continue past the scheduled payment popup if it appears
    And I open the payment date picker
    And I select the payment date from test data "<TestCaseId>"
    Then the late fee message area should be "<ExpectedLateFeeMessage>"

    Examples:
      | TestCaseId              | PaymentDate | ExpectedLateFeeMessage |
      | HAP-700 TS-001 TC-001   | <from data> | NotDisplayed          |
      | HAP-700 TS-001 TC-002   | <from data> | Displayed             |
      | HAP-700 TS-001 TC-003   | <from data> | NotDisplayed          |