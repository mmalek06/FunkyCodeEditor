Feature: RemovingText
	In order to remove text
	As a user
	I want to press backspace or delete keys with or without text selected


Scenario: Enter one character and press backspace
	Given Text to enter is 'a'
	When I enter text
		And I hit backspace key
	Then Cursor should be at '0' '0'


Scenario: Enter single empty line and press backspace
	Given Text to enter is newline
	When I enter text
		And I hit backspace key
	Then Cursor should be at '0' '0'
		And I should see '1' lines
		And The '0' line should be equal to ''
		And Shown number of lines in the lines panel should be '1'


Scenario: Enter three empty lines and press delete
	Given Text to enter is newline
		And Text to enter is newline
	When I enter text
		And I hit delete key
	Then Cursor should be at '0' '2'
		And I should see '3' lines
		And Shown number of lines in the lines panel should be '3'


Scenario: Delete from empty line
	Given Text to enter is newline
	When I enter text
		And I move caret to column number '0' in line '0'
		And I hit delete key
	Then I should see '1' lines
		And Shown number of lines in the lines panel should be '1'
		And The '0' line should be equal to ''
		And Cursor should be at '0' '0'
	

Scenario: Delete last line when the cursor is at the second one
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


Scenario: Delete last line that is not empty and when the cursor is at the second one
	Given Text to enter is newline
		And Text to enter is newline
		And Text to enter is 'asdf'
	When I enter text
		And I move caret to column number '0' in line '1'
		And I hit delete key
	Then I should see '2' lines
		And Shown number of lines in the lines panel should be '2'
		And The '0' line should be equal to ''
		And The '1' line should be equal to 'asdf'


Scenario: Two non empty lines entered, delete pressed at the end of first one
	Given Text to enter is 'asdf'
		And Text to enter is newline
		And Text to enter is 'qwer'
	When I enter text
		And I move caret to column number '4' in line '0'
		And I hit delete key
	Then I should see '1' lines
		And Shown number of lines in the lines panel should be '1'
		And The '0' line should be equal to 'asdfqwer'
		And Cursor should be at '4' '0'


Scenario: Backspace first line when the cursor is at the second one
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


Scenario: Two non empty lines entered, backspace pressed at the beginning of second one
	Given Text to enter is 'asdf'
		And Text to enter is newline
		And Text to enter is 'qwer'
	When I enter text
		And I move caret to column number '0' in line '1'
		And I hit backspace key
	Then I should see '1' lines
		And Shown number of lines in the lines panel should be '1'
		And The '0' line should be equal to 'asdfqwer'
		And Cursor should be at '4' '0'


Scenario: Four lines entered, delete pressed at the end of first
	Given Text to enter is 'asdf'
		And Text to enter is newline
		And Text to enter is newline
		And Text to enter is 'qwer'
		And Text to enter is newline
		And Text to enter is 'xzcv'
	When I enter text
		And I move caret to column number '4' in line '0'
		And I hit delete key
	Then I should see '3' lines
		And Shown number of lines in the lines panel should be '3'
		And The '0' line should be equal to 'asdf'
		And The '1' line should be equal to 'qwer'
		And The '2' line should be equal to 'xzcv'
		And Cursor should be at '4' '0'


Scenario: Four lines entered, delete pressed twice at the end of first
	Given Text to enter is 'asdf'
		And Text to enter is newline
		And Text to enter is newline
		And Text to enter is 'qwer'
		And Text to enter is newline
		And Text to enter is 'xzcv'
	When I enter text
		And I move caret to column number '4' in line '0'
		And I hit delete key
		And I hit delete key
	Then I should see '2' lines
		And Shown number of lines in the lines panel should be '2'
		And The '0' line should be equal to 'asdfqwer'
		And The '1' line should be equal to 'xzcv'


Scenario: Remove from collapsed line with brackets only
	Given Text to enter is '{}'
	When I enter text
		And I request folding for position starting at '0' '0'
		And I move caret to column number '5' in line '0'
		And I hit backspace key
	Then I should see '1' lines
		And Shown number of lines in the lines panel should be '1'
		And The '0' line should be equal to ''
		And Cursor should be at '0' '0'
		And I should see no folding


Scenario: Remove from collapsed line with brackets and text
	Given Text to enter is 'asdf '
		And Text to enter is '{}'
		And Text to enter is ' qwer'
	When I enter text
		And I request folding for position starting at '5' '0'
		And I move caret to column number '10' in line '0'
		And I hit backspace key
	Then I should see '1' lines
		And Shown number of lines in the lines panel should be '1'
		And The '0' line should be equal to 'asdf  qwer'
		And Cursor should be at '5' '0'
		And I should see no folding


Scenario: Press delete after collapse
	Given Text to enter is '{'
		And Text to enter is newline
		And Text to enter is newline
		And Text to enter is '}'
	When I enter text
		And I request folding for position starting at '0' '0'
		And I move caret to column number '5' in line '0'
		And I hit delete key
	Then I should see folding on position starting at '0' '0' and ending at '0' '2'
		And Shown number of lines in the lines panel should be '1'
		And The '0' line should be equal to '{...}'