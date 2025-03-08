using QuizPlatform.Domain.Exceptions;
using QuizPlatform.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizPlatform.Domain.Commands
{
    public class StartQuizCommand
    {
        public  int QuizId  { get; set; }
        public  Guid? UserId  { get; set; }
        public void Validate()
        {
            if (QuizId < 0)
                throw new ValidationException("QuizId must be nonnegative");
           
        }

    }
}
