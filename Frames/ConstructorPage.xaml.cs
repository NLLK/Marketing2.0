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
using System.Windows.Markup;
using System.Xml;
using System.IO;

namespace MegaMarketing2Reborn.Frames
{
    /// <summary>
    /// Логика взаимодействия для ConstructotPage.xaml
    /// </summary>
    public partial class ConstructorPage : Page
    {
        Excel excel;
        Props props;

        private RegisterQuestionnaire Questionnaire = new RegisterQuestionnaire();

        public ConstructorPage(Excel _excel, Props _props)
        {
            InitializeComponent();

            excel = _excel;
            props = _props;

            //первоначальная инициализация окна
            RegisterEditor.Visibility = Visibility.Hidden;
            AnswerEditor.Visibility = Visibility.Hidden;
            Questionnaire.AnswersList = new List<RegisterQuestion>();
        }

        private void OpenWebButton_Click(object sender, RoutedEventArgs e)
        {
            //excel.AddRegistersToExcel(RegisterList);
            //TODO:получить последнюю запись
            string recordId = "1";

            WebHtmlWindow window = new WebHtmlWindow(recordId, Questionnaire, excel);
            window.Show();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //QuestionnaireNameLabel.Content = "Имя анкеты: " + props.XMLFields.formName;
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

        private void QuestionButton_Click(object sender, RoutedEventArgs e)
        {
            //добавить еще кнопку и подпись для вопроса
            string prevQuestionNumber = AddParrentQuestionButtonAndLabel((Button)sender);
            //открыть редактор регистров
            RegisterEditor.Visibility = Visibility.Visible;
            //отобразить индекс вопроса
            QuestionIndexLabel.Content = prevQuestionNumber.ToString();
            //открыть поле с вводом вопроса
            AnswerEditor.Visibility = Visibility.Visible;
            //указать, что вводим вопрос
            AnswerEditorQuestionOrAnswerLabel.Content = "Текст вопроса: ";
            //радиобаттоны скрыть
            ChooseAnswerRadioButtonsDockPanel.Visibility = Visibility.Hidden;
            //добавить в объект
            Questionnaire.AnswersList.Add(new RegisterQuestion() { QuestionNumber = prevQuestionNumber });
        }
        private string AddParrentQuestionButtonAndLabel(Button sender)
        {
            //найти последнюю кнопку с вопросом
            Button lastQuestionButton = new Button();
            Label lastQuestionLabel = new Label();
            foreach (UIElement element in RegisterTreeGrid.Children)
            {
                if (Grid.GetColumn(element) == 0)
                {
                    if (Object.ReferenceEquals(element.GetType(), typeof(Button)))
                        lastQuestionButton = (Button)element;
                    if (Object.ReferenceEquals(element.GetType(), typeof(Label)))
                        lastQuestionLabel = (Label)element;
                }
            }

            //определить номер нового вопроса
            int questionNumberInt = int.Parse(lastQuestionButton.Tag.ToString()) +1;
            string questionNumber = questionNumberInt.ToString();

            //добавить строки для вопроса и кнопки
            RowDefinition labelRow = new RowDefinition();
            labelRow.Height = new GridLength(15);
            labelRow.Name = "QuestionLabelRow" + questionNumber;

            RegisterTreeGrid.RowDefinitions.Add(labelRow);

            RowDefinition buttonRow = new RowDefinition();
            buttonRow.Height = new GridLength(30);
            buttonRow.Name = "QuestionButtonRow" + questionNumber;

            RegisterTreeGrid.RowDefinitions.Add(buttonRow);

            //добавить подпись к кнопке
            Label newQuestionLabel = (Label)CopyObject(lastQuestionLabel);
            newQuestionLabel.Content = "Вопрос " + questionNumber;

            Grid.SetColumn(newQuestionLabel, 0);
            Grid.SetRow(newQuestionLabel, (questionNumberInt * 2) - 2);

            RegisterTreeGrid.Children.Add(newQuestionLabel);

            //скопировать кнопку и изменить ее свойства
            Button newQuestionButton = (Button)CopyObject(lastQuestionButton);
            newQuestionButton.Tag = questionNumber;
            newQuestionButton.Name = "QuestionButton" + questionNumber;
            newQuestionButton.Click += QuestionButton_Click;

            //удалить '+' с предыдущей кнопки кнопки
            lastQuestionButton.Content = "изм.";
            lastQuestionButton.Click -= QuestionButton_Click;
            lastQuestionButton.Click += QuestionButtonEditting_Click;
            //добавить кнопку в Grid
            Grid.SetColumn(newQuestionButton, 0);
            Grid.SetRow(newQuestionButton, (questionNumberInt*2)-1);

            RegisterTreeGrid.Children.Add(newQuestionButton);

            return (questionNumberInt-1).ToString();
        }

        private void QuestionButtonEditting_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private UIElement CopyObject(UIElement origin)
        {
            string gridXaml = XamlWriter.Save(origin);
            StringReader stringReader = new StringReader(gridXaml);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return (UIElement)XamlReader.Load(xmlReader);
        }

        private void RegistersScrollViewer_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RegisterEditor.Visibility = Visibility.Hidden;
            AnswerEditor.Visibility = Visibility.Hidden;
        }
    }
}
