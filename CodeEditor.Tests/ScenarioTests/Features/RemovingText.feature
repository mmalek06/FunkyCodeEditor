Feature: RemovingText
	In order to remove text
	As a user
	I want to press backspace or delete keys with or without text selected


Scenario: Delete from empty line - Green Path
	Given Text to enter is newline
	When I enter text
	And I move caret to column number '0' in line '0'
	And I hit delete key
	Then I should see '1' lines
	And Shown number of lines in the lines panel should be '1'
	And The '0' line should be equal to ''
	And Cursor should be at '0' '0'
	

Scenario: Delete last line when cursor is at the second one - Green Path
	Given Text to enter is newline
	And Text to enter is newline
	When I enter text
	And I move caret to column number '0' in line '1'
	And I hit delete key
	Then I should see '2' lines
	And Shown number of lines in the lines panel should be '2'
	And The '0' line should be equal to ''
	And The '1' line should be equal to ''
	And Cursor should be at '0' '1'


Scenario: Backspace first line when cursor is at the second one - Green Path
	Given Text to enter is newline
	And Text to enter is newline
	When I enter text
	And I move caret to column number '0' in line '1'
	And I hit backspace key
	Then I should see '2' lines
	And Shown number of lines in the lines panel should be '2'
	And The '0' line should be equal to ''
	And The '1' line should be equal to ''
	And Cursor should be at '0' '0'
