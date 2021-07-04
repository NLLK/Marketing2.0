using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaMarketing2Reborn.Models
{
    class RegisterQuestionnaire
    {
        private string PersonnelInfo = "";
        private string Note = "";
        private List<RegisterAnswer> AnswersList = new List<RegisterAnswer>();
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
        public string getNote()
        {
            return this.Note;
        }
        public void setNote(string _note)
        {
            this.Note = _note;
        }
        public List<RegisterAnswer> getAnswersList()
        {
            return this.AnswersList;
        }
        public void setAnswersList(List<RegisterAnswer> answers)
        {
            this.AnswersList = answers;
        }
    }
}
