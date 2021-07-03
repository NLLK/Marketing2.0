using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaMarketing2Reborn.Models
{
    public class RegisterQuestion
    {
        public string Question = "";//вопрос или ответ на вопрос в зависимости от наличия потомков
        public int Scale = 0; //шкала. -1 если у вопроса нет потомков
        public List<RegisterQuestion> Answers;//Ответы на вопрос
        public int QuestionNumber = 0;//номер вопроса (относительно анкеты или потомков)
        public int NextQuestionIfYes = 0;//если в качестве ответа был выбран этот вопрос, то перейти к другому пункту
                                         //0 - без принудительной переадресации

        public RegisterQuestion(string question, int scale, List<RegisterQuestion> answers, int questionNumber, int nextQuestionIfYes)
        {
            Question = question;
            Scale = scale;
            Answers = answers;
            QuestionNumber = questionNumber;
            NextQuestionIfYes = nextQuestionIfYes;
        }
    }
}
