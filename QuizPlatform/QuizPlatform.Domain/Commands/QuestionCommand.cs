using QuizPlatform.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using QuizPlatform.Domain.Abstractions;
using QuizPlatform.Services.Abstractions;
using ICommand = QuizPlatform.Domain.Abstractions.ICommand;
using QuizPlatform.Domain.Exceptions;


namespace QuizPlatform.Domain.Commands
{
    public class QuestionCommand : ICommand
    {
        public required int QuestionId { get; set; }
        public required string QuestionText { get; set; }
        public required List<Answer> Answers { get; set; }
        public required int Points { get; set; }


        public void Validate()
        {
            if (QuestionId < 0)
                throw new ValidationException("QuestionId must be nonnegative");
            
            if (string.IsNullOrWhiteSpace(QuestionText))
                throw new ValidationException("QuestionText must not be empty");
            
            if (QuestionText.Length < 1 || QuestionText.Length > 2000)
                throw new ValidationException("Minimum 1 and maximum 2000 symbols.");
            
            if (Answers.Count < 2 || Answers.Count > 8)
                throw new ValidationException("Minumum 2 and maximum 8 possible answers.");
            foreach (var answer in Answers)
            {
                if (answer.AnswerText.Length < 1 || answer.AnswerText.Length > 250)
                    throw new ValidationException("Minimum 1 and maximum 250 symbols.");
            }
            
            if (Points < 1 || Points > 5)
                throw new ValidationException("Minimum 1 and maximum 5 points.");
            if (Answers.Where(a=>a.IsCorrect==true).Distinct().Count() != Answers.Where(a => a.IsCorrect == true).Count())
                throw new ValidationException("Correct answer must be unique.");

            if (Answers.Where(a => a.IsCorrect == true).Count() < 1)
                throw new ValidationException("At least 1 correct answer.");

        }
    }
}