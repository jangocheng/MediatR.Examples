﻿Feature: Viewings

@Viewings
Scenario: Create viewing
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode   | enumTypeItemCode |
			| ActivityStatus | PreAppraisal     |
			| Division       | Residential      |
		And Property characteristics are set for given property type
		And User sets attributes for property in database
			| MinBedrooms | MaxBedrooms |
			| 1           | 3           |
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets negotiator id from database
		And User gets GB address form for Requirement and country details
		And User creates contacts in database with following data 
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And User sets locations details for the requirement
			| Postcode | City   | Line2   |
			| 1234     | London | Big Ben |
		And User creates following requirement in database
 			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms |
 			| 1000000  | 4000000  | 1           | 5           |
	When User creates viewing using api
	Then User should get OK http status code
		And Viewing details should be the same as already added

@Viewings
Scenario Outline: Create viewing with invalid data
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode   | enumTypeItemCode |
			| ActivityStatus | PreAppraisal     |
			| Division       | Residential      |
		And Property characteristics are set for given property type
		And User sets attributes for property in database
			| MinBedrooms | MaxBedrooms |
			| 1           | 3           |
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
		And User gets negotiator id from database
		And User gets GB address form for Requirement and country details
		And User creates contacts in database with following data 
			| FirstName | Surname | Title  |
			| Tomasz    | Bien    | Mister |
		And User sets locations details for the requirement
			| Postcode | City   | Line2   |
			| 1234     | London | Big Ben |
		And User creates following requirement in database
 			| MinPrice | MaxPrice | MinBedrooms | MaxBedrooms |
 			| 1000000  | 4000000  | 1           | 5           |
	When User creates viewing with invalid <data> using api
	Then User should get BadRequest http status code

	Examples: 
	| data        |
	| requirement |
	| activity    |
	| contact     |
