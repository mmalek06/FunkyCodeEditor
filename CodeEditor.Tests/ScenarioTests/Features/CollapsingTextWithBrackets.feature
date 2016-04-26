Feature: CollapsingText
	In order to collapse a range of text
	As a user
	I want to click on a folding symbol


Scenario: Create two folds in one line
	Given Text to enter is '{}'
		And Text to enter is '{}'
	When I enter text
	Then I should see '1' lines
		And I should see no folding


Scenario: Enter three opening and three closing brackets
	Given Text to enter is '{'
		And Text to enter is '{'
		And Text to enter is '{'
		And Text to enter is newline
		And Text to enter is newline
		And Text to enter is '}'
		And Text to enter is '}'
		And Text to enter is '}'
	When I enter text
	Then I should see '3' lines
		And Shown number of lines in the lines panel should be '3'
		And I should see no folding


Scenario: Enter two lines with brackets only with fold in first
	Given Text to enter is '{}'
		And Text to enter is newline
		And Text to enter is '{}'
	When I enter text
		And I request folding for position starting at '0' '0' and ending at '1' '0'
	Then I should see '2' lines
		And Shown number of lines in the lines panel should be '2'
		And The '0' line should be equal to '{...}'
		And The '1' line should be equal to '{}'


Scenario: Enter two lines with brackets only
	Given Text to enter is '{}'
		And Text to enter is newline
		And Text to enter is '{}'
	When I enter text
		And I request folding for position starting at '0' '0' and ending at '1' '0'
		And I request unfolding for position starting at '0' '0' and ending at '1' '0'
		And I request folding for position starting at '0' '0' and ending at '1' '0'
		And I request unfolding for position starting at '0' '0' and ending at '1' '0'
	Then I should see '2' lines
		And Shown number of lines in the lines panel should be '2'
		And The '0' line should be equal to '{}'
		And The '1' line should be equal to '{}'


Scenario: Enter five lines and collapse text
	Given Text to enter is 'asdf'
		And Text to enter is newline
		And Text to enter is '{'
		And Text to enter is newline
		And Text to enter is newline
		And Text to enter is '}'
		And Text to enter is newline
		And Text to enter is 'qwer'
	When I enter text
		And I request folding for position starting at '0' '1' and ending at '0' '3'
	Then I should see '3' lines
		And Shown number of lines in the lines panel should be '3'
		And The '0' line should be equal to 'asdf'
		And The '1' line should be equal to '{...}'
		And The '2' line should be equal to 'qwer'


Scenario: Enter four lines with two collapsible sections
	Given Text to enter is 'asdf {}'
		And Text to enter is newline
		And Text to enter is newline
		And Text to enter is 'b'
		And Text to enter is newline
		And Text to enter is '{}'
	When I enter text
		And I request folding for position starting at '5' '0' and ending at '6' '0'
		And I request unfolding for position starting at '5' '0' and ending at '6' '0'
	Then I should see '4' lines
		And Shown number of lines in the lines panel should be '4'
		And The '0' line should be equal to 'asdf {}'
		And The '1' line should be equal to ''
		And The '2' line should be equal to 'b'
		And The '3' line should be equal to '{}'


Scenario: Enter brackets below a line of text
	Given Text to enter is 'susie sits in a shoe shine shop'
		And Text to enter is newline
		And Text to enter is '{'
		And Text to enter is newline
		And Text to enter is newline
		And Text to enter is '}'
	When I enter text
		And I request folding for position starting at '0' '1' and ending at '0' '3'
	Then I should see '2' lines
		And Shown number of lines in the lines panel should be '2'
		And The '0' line should be equal to 'susie sits in a shoe shine shop'
		And The '1' line should be equal to '{...}'


Scenario: Create folding, move closing bracket one line to the bottom
	Given Text to enter is '{'
		And Text to enter is newline
		And Text to enter is newline
		And Text to enter is '}'
	When I enter text
		And I move caret to column number '0' in line '1'
		And I hit enter key
		And I request folding for position starting at '0' '0' and ending at '0' '3'
	Then I should see '1' lines
		And Shown number of lines in the lines panel should be '1'
		And The '0' line should be equal to '{...}'


Scenario: Create folding in first line, move it to the second
	Given Text to enter is '{}'
	When I enter text
		And I move caret to column number '0' in line '0'
		And I hit enter key
	Then I should see '2' lines
		And Shown number of lines in the lines panel should be '2'
		And The '1' line should be equal to '{}'
		And I should see folding on position starting at '0' '1' and ending at '1' '1'