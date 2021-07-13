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
            //Questionnaire.QuestionsList = new List<RegisterQuestion>();
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
            AnswerEditorTextBox.Tag = "Текст вопроса: ";
            //радиобаттоны скрыть
            ChooseAnswerRadioButtonsDockPanel.Visibility = Visibility.Hidden;
            //добавить в объект
            Questionnaire.QuestionsList.Add(new RegisterQuestion() { QuestionNumber = prevQuestionNumber });
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
            int questionNumberInt = int.Parse(lastQuestionButton.Tag.ToString()) + 1;
            string questionNumber = questionNumberInt.ToString();

            //добавить строки для вопроса и кнопки
            AddRowDefenitions(questionNumber);

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
            Grid.SetRow(newQuestionButton, (questionNumberInt * 2) - 1);

            RegisterTreeGrid.Children.Add(newQuestionButton);
            RegisterName(newQuestionButton.Name, newQuestionButton);
            return (questionNumberInt - 1).ToString();
        }

        private void AddRowDefenitions(string questionNumber)
        {
            questionNumber = questionNumber.Replace('.', '_');
            //добавить строки для вопроса и кнопки
            RowDefinition labelRow = new RowDefinition();
            labelRow.Height = new GridLength(15);
            labelRow.Name = "QuestionLabelRow" + questionNumber;

            RegisterTreeGrid.RowDefinitions.Add(labelRow);

            RowDefinition buttonRow = new RowDefinition();
            buttonRow.Height = new GridLength(30);
            buttonRow.Name = "QuestionButtonRow" + questionNumber;

            RegisterTreeGrid.RowDefinitions.Add(buttonRow);
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

        private void AddSubquestionButton_Click(object sender, RoutedEventArgs e)
        {
            //тоже самое, что при ответе, но scale = 0
        }

        private void AddAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            //добавить в объект новое поле //scale != 0
            string parentQuestionIndex = QuestionIndexLabel.Content.ToString();

            string[] splitted = parentQuestionIndex.Split('.');
            RegisterQuestion question = getQuestion(splitted, Questionnaire.QuestionsList);
            RegisterQuestion newAnswer = new RegisterQuestion(question);
            question.Answers.Add(newAnswer);

            //добавить в грид кнопку и надпись
            AddQuestionButtonAndLabel(newAnswer, (Button)sender);
            //TODO: делаем это
            //подвинуть другие кнопки, если требуется
            //отрисовать линию

        }


        private void AddQuestionButtonAndLabel(RegisterQuestion question, Button senderButton)
        {
            string questionNumber = question.QuestionNumber;

            AddRowDefenitions(questionNumber);

            string[] splitted = questionNumber.Split('.');
            Array.Resize(ref splitted, splitted.Length - 1);
            RegisterQuestion parrentQuestion = getQuestion(splitted, Questionnaire.QuestionsList);
            int numberOfAnswers = getNumberOfAnswers(parrentQuestion);

            //numberOfAnswers++;

            int buttonRow = Grid.GetRow(senderButton) + numberOfAnswers * 2;
            int column = getLevelOfQuestion(questionNumber);

            buttonRow = getPrevQuestionButtonRow(column, parrentQuestion);

            //добавить подпись к кнопке
            Label newQuestionLabel = (Label)CopyObject(AnswerLabelExample);
            newQuestionLabel.Content = questionNumber;
            newQuestionLabel.Visibility = Visibility.Visible;

            Grid.SetColumn(newQuestionLabel, column);
            Grid.SetRow(newQuestionLabel, buttonRow - 1);

            RegisterTreeGrid.Children.Add(newQuestionLabel);

            Button newQuestionButton = (Button)CopyObject(QuestionButton1);
            newQuestionButton.Tag = questionNumber;
            newQuestionButton.Name = "QuestionButton" + questionNumber.Replace('.', '_');

            newQuestionButton.Click += QuestionButtonEditting_Click;

            Grid.SetColumn(newQuestionButton, getLevelOfQuestion(questionNumber));
            Grid.SetRow(newQuestionButton, buttonRow);

            RegisterTreeGrid.Children.Add(newQuestionButton);

            RegisterName(newQuestionButton.Name, newQuestionButton);

        }
        private int getPrevQuestionButtonRow(int column, RegisterQuestion parrentQuestion)
        {
            Button lastButtonInRow = null;
            foreach (UIElement element in RegisterTreeGrid.Children)
            {
                if (Object.ReferenceEquals(element.GetType(), typeof(Button)))
                {
                    int columnOfElement = Grid.GetColumn(element);
                    if (columnOfElement == column)// && ((Button)element).Tag.ToString() == parrentQuestion.QuestionNumber)
                    {
                        lastButtonInRow = (Button)element;
                    }
                }
            }
            if (lastButtonInRow == null)
            {
                Button parrentButton = (Button)RegisterTreeGrid.FindName("QuestionButton" + parrentQuestion.QuestionNumber.Replace('.', '_'));
                return Grid.GetRow(parrentButton)+2;
            }
            else
                return Grid.GetRow(lastButtonInRow) + 2;

        }
        private int getLevelOfQuestion(string questionNumber)
        {
            string[] levels = questionNumber.Split('.');
            return levels.Length - 1;
        }
        private int getNumberOfAnswers(RegisterQuestion parrentQuestion)
        {
            int number = 0;
            foreach (RegisterQuestion el in parrentQuestion.Answers)
            {
                if (el.Answers != null)
                {
                    number++;
                    number += getNumberOfAnswers(el);
                }
                else number++;

            }
            return number;
        }
        private RegisterQuestion getQuestion(string[] index, List<RegisterQuestion> parentAnswersList)
        {
            int parentQuestionNumber = int.Parse(index[0]) - 1;

            RegisterQuestion question = parentAnswersList[parentQuestionNumber];

            if (question.Answers == null)
            {
                parentAnswersList[parentQuestionNumber].Answers = new List<RegisterQuestion>();
                question.Answers = parentAnswersList[parentQuestionNumber].Answers;
            }

            if (index.Length == 1)
            {
                return question;
            }
            else
            {
                index = ResizeArray(index);
                return getQuestion(index, question.Answers);
            }
        }

        private string[] ResizeArray(string[] array)
        {
            string[] newArray = new string[array.Length-1];
            for (int i = 1; i < array.Length; i++)
            {
                newArray[i - 1] = array[i];
            }
            return newArray;
        }

        private void AddTemplateAnswerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AnswerEditorSaveButton_Click(object sender, RoutedEventArgs e)
        {
            //сохранить ответ/вопрос
        }
        private void QuestionButtonEditting_Click(object sender, RoutedEventArgs e)
        {
            QuestionIndexLabel.Content = ((Button)sender).Tag.ToString();

            RegisterEditor.Visibility = Visibility.Visible;
            AnswerEditor.Visibility = Visibility.Visible;

            //записать данные об подвопросах в грид
            //указать в AnswerEditor что это вопрос
            //поместить туда вопрос, если уже имеется
        }
    }
}
