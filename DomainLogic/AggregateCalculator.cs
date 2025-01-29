using SalesRecordCalculator.Models;

namespace SalesRecordCalculator.DomainLogic;
public class AggregateCalculator : IAggregateCalculator
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

    private void addRegionToTally(string region)
    {
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
        if (unitCosts.Count % 2 != 0)
        {
            median = QuickSelect(unitCosts, unitCosts.Count / 2);
        }
        else
        {
            var leftMedian = QuickSelect(unitCosts, unitCosts.Count / 2 - 1);
            var RightMedian = QuickSelect(unitCosts, unitCosts.Count / 2);
            median = (leftMedian + RightMedian) / 2;
        }
        return median;
    }

    private decimal QuickSelect(List<decimal> values, int k)
    {
        int left = 0, right = values.Count - 1;

        while (left <= right)
        {
            int pivotIndex = Partition(values, left, right);

            if (pivotIndex == k)
                return values[pivotIndex];
            else if (pivotIndex < k)
                left = pivotIndex + 1;
            else
                right = pivotIndex - 1;
        }
        return -1; // Should never reach here
    }

    private int Partition(List<decimal> array, int leftIndex, int rightIndex)
    {
        decimal pivotValue = array[rightIndex];
        int partitionIndex = leftIndex;

        for (int comparatorIndex = leftIndex; comparatorIndex < rightIndex; comparatorIndex++)
        {
            var comparatorValue = array[comparatorIndex];
            if (comparatorValue < pivotValue)
            {
                SwapValues(array, partitionIndex, comparatorIndex);
                partitionIndex++;
            }
        }
        SwapValues(array, partitionIndex, rightIndex);
        return partitionIndex;
    }

    private void SwapValues(List<decimal> array, int index1, int index2)
    {
        decimal temp = array[index1];
        array[index1] = array[index2];
        array[index2] = temp;
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
}