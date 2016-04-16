Feature: CodeCollapsingWithBrackets
	In order to collapse code with brackets
	As a user
	I want to click on a folding symbol


Scenario: Collapse in one line - Green Path
	Given Text to enter is '{'
	And Text to enter is '}'
	When I enter text
	And I click folding symbol in line '0'
	Then The '0' line should be equal to '{...}'