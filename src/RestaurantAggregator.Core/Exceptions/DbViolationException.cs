namespace RestaurantAggregator.Core.Exceptions;

public class DbViolationException : Exception
{
    public DbViolationException(string message) : base(message)
    {
    }

    public DbViolationException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public DbViolationException()
    {
    }
}
