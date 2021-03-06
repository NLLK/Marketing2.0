﻿using System;
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
    //Класс работы с настройками
    public class Props
    {
        public PropsFields Fields;

        public Props()
        {
            Fields = new PropsFields();
            if (!File.Exists(Fields.XmlFileName))
            {
                File.Create(Fields.XmlFileName).Close();
                WriteXml();
            }
            ReadXml();
        }
        //Запись настроек в файл
        public void WriteXml()
        {
            XmlSerializer ser = new XmlSerializer(typeof(PropsFields));

            TextWriter writer = new StreamWriter(Fields.XmlFileName);
            ser.Serialize(writer, Fields);
            writer.Close();
        }
        //Чтение насроек из файла
        public void ReadXml()
        {
            if (File.Exists(Fields.XmlFileName))
            {
                XmlSerializer ser = new XmlSerializer(typeof(PropsFields));
                TextReader reader = new StreamReader(Fields.XmlFileName);
                Fields = ser.Deserialize(reader) as PropsFields;
                reader.Close();
            }
            else
            {
                //можно написать вывод сообщения если файла не существует
            }
        }
    }
}
