using QuizPlatform.Domain.Commands;
using QuizPlatform.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizPlatform.Domain.Abstractions
{
    public interface IQuizService
    {
        Task<Quiz?> StartQuizAsync(StartQuizCommand command);
        Task<double?> GetQuizScore(EndQuizCommand command);
        
    }
}
