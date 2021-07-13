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
        public string PersonnelInfo { get; set; }
        public string Note { get; set; }
        public string QuestionnaireName { get; set; }
        public List<RegisterQuestion> QuestionsList { get; set; }

        public RegisterQuestionnaire()
        {
            this.PersonnelInfo = "";
            this.Note = "";
            this.QuestionnaireName = "";
            this.QuestionsList = new List<RegisterQuestion>();
        }

        public void setPersonnelInfo(string _id, string _name)
        {
            if (_id.Equals(""))
            {
                this.PersonnelInfo = _name;
            }
            else if (_name.Equals("")) this.PersonnelInfo = _id;
            else this.PersonnelInfo = _id + "_" + _name;
        }
    }
}
