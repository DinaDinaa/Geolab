
using Microsoft.EntityFrameworkCore;
using QuizPlatform.Domain.Exceptions;
using QuizPlatform.Domain.Models.Entities;
using QuizPlatform.Repository.Database;
using QuizPlatform.Service.Services.Abstractions;
using QuizPlatform.Services.Abstractions;


namespace QuizPlatform.Services.Implementations
{
    public class TestRepository : ITestRepository
    {
        private readonly AppDbContext _databaseContext;
        private readonly IUnitOfWork _unitOfWork;

        public TestRepository(AppDbContext databaseContext, IUnitOfWork unitOfWork)
        {
            _databaseContext = databaseContext;
            _unitOfWork = unitOfWork;
        }


        public async Task<int> AddTestAsync(Test test)
        {
            _unitOfWork.Start();
            await _databaseContext.AddAsync(test);
            await _unitOfWork.CompleteAsync();

            return test.TestId;
        }
        public async Task UpdateTestAsync(Test test)
        {
            _unitOfWork.Start();
            _databaseContext.Entry(test).State = EntityState.Modified;
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteTestAsync(int testId)
        {

            var testDeleted = await _databaseContext.Tests.FirstOrDefaultAsync(t => t.TestId == testId);

            if (testDeleted != null)
            {
                _unitOfWork.Start();
                var a = _databaseContext.Tests.Remove(testDeleted);
                await _unitOfWork.CompleteAsync();
            }

        }

        public async Task<Test> GetTestByIdAsync(int id)
        {
            return await _databaseContext.Tests.FirstOrDefaultAsync(t => t.TestId == id)
                                                 ?? throw new ObjectNotFoundException(id, nameof(Test));
        }

        public async Task<List<Test>> GetTestsAsync()
        {
            return await _databaseContext.Tests
                                          .AsNoTracking()
                                          .Include(t => t.Questions!)
                                          .ThenInclude(q=>q.Answers)
                                          .ToListAsync();
        }


    }

}
