using QuizPlatform.Domain.Commands;
using QuizPlatform.Domain.Models.Entities;
using QuizPlatform.Service.Services.Abstractions;
using QuizPlatform.Services.Abstractions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace QuizPlatform.Service.Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionService(IQuestionRepository questionRepository)
        {
            _questionRepository = questionRepository;

          
        }

        public async Task RegisterQuestionAsync(QuestionCommand command)
        {
            command.Validate();
            var question = new Question
            {
                QuestionId = command.QuestionId,
                QuestionText = command.QuestionText,
                Points = command.Points,
                Answers = command.Answers,

            };

            await _questionRepository.AddQuestionAsync(question);
        }

        public async Task UnregisterQuestionAsync(QuestionCommand command)
        {
            await _questionRepository.DeleteQuestionAsync(command.QuestionId);
        }

    }
}




