
using QuizPlatform.Domain.Commands;
using QuizPlatform.Domain.Models.Entities;
namespace QuizPlatform.Services.Abstractions
{
    public interface ITestService
    {
         Task<int> AddTestAsync(TestCommand command);
         Task DeleteTestAsync(int id);
         Task UpdateTestAsync(TestCommand command);

    }
}
