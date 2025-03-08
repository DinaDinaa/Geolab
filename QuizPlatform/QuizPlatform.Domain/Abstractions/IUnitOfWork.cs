namespace QuizPlatform.Service.Services.Abstractions;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    void Start();

    Task CompleteAsync();
}