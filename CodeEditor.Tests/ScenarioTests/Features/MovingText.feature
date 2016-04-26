Feature: MovingText
	In order to move text
	I place caret on an appropriate position
	And hit one of enter, delete, backspace keys


Scenario: Move text from first to second line
	Given Text to enter is 'asdf'
	When I enter text
		And I move caret to column number '0' in line '0'
		And I hit enter key
	Then I should see '2' lines
		And Shown number of lines in the lines panel should be '2'
		And The '0' line should be equal to ''
		And The '1' line should be equal to 'asdf'
		And I should see no folding


Scenario: Move text from first to second and again to the first line
	Given Text to enter is 'asdf'
	When I enter text
		And I move caret to column number '0' in line '0'
		And I hit enter key
		And I hit backspace key
	Then I should see '1' lines
		And Shown number of lines in the lines panel should be '1'
		And The '0' line should be equal to 'asdf'


Scenario: Folds should not move
	Given Text to enter is '{}'
		And Text to enter is newline
		And Text to enter is '{}'
	When I enter text
	Then I should see '1' folding on position starting at '0' '0' and ending at '1' '0'
		And I should see '2' folding on position starting at '0' '1' and ending at '1' '1'


Scenario: Folds should move
	Given Text to enter is '{}'
		And Text to enter is newline
		And Text to enter is '{}'
	When I enter text
		And I move caret to column number '0' in line '0'
		And I hit enter key
	Then I should see '1' folding on position starting at '0' '1' and ending at '1' '1'
		And I should see '2' folding on position starting at '0' '2' and ending at '1' '2'