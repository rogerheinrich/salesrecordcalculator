using Microsoft.AspNetCore.Mvc;
using SalesRecordCalculator.DomainLogic;

namespace SalesRecordCalculator.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AggregateController(
        ISalesRecordReader salesRecordReader,
        IAggregateCalculator aggregateCalculator
    ) : ControllerBase
    {
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
}
