using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using WinForms = System.Windows.Forms;
using System.Windows.Controls;
using MegaMarketing2Reborn.Frames;
using MegaMarketing2Reborn.SettingsSetup;
using MegaMarketing2Reborn.Models;
using System.Net;

namespace MegaMarketing2Reborn.Frames
{
    /// <summary>
    /// Логика взаимодействия для ConstructotPage.xaml
    /// </summary>
    public partial class ConstructorPage : Page
    {
        Excel excel;
        Props props;

        private List<RegisterQuestion> RegisterList = new List<RegisterQuestion>();

        public ConstructorPage(Excel _excel, Props _props)
        {
            InitializeComponent();

            excel = _excel;
            props = _props;

            //первоначальная инициализация окна
            //RegisterCanvas.Visibility = Visibility.Hidden;
        }

        private void OpenWebButton_Click(object sender, RoutedEventArgs e)
        {
            excel.AddRegistersToExcel(RegisterList);
            //получить последнюю запись
            string recordId = "1";

            RegisterQuestionnaire registerQuestionnaire = new RegisterQuestionnaire();
            registerQuestionnaire.setAnswersList(RegisterList);
            registerQuestionnaire.setPersonnelInfo(props.XMLFields.personnelNumber, props.XMLFields.FIO);
            registerQuestionnaire.setQuestionnaireName(props.XMLFields.formName);

            WebHtmlWindow window = new WebHtmlWindow(recordId, registerQuestionnaire, excel);
            window.Show();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            questionnaireNameLabel.Content = "Имя анкеты: " + props.XMLFields.formName;
        }

        private void SomeFunction()
        {
            /*< Button Grid.Column = "1" Grid.Row = "3" Content = "+" Tag = "1" Margin = "15,3,15,3" />
              < Label Content = "1.1" Grid.Row = "2" Grid.Column = "1" HorizontalAlignment = "Right" Grid.RowSpan = "2" Margin = "0,-5,10,0" />*/
            /*
             <Label Content="Добавить вопрос" Grid.Row="4" Grid.Column="0" HorizontalAlignment="Center" Grid.RowSpan="2" Margin="0,-5,0,0"/>
             <Button Grid.Column="0" Grid.Row="5" Content="+" Tag="1" Margin="15,3,15,3"/>
             */
        }

    }
}
