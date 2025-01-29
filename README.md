# Sales Record Calculator
Provides an Asp.net Web Api with an endpoint that will take a posted sales record csv file, parse it and return a response containing aggregate information about the records in the provided file.

## Running Locally:
The Api project can be run locally by typing `dotnet run` in a terminal and then visiting it using the built in swagger documentation exposed for the endpoint. 
Swagger documentation can be found at the following url: [http://localhost:5215/swagger/index.html|http://localhost:5215/swagger/index.html]


## Code Observations:
* I chose to utilize the more traditional patterns of breaking out specific endpoints into controllers as I felt even though you could contain all of the code in the single Program.cs class, breaking it out offered a number of benefits including:
    * Well organized patterns others could follow for where to place code
    * Easier to unit test
    * Prevention of a sprawling Program.cs file should more endpoints get added on later. 
* I also chose to break out the dependency injection logic into it's own class for better organization and less sprawl in the Program.cs file as well.
* Code relevant to the direct execution of the endpoint was left in the controller action, and code relevant to the domain specific operations was broken out into classes specific to the different functions being performed. 
    * SalesRecordCsvReader is responsible for taking an IFormFile, reading and parsing it and then providing a plug in point where additional processing for each individual record can occur.
    * AggegateCalculator is responsible for updating aggregate tallys as records are processed, and then calculating the final aggregate values once all records have been processed. 
* Having this logic separate from the controller action allows for more discrete unit testing of each individual component in isolation.



## Considerations:
Items for consideration in order to harden this for a production environment, purposefully left out of this api to more appropriately fit within the timeframe suggested.
* Authentication and Authorization and api administration
* Observability (logging, tracing, metrics)
* Ability to handle localized inputs (language support, file encodings)
* Global exception handling to mask errors and sensitive details from external access. 
* Support for deployment to a cloud environment
* Infrastructure for rate-limiting, load balancing, and scaling



## Impinj Coding Assesment:

```
TIME:            2-3 hours
LANGUAGES:       C# preferred
FRAMEWORKS:      NET or NET CORE preferred,
TEST FRAMEWORKS: MS BUILD/NUNIT/XUNIT preferred
TYPE:            Web Api
FILES:           In repository
TESTS:           preferred
SUBMIT:          ZipFile, Github Repository Link
```

## Overview:
This exercise is to asses a candidates ability to design and implement a solution to a problem in a given period of time.

We want to see a demonstration of your thought process, skills and development experiences.

This solution should be able to run locally.

_Be prepared to explain your design decisions and architecture!_


## Problem:
Create a web API that can parse a sales record file into an object. The API should return an object with the following fields:
* The median Unit Cost
* The most common Region
* The first and last Order Date and the days between them
* The total revenue


## Included:
* Sample input at `Input\SalesRecords.csv`
* A git ignore file for a C# project
* A git attributes file

<br></br>

_Hint: a web api skeleton can be created with `dotnet new webapi`_
