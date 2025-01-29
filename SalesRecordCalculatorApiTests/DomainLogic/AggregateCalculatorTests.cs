using SalesRecordCalculator.DomainLogic;
using SalesRecordCalculator.Models;

namespace SalesRecordCalculatorApi.DomainLogic
{
    public class AggregateCalculatorTests
    {
        [Test]
        public void AddRecordToTally_ShouldUpdateRegionCounts()
        {
            // Arrange
            var calculator = new AggregateCalculator();
            var record = new SalesRecord { Region = "North", OrderDate = DateTime.Now, TotalRevenue = 100, UnitCost = 10 };

            // Act
            calculator.AddRecordToTally(record);

            // Assert
            var response = calculator.CalculateAggregate();
            Assert.That(response.MostCommonRegion, Is.EqualTo("North"));
        }

        [Test]
        public void AddRecordToTally_ShouldUpdateOrderDates()
        {
            // Arrange
            var calculator = new AggregateCalculator();
            var firstDate = new DateTime(2023, 1, 1);
            var lastDate = new DateTime(2023, 12, 31);
            var record1 = new SalesRecord { Region = "North", OrderDate = firstDate, TotalRevenue = 100, UnitCost = 10 };
            var record2 = new SalesRecord { Region = "North", OrderDate = lastDate, TotalRevenue = 100, UnitCost = 10 };

            // Act
            calculator.AddRecordToTally(record1);
            calculator.AddRecordToTally(record2);

            // Assert
            var response = calculator.CalculateAggregate();
            Assert.That(response.FirstOrderDate, Is.EqualTo(firstDate));
            Assert.That(response.LastOrderDate, Is.EqualTo(lastDate));
            Assert.That(response.DaysBetweenFirstAndLastOrder, Is.EqualTo(364));
        }

        [Test]
        public void AddRecordToTally_ShouldUpdateTotalRevenue()
        {
            // Arrange
            var calculator = new AggregateCalculator();
            var record1 = new SalesRecord { Region = "North", OrderDate = DateTime.Now, TotalRevenue = 100, UnitCost = 10 };
            var record2 = new SalesRecord { Region = "North", OrderDate = DateTime.Now, TotalRevenue = 200, UnitCost = 20 };

            // Act
            calculator.AddRecordToTally(record1);
            calculator.AddRecordToTally(record2);

            // Assert
            var response = calculator.CalculateAggregate();
            Assert.That(response.TotalRevenue, Is.EqualTo(300));
        }

        [Test]
        public void CalculateAggregate_ShouldReturnMaxRegion()
        {
            // Arrange
            var calculator = new AggregateCalculator();
            var record = new SalesRecord { Region = "South", OrderDate = DateTime.Now, TotalRevenue = 100, UnitCost = 10 };
            var record2 = new SalesRecord { Region = "South", OrderDate = DateTime.Now, TotalRevenue = 100, UnitCost = 10 };
            var record3 = new SalesRecord { Region = "North", OrderDate = DateTime.Now, TotalRevenue = 100, UnitCost = 10 };

            // Act
            calculator.AddRecordToTally(record);
            calculator.AddRecordToTally(record2);
            calculator.AddRecordToTally(record3);

            // Assert
            var response = calculator.CalculateAggregate();
            Assert.That(response.MostCommonRegion, Is.EqualTo("South"));
        }

        [Test]
        public void CalculateAggregate_ShouldReturnCorrectMedianUnitCost()
        {
            // Arrange
            var calculator = new AggregateCalculator();
            var record1 = new SalesRecord { Region = "North", OrderDate = DateTime.Now, TotalRevenue = 100, UnitCost = 10 };
            var record2 = new SalesRecord { Region = "North", OrderDate = DateTime.Now, TotalRevenue = 200, UnitCost = 20 };
            var record3 = new SalesRecord { Region = "North", OrderDate = DateTime.Now, TotalRevenue = 300, UnitCost = 30 };

            // Act
            calculator.AddRecordToTally(record1);
            calculator.AddRecordToTally(record2);
            calculator.AddRecordToTally(record3);

            // Assert
            var response = calculator.CalculateAggregate();
            Assert.That(response.MedianUnitCost, Is.EqualTo(20));
        }

        [Test]
        public void CalculateAggregate_ShouldReturnCorrectMedianUnitCostForEvenNumberOfRecords()
        {
            // Arrange
            var calculator = new AggregateCalculator();
            var record1 = new SalesRecord { Region = "North", OrderDate = DateTime.Now, TotalRevenue = 100, UnitCost = 10 };
            var record2 = new SalesRecord { Region = "North", OrderDate = DateTime.Now, TotalRevenue = 200, UnitCost = 20 };
            var record3 = new SalesRecord { Region = "North", OrderDate = DateTime.Now, TotalRevenue = 300, UnitCost = 30 };
            var record4 = new SalesRecord { Region = "North", OrderDate = DateTime.Now, TotalRevenue = 400, UnitCost = 40 };

            // Act
            calculator.AddRecordToTally(record1);
            calculator.AddRecordToTally(record2);
            calculator.AddRecordToTally(record3);
            calculator.AddRecordToTally(record4);

            // Assert
            var response = calculator.CalculateAggregate();
            Assert.That(response.MedianUnitCost, Is.EqualTo(25));
        }
    }
}