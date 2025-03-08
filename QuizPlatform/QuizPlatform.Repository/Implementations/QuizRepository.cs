using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using QuizPlatform.Domain.Exceptions;
using QuizPlatform.Domain.Models.Entities;
using QuizPlatform.Repository.Database;
using QuizPlatform.Service.Services.Abstractions;
using QuizPlatform.Services.Abstractions;
using static System.Net.Mime.MediaTypeNames;

namespace QuizPlatform.Repository.Implementations
{
    public class QuizRepository : IQuizRepository
    {
        private readonly AppDbContext _databaseContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;

        public QuizRepository(AppDbContext databaseContext, IUnitOfWork unitOfWork , IServiceProvider serviceProvider)
        {
            _databaseContext = databaseContext;
            _unitOfWork = unitOfWork;
            _serviceProvider = serviceProvider;
        }


        public async Task UpdateQuizAsync(Quiz quiz)
        {
         
            {
                _unitOfWork.Start();
                _databaseContext.Quizzes.Attach(quiz);
                await _unitOfWork.CompleteAsync();
            }

        }

        public async Task<Quiz> GetQuizByIdAsync(int id)
        {
            return await _databaseContext.Quizzes.FirstOrDefaultAsync(q => q.QuizId == id)
                ?? throw new ObjectNotFoundException(id, nameof(Quiz));
        }



        public async Task<List<Quiz>> GetQuizzesAsync(int? id = null, bool includeActiveQuizzes = false, bool includeTests = false, bool includeQuestions = false,bool includeCorrectAnswers = false)
        {

            var item = _databaseContext.Quizzes.AsNoTracking()
                                                   .Where(q => q.QuizId == id || id == null);
            
            if(includeActiveQuizzes)
            {
                item = item.Where(q => q.IsActive);
            }
            if (includeTests)
            {
               
                if (includeQuestions)
                {
                    item = item.Include(q => q.QuizzesTests)
                               .ThenInclude(l => l.Test)
                               .ThenInclude(t => t.Questions!)
                               .ThenInclude(q => q.Answers);
                }
                else
                {
                    item = item.Include(q => q.QuizzesTests)
                               .ThenInclude(l => l.Test);
                }

                var listQuizzes = item.ToList()

                        .Select(q => {
                            
                            q.AllTests = q.QuizzesTests.Select(l => l.Test).ToList()!;
                            if(!includeCorrectAnswers)
                            {
                                foreach (var itemTest in q.AllTests)
                                {
                                    foreach (var itemQuestion in  itemTest.Questions!)
                                    {
                                        itemQuestion.Answers.ForEach(a => { a.IsCorrect = false; });
                                    }
                                }
                                q.QuizzesTests = null;
                            }
                            return q;
                        }
                        ).ToList();
                return  listQuizzes;
            }
            return  await  item.ToListAsync();
        }

        public async Task<int> AddQuizAsync(Quiz quiz)
        {
            _unitOfWork.Start();
            await _databaseContext.AddAsync(quiz);
            await _unitOfWork.CompleteAsync();

            return quiz.QuizId;
        }

        public async Task<int> AddQuizResultAsync(QuizResult quizResult)
        {
            _unitOfWork.Start();
            await _databaseContext.AddAsync(quizResult);
            await _unitOfWork.CompleteAsync();

            return quizResult.QuizResultId;
        }

        public async Task UpdateQuizResultAsync(QuizResult quizResult)
        {

            _unitOfWork.Start();
            _databaseContext.Attach(quizResult);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<QuizResult> GetQuizResultByIdAsync(int quizResultId )
        {

                        return await _databaseContext.QuizResults.FirstOrDefaultAsync(t => t.QuizResultId == quizResultId)
                                                             ?? throw new ObjectNotFoundException(quizResultId, nameof(QuizResult));
        }
        public async Task DeleteQuizAsync(int quizId)
        {

            var quizDeleted = await _databaseContext.Quizzes.FirstOrDefaultAsync(t => t.QuizId == quizId);

            if (quizDeleted != null)
            {
                _unitOfWork.Start();
                var a = _databaseContext.Quizzes.Remove(quizDeleted);
                await _unitOfWork.CompleteAsync();
            }

        }
    }
}
