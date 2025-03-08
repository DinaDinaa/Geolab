using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPlatform.Domain.Models.Entities
{
    public class QuizResult
    {
        
        public int QuizResultId { get; set; }
        public int QuizId { get; set; }
        public Guid? UserId { get; set; }
        public double QuizTime { get; set; }
        public  Quiz quiz { get; set; }
        public double? Score { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string?  UsedQuestionsIds { get; set; }
        public string?  CheckedAnswerIds { get; set; }
        public bool QuizIsFinished { get; set; }
    }
}
