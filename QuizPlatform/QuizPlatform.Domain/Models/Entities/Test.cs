using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace QuizPlatform.Domain.Models.Entities
{
    public class Test
    {
       
        public int TestId { get; set; }
        public string TestName { get; set; }
        public double TestTime { get; set; }
        public bool IsActive { get; set; }
        public int?  QuestionCount { get; set; }
        public virtual List<Question>?  Questions { get; set; }
        public List<QuizTestLink> QuizzesTests { get; set; }
    }
}
