
using QuizPlatform.Domain.Commands;


namespace QuizPlatform.Services.Abstractions
{
    public interface IQuestionService
    {

        public Task RegisterQuestionAsync(QuestionCommand command);
        public Task UnregisterQuestionAsync(QuestionCommand command);

    }
}
