Feature: BreakNonEmptyLine
	In order to break line
	As a user
	I want to move the caret to a place inside a line and hit return


Scenario: Enter one line four chars long  - Green Path
	Given Text to enter is 'as'
	And Text to enter is 'df'
	When I enter text
	And I move caret to column number '2' in line '0'
	And I hit enter key
	Then I should see '2' lines
	And The '0' line should be equal to 'as'
	And The '1' line should be equal to 'df'


Scenario: Enter three lines with varying lengths - Green Path
	Given Text to enter is 'susie sits'
	And Text to enter is newline
	And Text to enter is 'in a shoe shine shop'
	When I enter text
	And I move caret to column number '7' in line '1'
	And I hit enter key
	Then I should see '3' lines
	And The '0' line should be equal to 'susie sits'
	And The '1' line should be equal to 'in a sh'
	And The '2' line should be equal to 'oe shine shop'


Scenario: Enter one line four chars long with cursor at the beginning - Green Path
	Given Text to enter is 'asdf'
	When I enter text
	And I move caret to column number '0' in line '0'
	And I hit enter key
	Then I should see '2' lines
	And The '0' line should be equal to ''
	And The '1' line should be equal to 'asdf'


Scenario: Enter one line four chars long and move the cursor outside text range - Red Path
	Given Text to enter is 'asdf'
	When I enter text
	And I move caret to column number '500' in line '500'
	Then I should see '1' lines
	And The '0' line should be equal to 'asdf'