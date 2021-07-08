using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MegaMarketing2Reborn.SettingsSetup
{
    public class PropsFields
    {
        public String XmlFileName = Environment.CurrentDirectory + "\\settings.xml";
        public String ExcelFilePath = Environment.CurrentDirectory;
        public String ExcelFileName = "excel";
    }

    public class XMLFields
	{
        public String personnelNumber = "";
        public String FIO = "";
        public String formName = "";
    }

    //Класс работы с настройками
    public class Props
    {
        public XMLFields XMLFields;
        public PropsFields Fields;

        public Props()
        {
            Fields = new PropsFields();
            XMLFields = new XMLFields();
            if (!File.Exists(Fields.XmlFileName))
            {
                File.Create(Fields.XmlFileName).Close();
                WriteXml();
            }
            ReadXml();
        }

        public void ChangeFields(String personnelNumber, String FIO, String formName)
		{
            XMLFields.personnelNumber = personnelNumber;
            XMLFields.FIO = FIO;
            XMLFields.formName = formName;

            WriteXml();
		}

        //Запись настроек в файл
        public void WriteXml()
        {
            XmlSerializer ser = new XmlSerializer(typeof(XMLFields));

            TextWriter writer = new StreamWriter(Fields.XmlFileName);
            ser.Serialize(writer, XMLFields);
            writer.Close();
        }
        //Чтение насроек из файла
        public void ReadXml()
        {
            if (File.Exists(Fields.XmlFileName))
            {
                XmlSerializer ser = new XmlSerializer(typeof(XMLFields));
                TextReader reader = new StreamReader(Fields.XmlFileName);
                XMLFields = ser.Deserialize(reader) as XMLFields;
                reader.Close();
            }
            else
            {
                //можно написать вывод сообщения если файла не существует
            }
        }
    }
}
