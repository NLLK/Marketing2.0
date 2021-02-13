using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaMarketing2Reborn
{
    public class UsersRegister
    {
        public string Question = "";
        public int Scale = 0;
        public List<string> AnswersList;

        public UsersRegister(string question, int scale, List<string> answersList)
        {
            this.Question = question;
            this.Scale = scale;
            this.AnswersList = answersList;
        }
    }
}
