using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizPlatform.Domain.Models.Entities
{
    public class Config
    {

        public int ConfigId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
