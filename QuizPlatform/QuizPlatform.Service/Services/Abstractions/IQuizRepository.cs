using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using QuizPlatform.Domain.Models.Entities;

namespace QuizPlatform.Services.Abstractions
{
    public interface IQuizRepository
    {
        Task<int> AddQuizAsync(Quiz quiz);
        Task UpdateQuizAsync(Quiz quiz);
        Task<List<Quiz>> GetQuizzesAsync(int? id = null, bool includeActiveQuizzes = false, bool includeTests = false, bool includeQuestions = false, bool includeCorrectAnswers = false);

        Task<int> AddQuizResultAsync(QuizResult quizResult);
        Task UpdateQuizResultAsync(QuizResult quizResult);
       
        Task<QuizResult> GetQuizResultByIdAsync(int quizResultId);
        Task DeleteQuizAsync(int quizId);
    }
}
