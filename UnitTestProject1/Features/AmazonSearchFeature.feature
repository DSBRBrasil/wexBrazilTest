Feature: AmazonSearchFeature	 

@mytag1
Scenario: Search result of the books in amazon site
	Given I navigate to www.amazon.com
	When I select the option books in the dropdown next to the search text input criteria
	| keyword |
	| Books    |
	Then I search for Test automation
	| keyword           |
	| test automation |
	And I select the cheapset book of the page without using any sorting method available
	When I reach the detailed book page, I check if the name in the header is the same name of the book that I selected previuosly
	 
	 
