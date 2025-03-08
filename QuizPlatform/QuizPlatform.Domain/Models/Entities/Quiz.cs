using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPlatform.Domain.Models.Entities
{
    public class Quiz
    {
        public int QuizId { get; set; }
        [NotMapped]
        public int CurrentQuizResultId { get; set; }    
        public string QuizName { get; set; }
        [NotMapped]
        public double QuizTime { get; set; }
        public bool IsActive { get; set; }
        public List<QuizTestLink> QuizzesTests { get; set; }

        [NotMapped]
        public List<Test> AllTests { get; set; }


    }
}
