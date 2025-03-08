using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using QuizPlatform.Domain.Abstractions;
using QuizPlatform.Domain.Commands;
using QuizPlatform.Domain.Exceptions;
using QuizPlatform.Domain.Models.Entities;
using QuizPlatform.Repository.Database;
using QuizPlatform.Services.Abstractions;
using System.Security.Principal;

namespace QuizPlatform.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizzesController : ControllerBase
    {

        private readonly IQuizRepository _quizRepository;
        private readonly IQuizService _quizService;
        private readonly ILogger _logger;

        public QuizzesController(IQuizRepository quizRepository, IQuizService quizService, ILogger<QuizzesController> logger)
        {
            _quizRepository = quizRepository;
            _quizService = quizService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Quiz>> GetQuizzes([FromQuery] int quizId)
        {
            
            _logger.LogInformation("Searching quiz with id: {QuizId}", quizId);
            var quizzes = await _quizRepository.GetQuizzesAsync(id: quizId, includeActiveQuizzes: true, includeTests: true, includeQuestions: true);
            var quiz = quizzes.FirstOrDefault();
            if (quiz is null)
            {
                _logger.LogWarning("Quiz not found");
                return NotFound();
            }
           
            _logger.LogDebug("Quiz found: {@Quiz}", quiz);
            return await Task.FromResult(quiz);
        }
       
        [HttpGet("{id}")]
        public async Task<ActionResult<Quiz>> GetQuiz(int id)
        {
            _logger.LogInformation("Searching quiz with id: {id}", id);
            var quizzes = await _quizRepository.GetQuizzesAsync(id);

            if (!quizzes.Any())
            {
                _logger.LogWarning("Quiz not found");
                return NotFound();
            }
            var quiz = quizzes.First();
            _logger.LogDebug("Quiz found: {@Quiz}", quiz);
            return await Task.FromResult(quiz);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuiz(int id)
        {
            var quizzes = await _quizRepository.GetQuizzesAsync(id);
            var quiz = quizzes.First();
            if (id != quiz.QuizId)
            {
                return BadRequest();
            }

            try
            {
                await _quizRepository.UpdateQuizAsync(quiz);
                _logger.LogDebug("Quiz updated: {@Quiz}", quiz);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await QuizExistsAsync(id)))
                {
                    _logger.LogWarning("Quiz not found");
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
           
            return NoContent();
        }
        [HttpPost("start")]
        public async Task<ActionResult<Quiz?>> StartQuiz(StartQuizCommand command)
        {
            return await _quizService.StartQuizAsync(command);

        }

        [HttpPost("end")]
        public async Task<ActionResult<double?>?> EndQuiz(EndQuizCommand command)
        {

            var score = await _quizService.GetQuizScore(command);
            var quizResult = await _quizRepository.GetQuizResultByIdAsync(command.QuizResultId);
            if (quizResult is not null)
            {
                quizResult.Score = score;
                quizResult.EndTime = DateTime.Now;
                quizResult.QuizIsFinished = true;
              
                
                await _quizRepository.UpdateQuizResultAsync(quizResult);
                return score;
            }
            return null;
        }
        [HttpPost]
        public async Task<ActionResult<Quiz>> PostQuiz(QuizCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var quizId = command.QuizId;
            var quizzes = await _quizRepository.GetQuizzesAsync(quizId);
            var quiz = quizzes.First();
            await _quizRepository.AddQuizAsync(quiz);

            return CreatedAtAction("GetQuiz", new { id = quiz.QuizId }, quiz);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuiz(int id)
        {
            _logger.LogInformation($"Deleted quiz with  id  = {id}");
            var quizzes = await _quizRepository.GetQuizzesAsync(id);
            var quiz = quizzes.First();
            if (quiz == null)
            {
                _logger.LogWarning("Quiz not found");
                return NotFound();
            }

            await _quizRepository.DeleteQuizAsync(id);

            return NoContent();
        }

        private async Task<bool> QuizExistsAsync(int id)
        {
            var quizList = await _quizRepository.GetQuizzesAsync(id);
            return quizList?.Any() ?? false;
        }
    }
}
