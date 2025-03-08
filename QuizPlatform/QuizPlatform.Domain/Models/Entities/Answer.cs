using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace QuizPlatform.Domain.Models.Entities
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public bool IsCorrect { get; set; }
        public string AnswerText { get; set; }
        public bool IsChecked { get; set; }

    }
}
