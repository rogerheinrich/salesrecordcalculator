using System.Numerics;
using CsvHelper.Configuration.Attributes;

namespace SalesRecordCalculator.DomainLogic.Models;

/// <summary>
/// Represents a sales record from a csv file.
/// </summary>
public class SalesRecord
{
    public required string Region { get; set; }

    [Optional]
    public string? Country { get; set; }

    [Optional]
    [Name("Item Type")]
    public string? ItemType { get; set; }

    [Optional]
    [Name("Sales Channel")]
    public string? SalesChannel { get; set; }

    [Optional]
    [Name("Order Priority")]
    public string? OrderPriority { get; set; }

    [Name("Order Date")]
    public required DateTime OrderDate { get; set; }

    [Optional]
    [Name("Order ID")]
    public BigInteger OrderId { get; set; }

    [Optional]
    [Name("Ship Date")]
    public DateTime ShipDate { get; set; }

    [Optional]
    [Name("Units Sold")]
    public int UnitsSold { get; set; }

    [Optional]
    [Name("Unit Price")]
    public decimal UnitPrice { get; set; }

    [Name("Unit Cost")]
    public required decimal UnitCost { get; set; }

    [Name("Total Revenue")]
    public required decimal TotalRevenue { get; set; }

    [Optional]
    [Name("Total Cost")]
    public decimal TotalCost { get; set; }

    [Optional]
    [Name("Total Profit")]
    public decimal TotalProfit { get; set; }
}