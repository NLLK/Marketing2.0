using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaMarketing2Reborn.Models
{
    class RegisterAnswer
    {
        string QuestionNumber;
        string Value;
        public RegisterAnswer(string _number, string _value)
        {
            this.QuestionNumber = _number;
            this.Value = _value;
        }
    }
}
