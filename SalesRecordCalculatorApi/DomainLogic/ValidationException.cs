namespace SalesRecordCalculator.DomainLogic;

/// <summary>
/// Wrapper for exceptions that occur during validation of csv files.
/// </summary>
public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}