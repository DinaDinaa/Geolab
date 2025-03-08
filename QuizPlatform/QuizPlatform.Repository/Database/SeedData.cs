using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizPlatform.Domain.Models.Entities;

namespace QuizPlatform.Repository.Database
{
    public class SeedData
    {
        private readonly AppDbContext _dbContext;

        public  SeedData (AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SeedDatabase()
        {

            List<Quiz> quizzes = new List<Quiz>()
            {
                new Quiz()
            {
                QuizName = "Natural Sciences",
                IsActive=true
            },
                new Quiz()
            {
                QuizName = "Other Sciences"
            }
            };
            _dbContext.AddRange(quizzes);
            _dbContext.SaveChanges();

            List<Test> tests = new List<Test>();

            tests.Add(new Test()
            {
                TestName = "Math",
                QuestionCount = 3,
                TestTime = 3,
                IsActive = true

            });


            tests.Add(new Test()
            {
                TestName = "Literature",
                QuestionCount = 2,
                TestTime = 2,
                IsActive = false

            });

            tests.Add(new Test()
            {
                TestName = "Physics",
                QuestionCount = 4,
                TestTime = 4,
                IsActive = true

            });


            _dbContext.AddRange(tests);
            _dbContext.SaveChanges();

            var tempQuizzes = _dbContext.Quizzes.ToList();
            var tempTests  = _dbContext.Tests.ToList();
             List<QuizTestLink>  quizzesTestes = new List<QuizTestLink>();
            tempQuizzes.ForEach(q =>
            {
                tempTests.ForEach(t =>
                {
                    quizzesTestes.Add(new QuizTestLink()
                    {
                        QuizId = q.QuizId,
                        TestId = t.TestId
                    });
                });

            });

            _dbContext.AddRange(quizzesTestes);
            _dbContext.SaveChanges();

            int ind = 1;

            List<Question> listQuestions = new List<Question>();

            while (ind <= 10)
            {
                listQuestions.Add(new Question()
                {
                    TestId = 1,
                    QuestionText = "Question " + ind,
                    AllowMultipleCorrectAnswers = false,
                    Points = ind
                });

                ind++;
            }
            _dbContext.AddRange(listQuestions);
            _dbContext.SaveChanges();
            var questions = _dbContext.Questions.ToList();

            List<Answer> listAnswers = new List<Answer>();
            
            foreach (var quest in questions)
            {

                ind = 1;
                List<Answer> tempAnswers = new ();
                while (ind <= 4)
                {
                    tempAnswers.Add(new Answer()
                    {   

                         AnswerText = quest.QuestionText +" "+  "Answer"+ind,
                         QuestionId = quest.QuestionId,

                    });
                    ind++;
                }
                tempAnswers.OrderBy(a=>Guid.NewGuid()).First().IsCorrect = true;
                listAnswers.AddRange(tempAnswers);

            }
            _dbContext.AddRange(listAnswers);
            _dbContext.SaveChanges();

        }

    }
}
