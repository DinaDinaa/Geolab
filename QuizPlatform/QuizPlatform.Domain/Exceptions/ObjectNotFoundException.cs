namespace QuizPlatform.Domain.Exceptions;

public class ObjectNotFoundException : DomainException
{
    public ObjectNotFoundException(object id, string objectType)
        : base($"{objectType} with id: '{id}' not found")
    {
    }
}