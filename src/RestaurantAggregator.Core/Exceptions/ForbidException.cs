namespace RestaurantAggregator.Core.Exceptions;

public class ForbidException : Exception
{
    public ForbidException(string message) : base(message)
    {
    }

    public ForbidException(string message, Exception innerException) : base(message, innerException)
    {
    }

    public ForbidException()
    {
    }
}
