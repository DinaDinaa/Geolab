using Microsoft.EntityFrameworkCore;
using QuizPlatform.Domain.Abstractions;
using QuizPlatform.Domain.Commands;
using QuizPlatform.Domain.Models.Entities;
using QuizPlatform.Service.Services.Abstractions;
using QuizPlatform.Services.Abstractions;
using System;
using System.Linq;
using System.Threading;

namespace QuizPlatform.Service.Services.Implementations
{

    public class QuizService : IQuizService
    {

        public QuizService(IQuizRepository quizRepository, IUnitOfWork unitOfWork)
        {
            _quizRepository = quizRepository;
            _unitOfWork = unitOfWork;
        }

        private readonly IQuizRepository _quizRepository;

        private readonly ITestService _testService;

        private readonly IUnitOfWork _unitOfWork;

        private QuizResult _quizResult = new QuizResult();



        public QuizService(IQuizRepository quizRepository, ITestService testService, IUnitOfWork unitOfWork)
        {
            _quizRepository = quizRepository;
            _testService = testService;
            _unitOfWork = unitOfWork;

        }

        public async Task<Quiz?> StartQuizAsync(StartQuizCommand command)
        {
            var quiz = (await _quizRepository.GetQuizzesAsync(id: command.QuizId, includeActiveQuizzes: true, includeTests: true , includeQuestions:true ))?.FirstOrDefault();
            if (quiz != null)
            {
                var currentQuestionsIDs = new List<string>();
                var quizTime = quiz?.AllTests.Where(t => t.IsActive).Sum(t => t.TestTime) ?? 0;

                quiz?.AllTests?.ForEach(t =>
                                      {
                                          t.Questions = t.Questions?.OrderBy(q => Guid.NewGuid()).Take(t.QuestionCount ?? int.MaxValue).ToList();
                                          var temp = t.Questions?.Select(q => q.QuestionId.ToString()).ToList() ?? new List<string>();
                                           currentQuestionsIDs.AddRange(temp);
                                      });

                var usedQuestionIds = currentQuestionsIDs.Aggregate((id1, id2) => $"{id1}|{id2}");

                var quizResult = new QuizResult()
                {
                    QuizId = command.QuizId,
                    UserId = command.UserId,
                    StartTime = DateTime.Now,
                    UsedQuestionsIds = usedQuestionIds,
                    QuizTime = quizTime,
                };
                int quizResultId = await _quizRepository.AddQuizResultAsync(quizResult);
                quiz.CurrentQuizResultId = quizResultId;
                return quiz;
            }
            return null;
        }



        public async Task<double?> GetQuizScore(EndQuizCommand command)
        {

            var quizResult = await _quizRepository.GetQuizResultByIdAsync(command.QuizResultId);
            var endTime = DateTime.Now;
            var actualQuizTime = (endTime - quizResult.StartTime).Minutes;


            if (quizResult == null || actualQuizTime +1 > quizResult.QuizTime)
            {
                return null;
            }
            var usedQuestionsIds = quizResult.UsedQuestionsIds?.Split("|")?.Select(id => int.Parse(id)).ToList();
            var quizzes = await _quizRepository.GetQuizzesAsync(id: quizResult.QuizId, includeActiveQuizzes: true, includeTests: true, includeQuestions: true, includeCorrectAnswers: true);


            double totalPoints = 0.0;
            double correctPoints = 0.0;
            var tests = quizzes.FirstOrDefault()?.AllTests.Where(t => t.IsActive).ToList();

            tests?.ForEach(t =>
                                {
                                  t.Questions = t.Questions?.Where(q => usedQuestionsIds.Contains(q.QuestionId)).ToList();
                                }
                           );
            var checkedAnswerIds = command.CheckedAnswersIds;
            foreach (var testItem in tests)
            {
                foreach (var questionItem in testItem?.Questions ?? new List<Question>())
                {
                    totalPoints += questionItem.Points;
                    var allAnswerIds = questionItem.Answers.Select(a => a.AnswerId).ToList();
                    var correctAnswers = questionItem.Answers.Where(a => a.IsCorrect).ToList();
                    var incorrectAnswers = questionItem.Answers.Where(a => !a.IsCorrect).ToList();
                    if (!questionItem.AllowMultipleCorrectAnswers)
                    {
                        if (allAnswerIds.Count(id => checkedAnswerIds?.Contains(id) ?? false) > 1)
                        {
                            return null; 
                        }
                        if (correctAnswers.Count > 1) return null;
                        var answerId = correctAnswers?.First().AnswerId;
                        if (checkedAnswerIds?.Any(id => id == answerId) ?? false)
                        {
                            correctPoints += questionItem.Points;
                        }
                    }
                    else
                    {
                        if (!incorrectAnswers.Any(a => checkedAnswerIds?.Any(id => id == a.AnswerId) ?? false))
                        {
                            if (correctAnswers.All(ca => checkedAnswerIds?.Contains(ca.AnswerId) ?? false))
                            {
                                correctPoints += questionItem.Points;
                            }

                        }
                    }
                }

            }
            quizResult.EndTime = endTime;
            quizResult.QuizIsFinished = true;
            quizResult.CheckedAnswerIds = command.CheckedAnswersIds?.Select(id=>id.ToString()).Aggregate((id1, id2) => $"{id1}|{id2}");
            await _quizRepository.UpdateQuizResultAsync(quizResult);
           
            if (totalPoints > 0)
            {
                return correctPoints / totalPoints;
            }

            return null;
        }
    }
}


