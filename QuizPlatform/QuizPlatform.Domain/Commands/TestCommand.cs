using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizPlatform.Domain.Abstractions;
using QuizPlatform.Domain.Exceptions;
using QuizPlatform.Domain.Models.Entities;

namespace QuizPlatform.Domain.Commands
{
    public class TestCommand : ICommand

    {
        public required List<Question> Questions { get; set; } = new List<Question>();
        public required int TestId { get; set; }
        public required string TestName { get; set; }
        public int TestTime { get; set; }
        public bool IsActive { get; set; }
        public int QuestionCount { get; set; }



        public void Validate()
        {
            if (TestId < 0)
                throw new ValidationException("TestId must be nonnegative");

            if (string.IsNullOrWhiteSpace(TestName))
                throw new ValidationException("TestName must not be empty");

            if (TestName.Length < 1 || TestName.Length > 100)
                throw new ValidationException("Minimum 1 and maximum 100 symbols.");

            if (TestTime < 1 || TestTime > 60)
                throw new ValidationException("Minimum 1 and maximum 60 minutes.");

            if (Questions?.Count > 100)
                throw new ValidationException("Maximum 100 questions.");
            foreach (Question question in Questions)
            {
                if (!question.AllowMultipleCorrectAnswers)
                {
                    if (question.Answers.Count(a => a.IsChecked) > 1)
                    {
                        throw new ValidationException("Question allows single answer");
                    }
                }
            }
        }



    }

}


