using QuizPlatform.Domain.Abstractions;
using QuizPlatform.Domain.Exceptions;
using QuizPlatform.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizPlatform.Domain.Commands
{
    public class EndQuizCommand : ICommand
    {
        
        public required int QuizResultId { get; set; }
        public List<int>? CheckedAnswersIds { get; set; }
        public void Validate()
        {
            if (QuizResultId < 0)
                throw new ValidationException("QuizResultId must be nonnegative");

        }
    }
}
