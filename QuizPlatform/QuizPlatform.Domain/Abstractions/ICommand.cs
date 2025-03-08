using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizPlatform.Domain.Abstractions
{
    public interface ICommand
    {
        public abstract void Validate();
    }
}
