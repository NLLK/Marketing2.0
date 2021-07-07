using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaMarketing2Reborn.Models
{
    [Serializable]
    public class RegisterQuestion
    {
        public string Question { get; set; }//вопрос или ответ на вопрос в зависимости от наличия потомков
        public int Scale { get; set; } //шкала. -1 если у вопроса нет потомков
        public List<RegisterQuestion> Answers { get; set; }//Ответы на вопрос
        public int QuestionNumber { get; set; }//номер вопроса (относительно анкеты или потомков)
        public int NextQuestionIfYes { get; set; }//если в качестве ответа был выбран этот вопрос, то перейти к другому пункту
                                         //0 - без принудительной 
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
