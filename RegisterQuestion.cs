using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaMarketing2Reborn
{
    public class RegisterQuestion
    {
        public string Question = "";
        public int Scale = 0;
        public List<string> AnswersList = new List<string>(){ "Да", "Нет" };
        public int QuestionNumber = 0;

        public RegisterQuestion(string question, int scale, List<string> answersList, int questionNumber)
        {
            this.Question = question;
            this.Scale = scale;
            this.AnswersList = answersList;
            this.QuestionNumber = questionNumber;
        }
    }
}
