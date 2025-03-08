using QuizPlatform.Domain.Commands;
using QuizPlatform.Domain.Models.Entities;
using QuizPlatform.Service.Services.Abstractions;
using QuizPlatform.Services.Abstractions;



namespace QuizPlatform.Service.Services.Implementations
{
    public class TestService : ITestService
    {

        public TestService(ITestRepository testRepository, IUnitOfWork unitOfWork)
        {
            _testRepository = testRepository;
            _unitOfWork = unitOfWork;

        }

        private readonly ITestRepository _testRepository;

        private readonly IUnitOfWork _unitOfWork;


        public async Task<int> AddTestAsync(TestCommand command)
        {
            command.Validate();

            var test = new Test
            {
                TestName = command.TestName,
                TestTime = command.TestTime,
                QuestionCount = command.QuestionCount,
                IsActive = command.IsActive


            };

            await _testRepository.AddTestAsync(test);
            return command.TestId;
        }


        public async Task DeleteTestAsync(int testId)
        {
            await _testRepository.DeleteTestAsync(testId);
        }       
        public async Task UpdateTestAsync(TestCommand command)
        {
            command.Validate();

            var test = new Test
            {
                TestId = command.TestId,
                TestName = command.TestName,
                TestTime = command.TestTime,
                QuestionCount = command.QuestionCount,
                IsActive = command.IsActive


            };
            await _testRepository.UpdateTestAsync(test);
        }
    }
}
