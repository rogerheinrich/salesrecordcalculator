using SalesRecordCalculator.Models;

namespace SalesRecordCalculator.DomainLogic;
public interface ISalesRecordReader
{
    void ProcessSalesRecords(IFormFile csvFile, Action<SalesRecord> processRecord);
}