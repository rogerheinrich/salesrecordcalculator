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
                var regionCounts = new Dictionary<string, int>();
                salesRecordReader.ProcessSalesRecords(csvFile, record =>
                {
                    aggregateCalculator.AddRecordToTally(record);
                });

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
