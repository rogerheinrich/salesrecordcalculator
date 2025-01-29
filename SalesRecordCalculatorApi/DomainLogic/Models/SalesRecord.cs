using System.Numerics;
using CsvHelper.Configuration.Attributes;

namespace SalesRecordCalculator.Models;

public class SalesRecord
{
    //I chose to use decimals for the representation of the financial data as
    //I imagined that the precision offered for the financial calculations may
    //be preferential to the performance benefits of using floats or doubles.

    //Also not knowing how large the order ids could be, I chose to use a BigInteger
    //to represent them as well, as it can handle arbitrarily large integers.

    //Seeing as well as I am unsure what the data will look like, I chose to make
    //most of the string fields nullable since they aren't actually required
    //in any calculations. Had we been storing this data more long term 
    //I may have chosen to make them required as data cleanliness in long term
    //storage could be more important, but since we get new data for each request,
    //flexibility seemed a wiser choice to allow more data to be easily processed. 

    public required string Region { get; set; }
    public string? Country { get; set; }

    [Name("Item Type")]
    public string? ItemType { get; set; }

    [Name("Sales Channel")]
    public string? SalesChannel { get; set; }

    [Name("Order Priority")]
    public string? OrderPriority { get; set; }

    [Name("Order Date")]
    public required DateTime OrderDate { get; set; }

    [Name("Order ID")]
    public BigInteger OrderId { get; set; }

    [Name("Ship Date")]
    public DateTime ShipDate { get; set; }

    [Name("Units Sold")]
    public int UnitsSold { get; set; }

    [Name("Unit Price")]
    public decimal UnitPrice { get; set; }

    [Name("Unit Cost")]
    public required decimal UnitCost { get; set; }

    [Name("Total Revenue")]
    public required decimal TotalRevenue { get; set; }

    [Name("Total Cost")]
    public decimal TotalCost { get; set; }

    [Name("Total Profit")]
    public decimal TotalProfit { get; set; }
}