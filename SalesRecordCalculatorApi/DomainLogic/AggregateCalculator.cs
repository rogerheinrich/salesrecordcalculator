using SalesRecordCalculator.DomainLogic.Models;
using SalesRecordCalculator.Models;

namespace SalesRecordCalculator.DomainLogic;

public interface IAggregateCalculator
{
    void AddRecordToTally(SalesRecord record);
    AggregateResponse CalculateAggregate();
}

public class AggregateCalculator(IQuickSelectSort SortAlgorithm) : IAggregateCalculator
{
    private Dictionary<string, int> regionCounts = new Dictionary<string, int>();
    private DateTime firstRecordDate;
    private DateTime lastRecordDate;
    private decimal totalRevenue;
    private List<decimal> unitCosts = new List<decimal>();

    public void AddRecordToTally(SalesRecord record)
    {
        addRegionToTally(record.Region);
        addOrderDateToTally(record.OrderDate);
        totalRevenue += record.TotalRevenue;
        unitCosts.Add(record.UnitCost);
    }

    public AggregateResponse CalculateAggregate()
    {
        var mostCommonRegion = regionCounts.MaxBy(x => x.Value).Key;
        var daysBetweenFirstAndLastOrder = (int)(lastRecordDate - firstRecordDate).TotalDays;
        var medianCost = FindMedianUnitCost();

        return new AggregateResponse
        {
            MostCommonRegion = mostCommonRegion,
            FirstOrderDate = firstRecordDate,
            LastOrderDate = lastRecordDate,
            DaysBetweenFirstAndLastOrder = daysBetweenFirstAndLastOrder,
            TotalRevenue = totalRevenue,
            MedianUnitCost = medianCost
        };
    }

    private void addRegionToTally(string region)
    {
        // first time through add region to dictionary
        // subsequent times increment the count as we encounter the region
        if (regionCounts.ContainsKey(region))
        {
            regionCounts[region]++;
        }
        else
        {
            regionCounts[region] = 1;
        }
    }

    private void addOrderDateToTally(DateTime orderDate)
    {
        //first time through set the first and last record date
        //subsequent times update the first and last record date as needed
        if (firstRecordDate == default)
        {
            firstRecordDate = orderDate;
            lastRecordDate = orderDate;
        }
        else
        {
            if (orderDate < firstRecordDate)
            {
                firstRecordDate = orderDate;
            }
            else if (orderDate > lastRecordDate)
            {
                lastRecordDate = orderDate;
            }
        }
    }

    private decimal FindMedianUnitCost()
    {
        decimal median;
        //if the count is odd, return the middle value
        //if the count is even, return the average of the two middle values
        if (unitCosts.Count % 2 != 0)
        {
            median = SortAlgorithm.QuickSelect(unitCosts, unitCosts.Count / 2);
        }
        else
        {
            var leftMedian = SortAlgorithm.QuickSelect(unitCosts, unitCosts.Count / 2 - 1);
            var RightMedian = SortAlgorithm.QuickSelect(unitCosts, unitCosts.Count / 2);
            median = (leftMedian + RightMedian) / 2;
        }
        return median;
    }
}