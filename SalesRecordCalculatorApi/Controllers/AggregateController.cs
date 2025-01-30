using Microsoft.AspNetCore.Mvc;
using SalesRecordCalculator.DomainLogic;

namespace SalesRecordCalculator.Controllers;

/// <summary>
/// Controller for handling aggregate calculations from csv files.
/// </summary>
[Route("/[controller]")]
[ApiController]
public class AggregateController(
    ISalesRecordReader salesRecordReader,
    IAggregateCalculator aggregateCalculator
) : ControllerBase
{
    /// <summary>
    /// Reads the provided csv file of sales records and calculates 
    /// the aggregate values for the records.    
    /// </summary>
    /// <param name="csvFile">CSV File of sales records to calculate aggregate values from</param>
    /// <response code="200">Returns an AggregateResponse object containing the calculated aggregate values</response>
    /// <response code="400">The file failed validation, or information is missing from the request</response>
    [HttpPost("calculatefromcsv")]
    public IActionResult calculateFromCsv(IFormFile csvFile)
    {
        try
        {
            // iterate through each record in the file and add relevant
            // information from the record to the tallys being kept in 
            // the aggregate calculator
            salesRecordReader.ProcessSalesRecords(csvFile, record =>
            {
                aggregateCalculator.AddRecordToTally(record);
            });

            // After all records have been initially processed, calculate
            // the remaining aggregate values and return them.
            var response = aggregateCalculator.CalculateAggregate();
            return Ok(response);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
