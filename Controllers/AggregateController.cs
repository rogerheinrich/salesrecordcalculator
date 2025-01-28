using Microsoft.AspNetCore.Mvc;

namespace SalesRecordCalculator.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AggregateController : ControllerBase
    {
        [HttpPost("calculatefromcsv")]
        public IActionResult calculateFromCsv(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            using (var reader = new StreamReader(csvFile.OpenReadStream()))
            {
                var currentRecord = reader.ReadLine();
                //todo: iterate through records and calculate the aggregates
            }
            return Ok("File uploaded successfully.");
        }
    }
}
