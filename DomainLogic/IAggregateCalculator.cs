using SalesRecordCalculator.Models;

namespace SalesRecordCalculator.DomainLogic;
public interface IAggregateCalculator
{
    void AddRecordToTally(SalesRecord record);
    AggregateResponse CalculateAggregate();
}