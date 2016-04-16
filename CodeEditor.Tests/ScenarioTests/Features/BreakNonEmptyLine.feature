Feature: BreakNonEmptyLine
	In order to break line
	As a user
	I want to move the caret to a place inside a line and hit return

@mytag
Scenario: Break non empty line - Green Path
	Given Text to enter is 'as'
	And Text to enter is 'df'
	When I enter text
	And I move caret to column number '3' in line '1'
	And I hit enter key
	Then I should see '2' lines
	And The '0' line should be equal to 'as'
	And The '1' line should be equal to 'df'
