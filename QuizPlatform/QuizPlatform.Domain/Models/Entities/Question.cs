namespace QuizPlatform.Domain.Models.Entities
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string? QuestionText { get; set; }
        public List<Answer> Answers { get; set; }

        public int TestId { get; set; }
        public Test Test { get; set; }
        public bool AllowMultipleCorrectAnswers { get; set; }
        public int Points { get; set; }

      

    }
}
