using System.Data;
using System.Diagnostics;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using SalesRecordCalculator.Models;

namespace SalesRecordCalculator.DomainLogic;
public class SalesRecordCsvReader : ISalesRecordReader
{
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