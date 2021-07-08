using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaMarketing2Reborn.Models
{
    [Serializable]
    public class RegisterQuestionnaire
    {
        private string PersonnelInfo { get; set; }
        private string Note { get; set; }
        private string QuestionnaireName { get; set; }
        private List<RegisterQuestion> AnswersList { get; set; }


        public void setPersonnelInfo(string _id, string _name)
        {
            if (_id.Equals(""))
            {
                this.PersonnelInfo = _name;
            }
            else if (_name.Equals("")) this.PersonnelInfo = _id;
            else this.PersonnelInfo = _id + "_" + _name;
        }
        public string getPersonnelInfo()
        {
            return this.PersonnelInfo;
        }
/*        public string getNote()
        {
            return this.Note;
        }
        public void setNote(string _note)
        {
            this.Note = _note;
        }*/
        public List<RegisterQuestion> getAnswersList()
        {
            return this.AnswersList;
        }
        public void setAnswersList(List<RegisterQuestion> answers)
        {
            this.AnswersList = answers;
        }
        public string getQuestionnaireName()
        {
            return this.QuestionnaireName;
        }
        public void setQuestionnaireName(string questionnaireName)
        {
            this.QuestionnaireName = questionnaireName;
        }
    }
}
