using System.Numerics;

namespace SalesRecordCalculator.DomainLogic;

public interface IQuickSelectSort
{
    tValueType QuickSelect<tValueType>(List<tValueType> values, int k) where tValueType : INumber<tValueType>;
}

/// <summary>
/// QuickSelectSort class for sorting values and finding the kth smallest element
/// Utilizes an iterative approach to avoid stack overflow with large datasets
/// Utilizes a random partition selection to avoid worst case performance
/// </summary>
public class QuickSelectSort : IQuickSelectSort
{
    private Random Random = new Random();

    /// <summary>
    /// Finds the kth smallest element in a list of values
    /// </summary>
    /// <typeparam name="tValueType">type of values to sort through</typeparam>
    /// <param name="values">List of values to sort</param>
    /// <param name="k">Index in sorted list to return the value for</param>
    /// <returns>Value of the kth smallest element from the list of values</returns>
    /// <exception cref="InvalidOperationException">Thrown when K is outside the bounds of the list provided.</exception>
    public tValueType QuickSelect<tValueType>(List<tValueType> values, int k) where tValueType : INumber<tValueType>
    {
        int left = 0, right = values.Count - 1;

        while (left <= right)
        {
            int pivotIndex = RandomPartition(values, left, right);

            if (pivotIndex == k)
                return values[pivotIndex];
            else if (pivotIndex < k)
                left = pivotIndex + 1;
            else
                right = pivotIndex - 1;
        }
        throw new InvalidOperationException("No solution found");
    }

    private int RandomPartition<tValueType>(List<tValueType> values, int left, int right) where tValueType : INumber<tValueType>
    {
        // Select a random pivot index between left and right
        // to avoid the worst case of quicksort
        int randomPivotIndex = Random.Next(left, right + 1);
        SwapValues(values, randomPivotIndex, right); // Move pivot to end
        return Partition(values, left, right);
    }

    private int Partition<tValueType>(List<tValueType> values, int leftIndex, int rightIndex) where tValueType : INumber<tValueType>
    {
        var pivotValue = values[rightIndex];
        int partitionIndex = leftIndex;

        for (int comparatorIndex = leftIndex; comparatorIndex < rightIndex; comparatorIndex++)
        {
            var comparatorValue = values[comparatorIndex];
            if (comparatorValue < pivotValue)
            {
                SwapValues(values, partitionIndex, comparatorIndex);
                partitionIndex++;
            }
        }
        SwapValues(values, partitionIndex, rightIndex);
        return partitionIndex;
    }

    private void SwapValues<tValueType>(List<tValueType> values, int index1, int index2) where tValueType : INumber<tValueType>
    {
        var temp = values[index1];
        values[index1] = values[index2];
        values[index2] = temp;
    }

}