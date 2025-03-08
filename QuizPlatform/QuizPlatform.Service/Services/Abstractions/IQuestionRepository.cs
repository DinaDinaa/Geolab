using QuizPlatform.Domain.Models.Entities;

namespace QuizPlatform.Services.Abstractions
{
    public interface IQuestionRepository
    {
        Task<int> AddQuestionAsync(Question question);
        Task UpdateAsync(Question question);
        Task DeleteQuestionAsync(int id);
        Task<List<Question>> GetQuestionsAsync();
        Task<Question> GetQuestionByIdAsync(int id);
    }
}
