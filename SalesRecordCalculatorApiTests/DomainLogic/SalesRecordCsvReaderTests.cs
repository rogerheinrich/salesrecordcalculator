using System.Numerics;
using System.Text;
using Microsoft.AspNetCore.Http;
using Moq;
using SalesRecordCalculator.DomainLogic;
using SalesRecordCalculator.DomainLogic.Models;

namespace SalesRecordCalculatorApi.DomainLogic
{
    public class SalesRecordCsvReaderTests
    {
        [Test]
        public void ProcessSalesRecords_EmptyFile_ThrowsValidationException()
        {
            // Arrange
            var csvReader = new SalesRecordCsvReader();
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(0);

            // Act & Assert
            Assert.Throws<ValidationException>(() => csvReader.ProcessSalesRecords(mockFile.Object, record => { }));
        }

        [Test]
        public void ProcessSalesRecords_InvalidFileExtension_ThrowsValidationException()
        {
            // Arrange
            var csvReader = new SalesRecordCsvReader();
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(100);
            mockFile.Setup(f => f.FileName).Returns("test.txt");

            // Act & Assert
            Assert.Throws<ValidationException>(() => csvReader.ProcessSalesRecords(mockFile.Object, record => { }));
        }

        [Test]
        public void ProcessSalesRecords_ValidCsvFile_ProcessesRecords()
        {
            // Arrange
            var csvReader = new SalesRecordCsvReader();
            var csvContent =
@"Region,Country,Item Type,Sales Channel,Order Priority,Order Date,Order ID,Ship Date,Units Sold,Unit Price,Unit Cost,Total Revenue,Total Cost,Total Profit
Middle East and North Africa,Azerbaijan,Snacks,Online,C,10/8/2014,535113847,10/23/2014,934,152.58,97.44,142509.72,91008.96,51500.76";
            var mockFile = new Mock<IFormFile>();
            var contentBytes = Encoding.UTF8.GetBytes(csvContent);
            var stream = new MemoryStream(contentBytes);
            mockFile.Setup(f => f.Length).Returns(contentBytes.Length);
            mockFile.Setup(f => f.FileName).Returns("test.csv");
            mockFile.Setup(f => f.OpenReadStream()).Returns(stream);

            var processedRecords = new List<SalesRecord>();

            // Act
            csvReader.ProcessSalesRecords(mockFile.Object, record => processedRecords.Add(record));

            // Assert
            Assert.That(processedRecords.Count, Is.EqualTo(1));
            Assert.That(processedRecords[0].Region, Is.EqualTo("Middle East and North Africa"));
            Assert.That(processedRecords[0].Country, Is.EqualTo("Azerbaijan"));
            Assert.That(processedRecords[0].ItemType, Is.EqualTo("Snacks"));
            Assert.That(processedRecords[0].SalesChannel, Is.EqualTo("Online"));
            Assert.That(processedRecords[0].OrderPriority, Is.EqualTo("C"));
            Assert.That(processedRecords[0].OrderDate, Is.EqualTo(new DateTime(2014, 10, 8)));
            Assert.That(processedRecords[0].OrderId, Is.EqualTo((BigInteger)535113847));
            Assert.That(processedRecords[0].ShipDate, Is.EqualTo(new DateTime(2014, 10, 23)));
            Assert.That(processedRecords[0].UnitsSold, Is.EqualTo(934));
            Assert.That(processedRecords[0].UnitPrice, Is.EqualTo(152.58m));
            Assert.That(processedRecords[0].UnitCost, Is.EqualTo(97.44m));
            Assert.That(processedRecords[0].TotalRevenue, Is.EqualTo(142509.72m));
            Assert.That(processedRecords[0].TotalCost, Is.EqualTo(91008.96m));
            Assert.That(processedRecords[0].TotalProfit, Is.EqualTo(51500.76m));
        }

        [Test]
        public void ProcessSalesRecords_MissingField_ThrowsValidationException()
        {
            // Arrange
            var csvReader = new SalesRecordCsvReader();
            var csvContent = "Id,Name\n1,Test\n2,Test2";
            var mockFile = new Mock<IFormFile>();
            var contentBytes = Encoding.UTF8.GetBytes(csvContent);
            var stream = new MemoryStream(contentBytes);
            mockFile.Setup(f => f.Length).Returns(contentBytes.Length);
            mockFile.Setup(f => f.FileName).Returns("test.csv");
            mockFile.Setup(f => f.OpenReadStream()).Returns(stream);

            // Act & Assert
            Assert.Throws<ValidationException>(() => csvReader.ProcessSalesRecords(mockFile.Object, record => { }));
        }
    }
}