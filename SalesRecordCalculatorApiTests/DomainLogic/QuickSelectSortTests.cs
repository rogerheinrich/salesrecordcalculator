using SalesRecordCalculator.DomainLogic;

namespace SalesRecordCalculatorApi.DomainLogic
{
    public class QuickSelectSortTest
    {
        private readonly QuickSelectSort _quickSelectSort;

        public QuickSelectSortTest()
        {
            _quickSelectSort = new QuickSelectSort();
        }

        [Test]
        public void QuickSelect_ShouldReturnKthSmallestElement_ForValidInput()
        {
            // Arrange
            var values = new List<decimal> { 3.5m, 2.1m, 5.6m, 1.2m, 4.8m };
            int k = 2;

            // Act
            var result = _quickSelectSort.QuickSelect(values, k);

            // Assert
            Assert.That(result, Is.EqualTo(3.5m));
        }

        [Test]
        public void QuickSelect_ShouldReturnSmallestElement_ForKEqualZero()
        {
            // Arrange
            var values = new List<decimal> { 3.5m, 2.1m, 5.6m, 1.2m, 4.8m };
            int k = 0; // Smallest element

            // Act
            var result = _quickSelectSort.QuickSelect(values, k);

            // Assert
            Assert.That(result, Is.EqualTo(1.2m));
        }

        [Test]
        public void QuickSelect_ShouldReturnLargestElement_ForKEqualToCountMinusOne()
        {
            // Arrange
            var values = new List<decimal> { 3.5m, 2.1m, 5.6m, 1.2m, 4.8m };
            int k = values.Count - 1; // Largest element

            // Act
            var result = _quickSelectSort.QuickSelect(values, k);

            // Assert
            Assert.That(result, Is.EqualTo(5.6m));
        }

        [Test]
        public void QuickSelect_ShouldThrowInvalidOperationException_ForInvalidK()
        {
            // Arrange
            var values = new List<decimal> { 3.5m, 2.1m, 5.6m, 1.2m, 4.8m };
            int k = values.Count; // Invalid k (out of bounds)

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _quickSelectSort.QuickSelect(values, k));
        }

        [Test]
        public void QuickSelect_ShouldHandleSingleElementList()
        {
            // Arrange
            var values = new List<decimal> { 3.5m };
            int k = 0; // Only one element

            // Act
            var result = _quickSelectSort.QuickSelect(values, k);

            // Assert
            Assert.That(result, Is.EqualTo(3.5m));
        }
    }
}