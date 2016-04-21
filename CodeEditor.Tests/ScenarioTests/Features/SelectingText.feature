Feature: SelectingText


Scenario: Enter one line and select chars in the middle - left to right
	Given Text to enter is 'asdfqwerzxcv'
	When I enter text
		And I select text from '4' '0' to '8' '0'
	Then Selected text should be 'qwer'


Scenario: Enter one line and select chars in the middle - right to left
	Given Text to enter is 'asdfqwerzxcv'
	When I enter text
		And I select text from '8' '0' to '4' '0'
	Then Selected text should be 'qwer'


Scenario: Enter two lines and select text
	Given Text to enter is 'I saw Susie sitting in a shoe shine shop.'
		And Text to enter is newline
		And Text to enter is 'Where she sits she shines, and where she shines she sits.'
	When I enter text
		And I select text from '1' '0' to '10' '1'
	Then Selected text should be ' saw Susie sitting in a shoe shine shop.Where she '


Scenario: Enter two lines where first one is shorter than the second one
	Given Text to enter is 'asdf'
		And Text to enter is newline
		And Text to enter is 'asdf xzcv qwer'
	When I enter text
		And I select text from '4' '0' to '9' '1'
	Then Selected text should be 'asdf xzcv'