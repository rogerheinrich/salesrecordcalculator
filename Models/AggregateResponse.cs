using System.Numerics;

namespace SalesRecordCalculator.Models
{
    public class AggregateResponse
    {
        public double MedianUnitCost { get; set; }
        public double MostCommonRegion { get; set; }
        public DateTime FirstOrderDate { get; set; }
        public DateTime LastOrderDate { get; set; }
        public int DaysBetweenFirstAndLastOrder { get; set; }
        public BigInteger TotalRevenue { get; set; }

    }
}