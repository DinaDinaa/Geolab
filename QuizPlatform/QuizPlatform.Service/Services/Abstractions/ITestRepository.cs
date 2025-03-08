using QuizPlatform.Domain.Models.Entities;

namespace QuizPlatform.Services.Abstractions
{
    public interface ITestRepository
    {
        Task<int> AddTestAsync(Test test);
        Task UpdateTestAsync(Test test);
        Task DeleteTestAsync(int id);
        Task<List<Test>> GetTestsAsync();
        Task<Test> GetTestByIdAsync(int id);

    }
}
