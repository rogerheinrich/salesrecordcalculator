using System.Globalization;
using CsvHelper;
using SalesRecordCalculator.DomainLogic.Models;

namespace SalesRecordCalculator.DomainLogic;

public interface ISalesRecordReader
{
    void ProcessSalesRecords(IFormFile csvFile, Action<SalesRecord> processRecord);
}

/// <summary>
/// Reads a csv file and processes each record in the file.
/// </summary>
public class SalesRecordCsvReader : ISalesRecordReader
{

    /// <summary>
    /// Takes an IFormFile from an httprequest, reads the file as a csv file
    /// and provides a delegate for processing each record in the file.
    /// </summary>
    /// <param name="csvFile">File to process</param>
    /// <param name="processRecord">Delegate to call for processing each SalesRecord row in the file</param>
    /// <exception cref="ValidationException">Thrown when the file does not pass validation and cannot be processed.</exception>
    public void ProcessSalesRecords(IFormFile csvFile, Action<SalesRecord> processRecord)
    {
        // check for empty file, return bad request if empty, asp.net web api should
        // check for complete absence of file, but this is a good check to have as well.
        if (csvFile.Length == 0)
        {
            throw new ValidationException("Empty file, cannot calculate aggregate data.");
        }

        //check for csv file extension, return bad request if not csv.
        if (csvFile.FileName.Split('.').Last() != "csv")
        {
            throw new ValidationException("File is not a csv file, cannot calculate aggregate data.");
        }

        try
        {
            using (var reader = new StreamReader(csvFile.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    // chose to process records one at a time instead of
                    // letting the library do the default operation of 
                    // creating a collection with all data. This will
                    // allow us to only record the information we
                    // need for the aggregate calculations. Also
                    // avoid a secondary iteration of the data to get
                    // the aggregate values.
                    processRecord(csv.GetRecord<SalesRecord>());
                }
            }
        }
        catch (CsvHelper.MissingFieldException ex)
        {
            throw new ValidationException($"Missing field(s) in csv file {ex.Message}. cannot calculate aggregate data.");
        }
    }
}