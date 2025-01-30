using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SalesRecordCalculator.Controllers;
using SalesRecordCalculator.DomainLogic;
using SalesRecordCalculator.DomainLogic.Models;
using SalesRecordCalculator.Models;
using System.Text;

namespace SalesRecordCalculatorApi.Controllers
{
    [TestFixture]
    public class AggregateControllerTest
    {
        private Mock<ISalesRecordReader> _mockSalesRecordReader;
        private Mock<IAggregateCalculator> _mockAggregateCalculator;
        private AggregateController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockSalesRecordReader = new Mock<ISalesRecordReader>();
            _mockAggregateCalculator = new Mock<IAggregateCalculator>();
            _controller = new AggregateController(_mockSalesRecordReader.Object, _mockAggregateCalculator.Object);
        }

        [Test]
        public void CalculateFromCsv_ValidCsvFile_ReturnsOkResult()
        {
            // Arrange
            var csvContent = "Region,Sales\nNorth,100\nSouth,200";
            var csvFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(csvContent)), 0, csvContent.Length, "csvFile", "test.csv");

            _mockSalesRecordReader.Setup(reader => reader.ProcessSalesRecords(
                It.IsAny<IFormFile>(), It.IsAny<Action<SalesRecord>>()))
                .Callback<IFormFile, Action<SalesRecord>>((file, callback) =>
                {
                    callback(new SalesRecord { Region = "North", OrderDate = DateTime.Now, TotalRevenue = 100, UnitCost = 10 });
                });

            DateTime now = DateTime.Now;
            _mockAggregateCalculator.Setup(calculator => calculator.CalculateAggregate())
                .Returns(new AggregateResponse
                {
                    MedianUnitCost = 2,
                    MostCommonRegion = "North",
                    FirstOrderDate = now.AddDays(-1),
                    LastOrderDate = now,
                    DaysBetweenFirstAndLastOrder = 1,
                    TotalRevenue = 300
                });

            // Act
            var result = _controller.calculateFromCsv(csvFile);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.InstanceOf<AggregateResponse>());
            Assert.That(((AggregateResponse)okResult.Value).MedianUnitCost, Is.EqualTo(2));
            Assert.That(((AggregateResponse)okResult.Value).MostCommonRegion, Is.EqualTo("North"));
            Assert.That(((AggregateResponse)okResult.Value).FirstOrderDate, Is.EqualTo(now.AddDays(-1)));
            Assert.That(((AggregateResponse)okResult.Value).LastOrderDate, Is.EqualTo(now));
            Assert.That(((AggregateResponse)okResult.Value).DaysBetweenFirstAndLastOrder, Is.EqualTo(1));
            Assert.That(((AggregateResponse)okResult.Value).TotalRevenue, Is.EqualTo(300));
        }

        [Test]
        public void CalculateFromCsv_InvalidCsvFile_ReturnsBadRequest()
        {
            // Arrange
            var csvContent = "InvalidContent";
            var csvFile = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes(csvContent)), 0, csvContent.Length, "csvFile", "test.csv");

            _mockSalesRecordReader.Setup(reader => reader.ProcessSalesRecords(It.IsAny<IFormFile>(), It.IsAny<Action<SalesRecord>>()))
                .Throws(new ValidationException("Invalid CSV file"));

            // Act
            var result = _controller.calculateFromCsv(csvFile);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult, Is.Not.Null);
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
        }
    }
}