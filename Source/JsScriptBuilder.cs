using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaMarketing2Reborn.Source
{
    class JsScriptBuilder
    {
        string mainBody = "";
        public JsScriptBuilder(){}
        public JsScriptBuilder(string filePath)
        {
            ReadFromFile(filePath);
        }
        public string ReplaceValue(string field, string newField)
        {
            mainBody = mainBody.Replace("var "+field, "var " + field+ " = "+ "\""+newField+"\"");
            return mainBody;
        }
        public string ReplaceValueWithObject(string field, string newField)
        {
            mainBody = mainBody.Replace("var " + field, "var " + field + " = " +  newField);
            return mainBody;
        }

        public void ReadFromFile(string filePath)
        {
            string mainDirectory = Environment.CurrentDirectory;
            mainBody = File.ReadAllText(filePath, System.Text.Encoding.UTF8);
        }
        public string getScript()
        {
            return mainBody;
        }
    }
}
