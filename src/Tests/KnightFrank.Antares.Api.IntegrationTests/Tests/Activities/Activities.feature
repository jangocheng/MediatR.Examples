﻿Feature: Activities

@Activity
Scenario Outline: Retrieve error messages for improper data
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets <activityTypeCode> for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode       | enumTypeItemCode |
			| OwnershipType      | Freeholder       |
			| <activityStatusId> | PreAppraisal     |
			| Division           | Residential      |
		And User sets attributes for property in database
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 1           | 3           | 1             | 3             | 2            | 3            | 1000    | 3000    | 500         | 4000        | 1                   | 3                   |
		And Property characteristics are set for given property type
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
	When User creates activity for given <propertyId> property id using api 
	Then User should get <statusCode> http status code

	Examples:
	| propertyId                           | activityStatusId                     | statusCode | activityTypeCode |
	| 00000000-0000-0000-0000-000000000002 | ActivityStatus                       | BadRequest | Freehold Sale    |
	| latest                               | 00000000-0000-0000-0000-000000000001 | BadRequest | Freehold Sale    |
	| latest                               | ActivityStatus                       | BadRequest | Assignment       |
	| latest                               | ActivityStatus                       | BadRequest | invalid          |

@Activity
Scenario: Create Activity for an existing property
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode   | enumTypeItemCode |
			| OwnershipType  | Freeholder       |
			| ActivityStatus | PreAppraisal     |
			| Division       | Residential      |
		And User sets attributes for property in database
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 1           | 3           | 1             | 3             | 2            | 3            | 1000    | 3000    | 500         | 4000        | 1                   | 3                   |
		And Property characteristics are set for given property type
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And User creates contacts in database with following data
			| FirstName | Surname | Title |
			| Michael   | Angel   | cheef |
			| Michael   | Angel   | cook  |
		And Ownership exists in database
			| PurchaseDate | SellDate   | BuyPrice | SellPrice |
			| 01-05-2011   | 01-04-2013 | 1000000  |           |
			| 01-05-2014   |            | 1000000  |           |
	When User creates activity for given latest property id using api
	Then User should get OK http status code
		And Created Activity is saved in database

@Activity
Scenario Outline: Get Activity by incorrect activity id
	When User gets activity with <activityId> id
	Then User should get <expectedStatusCode> http status code

	Examples:
	| activityId                           | expectedStatusCode |
	| a                                    | BadRequest         |
	| 00000000-0000-0000-0000-000000000001 | NotFound           |

@Activity
Scenario: Get Activity by correct activity id
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode   | enumTypeItemCode |
			| ActivityStatus | PreAppraisal     |
			| Division       | Residential      |
		And Property characteristics are set for given property type
		And User sets attributes for property in database
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 1           | 3           | 1             | 3             | 2            | 3            | 1000    | 3000    | 500         | 4000        | 1                   | 3                   |
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
	When User gets activity with latest id
	Then User should get OK http status code
		And Retrieved activity should be same as in database

@Activity
Scenario: Record and update residential sale valuation
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode   | enumTypeItemCode |
			| ActivityStatus | PreAppraisal     |
			| Division       | Residential      |
		And User sets attributes for property in database
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 1           | 3           | 1             | 3             | 2            | 3            | 1000    | 3000    | 500         | 4000        | 1                   | 3                   |
		And Property characteristics are set for given property type
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
	When User updates activity latest id and latest status with following sale valuation
		| MarketAppraisalPrice | RecommendedPrice | VendorEstimatedPrice |
		| 1                    | 2                | 3                    |
	Then User should get OK http status code
		And Retrieved activity should be same as in database

@Activity
Scenario Outline: Record and update residential sale valuation for improper data
	Given User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode   | enumTypeItemCode |
			| Division       | Residential      |
			| ActivityStatus | PreAppraisal     |
		And User sets attributes for property in database
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions | MinBathrooms | MaxBathrooms | MinArea | MaxArea | MinLandArea | MaxLandArea | MinCarParkingSpaces | MaxCarParkingSpaces |
			| 1           | 3           | 1             | 3             | 2            | 3            | 1000    | 3000    | 500         | 4000        | 1                   | 3                   |
		And Property characteristics are set for given property type
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and PreAppraisal activity status exists in database
	When User updates activity <activityId> id and <activityStatusID> status with following sale valuation
		| MarketAppraisalPrice   | RecommendedPrice | VendorEstimatedPrice |
		| <marketAppraisalPrice> | 2                | 3                    |
	Then User should get <statusCode> http status code

	Examples:
	| activityId                           | activityStatusID                     | marketAppraisalPrice | statusCode |
	| 00000000-0000-0000-0000-000000000002 | latest                               | 1                    | BadRequest |
	| latest                               | 00000000-0000-0000-0000-000000000001 | 2                    | BadRequest |

@Activity
Scenario: Get all activities
	Given All activities have been deleted from database
		And User gets GB address form for Property and country details
		And User gets House for PropertyType
		And User gets Freehold Sale for ActivityType
		And User gets EnumTypeItemId and EnumTypeItem code
			| enumTypeCode   | enumTypeItemCode |
			| Division       | Residential      |
			| ActivityStatus | NotSelling       |
		And User sets attributes for property in database
			| MinBedrooms | MaxBedrooms | MinReceptions | MaxReceptions |
			| 1           | 3           | 1             | 3             |
		And Property characteristics are set for given property type
		And Property with Address and Residential division is in database
        	| PropertyName | PropertyNumber | Line1           | Line2              | Line3      | Postcode | City   | County         |
        	| abc          | 1              | Beautifull Flat | Lewis Cubit Square | King Cross | N1C      | London | Greater London |
		And Activity for latest property and NotSelling activity status exists in database
	When User gets activities
	Then User should get OK http status code
		And Retrieved activities should be the same as in database
			| PropertyName | PropertyNumber | Line2              |
			| abc          | 1              | Lewis Cubit Square |