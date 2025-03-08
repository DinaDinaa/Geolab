using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizPlatform.Domain.Models.Entities
{
    public class QuizTestLink
    {
        public int Id { get; set; }
        public int   QuizId { get; set; }
        public int   TestId { get; set; }
        public Quiz   Quiz { get; set; }
        public Test   Test { get; set; }
    }
}
