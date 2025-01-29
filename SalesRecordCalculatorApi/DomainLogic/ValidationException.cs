namespace SalesRecordCalculator.DomainLogic;

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message)
    {
    }
}