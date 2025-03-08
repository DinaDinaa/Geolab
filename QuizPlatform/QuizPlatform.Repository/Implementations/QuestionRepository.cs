using Microsoft.EntityFrameworkCore;
using QuizPlatform.Domain.Exceptions;
using QuizPlatform.Domain.Models.Entities;
using QuizPlatform.Repository.Database;
using QuizPlatform.Service.Services.Abstractions;
using QuizPlatform.Services.Abstractions;


namespace QuizPlatform.Services.Implementations
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext _databaseContext;
        private readonly IUnitOfWork _unitOfWork;

        public QuestionRepository(AppDbContext databaseContext, IUnitOfWork unitOfWork)
        {
            _databaseContext = databaseContext;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> AddQuestionAsync(Question question)
        {
            _unitOfWork.Start();
            await _databaseContext.AddAsync(question);
            await _unitOfWork.CompleteAsync();

            return question.QuestionId;
        }
        public async Task UpdateAsync(Question question)
        {
            _unitOfWork.Start();
            _databaseContext.Questions.Attach(question);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteQuestionAsync(int questionId)
        {

            var questionDeleted = await _databaseContext.Questions.FirstOrDefaultAsync(q => q.QuestionId == questionId);

            if (questionDeleted != null)
            {
                _unitOfWork.Start();
                var a = _databaseContext.Questions.Remove(questionDeleted);
                await _unitOfWork.CompleteAsync();
            }

        }

        public async Task<Question> GetQuestionByIdAsync(int id)
        {
            return await _databaseContext.Questions.FirstOrDefaultAsync(q => q.QuestionId == id)
               ?? throw new ObjectNotFoundException(id, nameof(Question));
        }

        public async Task<List<Question>> GetQuestionsAsync()
        {
            return await _databaseContext.Questions
                                           .AsNoTracking()
                                           .ToListAsync();
        }

    }
}
