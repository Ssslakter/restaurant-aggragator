namespace RestaurantAggregator.Core.Exceptions;

public class NotFoundInDbException : Exception
{
    public NotFoundInDbException()
    {
    }

    public NotFoundInDbException(string? message) : base(message)
    {
    }

    public NotFoundInDbException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}