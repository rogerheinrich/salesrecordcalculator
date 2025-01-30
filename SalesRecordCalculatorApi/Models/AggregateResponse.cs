namespace SalesRecordCalculator.Models;

/// <summary>
/// Represents the aggregate data calculated from a set of sales records.
/// </summary>
public class AggregateResponse
{
    public decimal MedianUnitCost { get; set; }
    public required string MostCommonRegion { get; set; }
    public DateTime FirstOrderDate { get; set; }
    public DateTime LastOrderDate { get; set; }
    public int DaysBetweenFirstAndLastOrder { get; set; }
    public decimal TotalRevenue { get; set; }
}