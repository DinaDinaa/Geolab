using Microsoft.AspNetCore.Mvc;
using QuizPlatform.Domain.Commands;
using QuizPlatform.Domain.Models.Entities;
using QuizPlatform.Repository.Implementations;
using QuizPlatform.Services.Abstractions;

namespace QuizPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {

        private readonly ITestRepository _testRepository;
        private readonly ITestService _testService;
        private readonly ILogger<TestsController> _logger;

        public TestsController( ITestRepository testRepository , ILogger<TestsController> logger , ITestService testService)
        {
            
            _testRepository = testRepository;
            _logger = logger;
            _testService = testService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Test>>> GetTests()
        {
            _logger.LogInformation("Searching tests");
            return await _testRepository.GetTestsAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Test>> GetTest(int id)
        {
            _logger.LogInformation("Searching quiz with id: {id}", id);
            var test = await _testRepository.GetTestByIdAsync(id);

            if (test == null)
            {
                _logger.LogWarning("Test not found");
                return NotFound();
            }

            return test;
        }

        [HttpPost]
        public async Task PostTest(TestCommand command)
        {
            await _testService.AddTestAsync(command);
        }


        [HttpPut]
        public async Task PutTest( TestCommand command)
        {
            await _testService.UpdateTestAsync(command);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTest(int id)
        {
            _logger.LogInformation($"Deleted test with  id  = {id}");
            
            var test = await _testRepository.GetTestByIdAsync(id);
            if (test == null)
            {
                _logger.LogWarning("Test not found");
                return NotFound();
            }

            await _testRepository.DeleteTestAsync(id);

            return NoContent();

        }

    }
}
