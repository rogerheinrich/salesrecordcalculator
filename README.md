# Sales Record Calculator
Provides an Asp.net Web Api with an endpoint that will take a posted sales record csv file, parse it and return a response containing aggregate information about the records in the provided file.

## Running Locally:
* The Api project can be run locally by doing the following:
    1. Navigate a terminal to the `SalesRecordCalculatorApi` folder 
    1. Type `dotnet run` in the terminal
    1. Visit the host address in your browser using the built in swagger documentation exposed for the endpoint.     
        * Swagger documentation can be found at the following url: [http://localhost:5215/swagger/index.html](http://localhost:5215/swagger/index.html)
            * Assumes your host address gets set to 5215. If not adjust to match the port your instance is running on.
* You can also step-through debug by using Visual Studio or VS Code's debugging capabilities

## Running Tests
* Tests can be run by typing `dotnet test` in a terminal and observing the test output. Or by running them in the test explorers available in Visual Studio or VS Code.


## Code Observations:
* I chose to utilize the more traditional patterns of breaking out specific endpoints into controllers as I felt even though you could contain all of the code in the single Program.cs class, breaking it out offered a number of benefits including:
    * Well organized patterns others could follow for where to place code
    * Potentially easier to unit test
    * Prevention of a sprawling Program.cs file should more endpoints get added on later. 
* I also chose to break out the dependency injection logic into it's own class for better organization and less sprawl in the Program.cs file as well.
* Code relevant to the direct execution of the endpoint was left in the controller action, and code relevant to the domain specific operations were broken out into classes specific to the different functions being performed. 
    * SalesRecordCsvReader is responsible for taking an IFormFile, reading and parsing it and then providing a plug in point where additional processing for each individual record can occur.
    * AggegateCalculator is responsible for updating aggregate tallys as records are processed, and then calculating the final aggregate values once all records have been processed. 
* Having this logic separate from the controller action allows for more discrete unit testing of each individual component in isolation.
* I chose to implement a version of the quickSelect method that utilizes a random partition selection screen to help minimize the worst case time complexity issue that QuickSelect can suffer from in certain situations. (eg poor partition selection that minimizes set reduction from iteration to iteration)
    * I am somewhat on the fence on the choice to use quickSelect. I believe that linq already uses an implementation of quicksort for sorting collections which means that it already has a fair amount of the performance gains we may see with quickSelection. Without knowing more about the requirements that surround this api, I am reticent to jump in with a custom sorting algorithm. It may be that we see enough performance with built-in linq structures to go that route. 
        * Generally I would recommend starting with the built-in linq structures, do performance testing with expected data loads, and assess additional performance optimizations from there. That way the maintenance overhead of having to support and understand a custom built implementation of a sorting algorithm can be avoided. I ended up doing the quickSelect implementation here to showcase my understanding of it and I thought that it may be more interesting for conversation and assessment than just utilizing linq's sort capabilities.
    
    * In regards to the quickSelect method, I constructed it as an iterative approach as opposed to a recursive approach because I was concerned the dataset size may end up causing issues with stack overflows. Worst case scenario in terms of complexity could lead to an O(n) situation in terms of recursion calls. So with a dataset of 100,000 records we could potentially run into a situation where we need to make 100,000 recursive calls, which I believe "could" be in range of how much dotnet may support (online information seems to indicate somewhere between 10,000 to 100,000), but I figure it was better to be safe than sorry. 
    
    * I chose to utilize a random partition selection scheme, as opposed to a median of medians approach. The randomized approach does not neccessarily alleviate all possible scenarios where the worst case complexity could occur, but it does reduce it significantly. It is also more performant that the median of medians approach. I could be convinced to change it if it turns out we absolutely needed to avoid the worst case complexity scenario, in which case the median of medians approach would give us that certainty at the cost of some of the performance.

* For the storing of information from the SalesRecord csv:
    * Used decimals for the representation of the financial data as I imagined that the precision offered for the financial calculations may be preferential to the performance benefits of using floats or doubles.

    * Based off values in sample file for order ids I decided big integer may be best appropriate as it can handle arbitrarily large values. Order Ids were within an order of magnitude of integer max value

    * Left most of the fields optional and nullable (where possible) since they aren't actually required in any calculations. Had we been storing this data more long term I may have chosen to make them required as data cleanliness in long term storage could be more important, but since we get new data for each request, flexibility seemed a wiser choice to allow more data to be easily processed.


## Considerations:
Items for consideration in order to harden this for a production environment, purposefully left out of this api to more appropriately fit within the timeframe suggested.
* Authentication and Authorization
* Https and SSL encryption
* Api administration
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
