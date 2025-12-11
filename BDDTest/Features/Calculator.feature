Feature: Blood pressure classification
  The calculator should classify blood pressure correctly
  and validate systolic must be greater than diastolic

  Scenario: Ideal blood pressure
    Given a systolic value of 110
    And a diastolic value of 70
    When I check the category
    Then the category should be "Ideal Blood Pressure"

  Scenario: High due to systolic
    Given a systolic value of 150
    And a diastolic value of 85
    When I check the category
    Then the category should be "High Blood Pressure"

  Scenario: High due to diastolic
    Given a systolic value of 120
    And a diastolic value of 95
    When I check the category
    Then the category should be "High Blood Pressure"

  Scenario: Pre-high BP
    Given a systolic value of 135
    And a diastolic value of 82
    When I check the category
    Then the category should be "Pre-High Blood Pressure"

  Scenario: Low blood pressure
    Given a systolic value of 85
    And a diastolic value of 55
    When I check the category
    Then the category should be "Low Blood Pressure"

  Scenario: Invalid systolic lower than diastolic
    Given a systolic value of 80
    And a diastolic value of 90
    When I validate the reading
    Then validation should fail
    And it should contain "Systolic value must be greater than Diastolic value"

  Scenario: Advice message for ideal blood pressure
    Given a systolic value of 110
    And a diastolic value of 70
    When I check the category
    Then the category should be "Ideal Blood Pressure"
    And the advice message should be "Your blood pressure is ideal. Maintain a healthy lifestyle!"