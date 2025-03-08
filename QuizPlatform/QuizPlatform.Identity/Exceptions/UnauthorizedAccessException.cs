namespace QuizPlatform.Identity.Exceptions;

public class UnauthorizedAccessException : Exception
{
    public UnauthorizedAccessException()
    {
    }

    public UnauthorizedAccessException(string message) : base(message)
    {
    }
}