Feature: SetParameter
	In order to set parameter values
	As a user
	I want to set parameter values

# This is just to show how it is possible
# to use SpecFlow with rvtUnit (and Moq).
# It is not a good example for integration testing.
Scenario: Set Parameter
	Given I have a parameter called "Test Parameter"
	When I change its value to 20
	Then the "Test Parameter" value should be 20
