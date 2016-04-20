Feature: SelectingText


Scenario: Enter two lines and select text
	Given Text to enter is 'I saw Susie sitting in a shoe shine shop.'
		And Text to enter is newline
		And Text to enter is 'Where she sits she shines, and where she shines she sits.'
	When I enter text
		And I select text from '1' '0' to '10' '1'
	Then Selected text should be ' saw Susie sitting in a shoe shine shop.Where she '
