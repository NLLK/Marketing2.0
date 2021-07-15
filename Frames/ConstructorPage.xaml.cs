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
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Documents;

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

        private List<Button[]> TreeButtonsArray = new List<Button[]>();
        private List<Label[]> TreeLabelsArray = new List<Label[]>();

        public ConstructorPage(Excel _excel, Props _props)
        {
            InitializeComponent();

            excel = _excel;
            props = _props;

            //первоначальная инициализация окна
            RegisterEditor.Visibility = Visibility.Hidden;
            AnswerEditor.Visibility = Visibility.Hidden;


            AddButtonsAndLabelsInTreeArrays(1, 0, (Button)RegisterTreeGrid.FindName("QuestionButton1"), (Label)RegisterTreeGrid.FindName("QuestionLabel1"));


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

        private void QuestionButton_Click(object sender, RoutedEventArgs e)
        {
            //добавить еще кнопку и подпись для вопроса
            string prevQuestionNumber = AddParrentQuestionButtonAndLabel((Button)sender);
/*            //открыть редактор регистров
            RegisterEditor.Visibility = Visibility.Visible;
            //отобразить индекс вопроса
            QuestionIndexLabel.Content = prevQuestionNumber.ToString();
            //открыть поле с вводом вопроса
            AnswerEditor.Visibility = Visibility.Visible;
            //указать, что вводим вопрос
            AnswerEditorQuestionOrAnswerLabel.Content = "Текст вопроса: ";
            AnswerEditorTextBox.Tag = "Текст вопроса: ";*//*
            //радиобаттоны скрыть
            ChooseAnswerRadioButtonsDockPanel.Visibility = Visibility.Hidden;*/
            //добавить в объект
            Questionnaire.QuestionsList.Add(new RegisterQuestion() { QuestionNumber = prevQuestionNumber });
            QuestionEditting(prevQuestionNumber);
        }
        private string AddParrentQuestionButtonAndLabel(Button sender)
        {
            ClearChoosenButtons();
            //найти последнюю кнопку с вопросом
            Button lastQuestionButton = new Button();
            Label lastQuestionLabel = new Label();
            List<Button> list = new List<Button>();
            foreach (UIElement element in RegisterTreeGrid.Children)
            {
                if (Grid.GetColumn(element) == 0)
                {
                    if (Object.ReferenceEquals(element.GetType(), typeof(Button)))
                        list.Add((Button)element);
                }
            }
            foreach (Button element in getListOfButtonsFromTreeGrid())
            {
                if (Grid.GetColumn(element) == 0)
                {
                    lastQuestionButton = (Button)element;
                }
            }
            foreach (Label element in getListOfLabelsFromTreeGrid())
            {
                if (Grid.GetColumn(element) == 0)
                {
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

            int buttonRow = (questionNumberInt * 2) - 1;
            int buttonColumn = 0;

            Grid.SetColumn(newQuestionButton, buttonColumn);
            Grid.SetRow(newQuestionButton, buttonRow);

            RegisterTreeGrid.Children.Add(newQuestionButton);
            RegisterName(newQuestionButton.Name, newQuestionButton);

            AddButtonsAndLabelsInTreeArrays(buttonRow, buttonColumn, newQuestionButton, newQuestionLabel);

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

        private void AddSubquestionButton_Click(object sender, RoutedEventArgs e)
        {
            //тоже самое, что при ответе, но scale = 0
        }

        private void AddAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            ClearChoosenButtons();
            //добавить в объект новое поле //scale != 0
            string parentQuestionIndex = QuestionIndexLabel.Content.ToString();

            string[] splitted = parentQuestionIndex.Split('.');
            RegisterQuestion question = getQuestion(splitted, Questionnaire.QuestionsList);
            RegisterQuestion newAnswer = new RegisterQuestion(question);
            question.Answers.Add(newAnswer);

            //добавить в грид кнопку и надпись
            Button newButton = AddQuestionButtonAndLabel(newAnswer);
            //HighlightButton(newButton);

            //подвинуть кнопки
            MoveButtonsAndLabelsDown();
            //отрисовать линию
            RegisterTreeLinesCanvas.Children.Clear();
            foreach (RegisterQuestion registerQuestion in Questionnaire.QuestionsList)
            {
                DrawTreeLines(registerQuestion);
            }

            QuestionEditting(parentQuestionIndex);


        }

        private void DrawTreeLines(RegisterQuestion question)
        {
            Button parrentButton = (Button)RegisterTreeGrid.FindName("QuestionButton" + question.QuestionNumber.Replace('.', '_'));
            int x1 = Grid.GetColumn(parrentButton);
            int y1 = Grid.GetRow(parrentButton) / 2;

            foreach (RegisterQuestion answer in question.Answers)
            {
                Button childButton = (Button)RegisterTreeGrid.FindName("QuestionButton" + answer.QuestionNumber.Replace('.', '_'));
                int y2 = (Grid.GetRow(childButton)) / 2;

                DrawTreeLine(x1, y1, y2);
                if (answer.Answers != null)
                {
                    DrawTreeLines(answer);
                }
            }
        }

        private void DrawTreeLine(int x1, int y1, int y2)
        {
            int rowLabelHeight = 15;
            int rowButtonHeight = 30;
            float columnWidth = (float)RegisterTreeGrid.ActualWidth / 8;
            int buttonMargin = 15;

            Polyline polyLine = new Polyline() { Stroke = Brushes.Black, StrokeThickness = 2 };

            PointCollection points = new PointCollection();


            float x, y;

            x = (x1 + 1) * columnWidth - buttonMargin;
            y = (y1) * (rowLabelHeight + rowButtonHeight) + (rowLabelHeight + rowButtonHeight / 2);
            points.Add(new Point(x, y));

            x += buttonMargin;
            points.Add(new Point(x, y));

            y += (y2 - y1) * ((rowButtonHeight / 2) * 2 + rowLabelHeight);
            points.Add(new Point(x, y));

            x += buttonMargin;
            points.Add(new Point(x, y));

            polyLine.Points = points;
            RegisterTreeLinesCanvas.Children.Add(polyLine);
        }

        private Button AddQuestionButtonAndLabel(RegisterQuestion question)
        {
            //номер вопроса новой кнопки
            string questionNumber = question.QuestionNumber;
            //добавляем строк
            AddRowDefenitions(questionNumber);

            //получаем объект родительского вопроса
            string[] splitted = questionNumber.Split('.');
            Array.Resize(ref splitted, splitted.Length - 1);
            RegisterQuestion parrentQuestion = getQuestion(splitted, Questionnaire.QuestionsList);
            int numberOfAnswers = 0;
            for (int i = 0; i < parrentQuestion.Answers.Count - 1; i++)
            {
                //получаем количество ответов, которое уже есть в этом вопросе и ниже
                numberOfAnswers += getNumberOfAnswers(parrentQuestion.Answers[i]);
            }

            int column = getLevelOfQuestion(questionNumber);
            int buttonRow = getPrevQuestionButtonRow(parrentQuestion) + numberOfAnswers * 2 + 2;

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

            AddButtonsAndLabelsInTreeArrays(buttonRow, column, newQuestionButton, newQuestionLabel);

            return newQuestionButton;
        }

        private void MoveButtonsAndLabelsDown()
        {
            for (int rowNumber = 0; rowNumber < TreeButtonsArray.Count; rowNumber++)
            {
                Button[] buttons = TreeButtonsArray[rowNumber];
                int buttonCount = 0;
                int indexOfLastButton = 0;
                //количество кнопок в строке
                for (int i = 0; i < buttons.Length; i++)
                {
                    if (buttons[i] != null)
                    {
                        buttonCount++;
                        indexOfLastButton = i;
                    }
                }
                //если больше одной
                if (buttonCount != 1)
                {//для всех кнопок, кроме последней добавленной 

                    TreeButtonsArray.Insert(rowNumber + 1, new Button[8]);
                    TreeLabelsArray.Insert(rowNumber + 1, new Label[8]);
                    //MoveDownTable(rowNumber);

                    for (int i = 0; i < indexOfLastButton; i++)
                    {
                        if (buttons[i] != null)
                        {
                            TreeButtonsArray[rowNumber + 1][i] = TreeButtonsArray[rowNumber][i];
                            TreeButtonsArray[rowNumber][i] = null;

                            TreeLabelsArray[rowNumber + 1][i] = TreeLabelsArray[rowNumber][i];
                            TreeLabelsArray[rowNumber][i] = null;
                        }

                    }
                    for (int i = indexOfLastButton + 1; i < 8; i++)
                    {
                        if (buttons[i] != null)
                        {
                            TreeButtonsArray[rowNumber + 1][i] = TreeButtonsArray[rowNumber][i];
                            TreeButtonsArray[rowNumber][i] = null;

                            TreeLabelsArray[rowNumber + 1][i] = TreeLabelsArray[rowNumber][i];
                            TreeLabelsArray[rowNumber][i] = null;
                        }
                    }


                    for (int j = 0; j < TreeButtonsArray.Count; j++)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            if (TreeButtonsArray[j][i] != null)
                                Grid.SetRow(TreeButtonsArray[j][i], (j * 2) + 1);
                            if (TreeLabelsArray[j][i] != null)
                                Grid.SetRow(TreeLabelsArray[j][i], (j * 2));
                        }
                    }

                    break;
                }
            }
        }

        private void AddButtonsAndLabelsInTreeArrays(int row, int column, Button button, Label label)
        {
            row = (row - 1) / 2;
            while (TreeButtonsArray.Count - 1 < row)
            {
                TreeButtonsArray.Add(new Button[8]);
            }
            TreeButtonsArray[row][column] = button;

            while (TreeLabelsArray.Count - 1 < row)
            {
                TreeLabelsArray.Add(new Label[8]);
            }
            TreeLabelsArray[row][column] = label;

        }

        private int getPrevQuestionButtonRow(RegisterQuestion parrentQuestion)
        {
            Button questionButton = (Button)RegisterTreeGrid.FindName("QuestionButton" + parrentQuestion.QuestionNumber.Replace('.', '_'));

            return Grid.GetRow(questionButton);
        }
        private int getLevelOfQuestion(string questionNumber)
        {
            string[] levels = questionNumber.Split('.');
            return levels.Length - 1;
        }
        private int getNumberOfAnswers(RegisterQuestion parrentQuestion)
        {
            int number = 1;
            if (parrentQuestion.Answers == null)
                return number;

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
            string[] newArray = new string[array.Length - 1];
            for (int i = 1; i < array.Length; i++)
            {
                newArray[i - 1] = array[i];
            }
            return newArray;
        }

        private List<Button> getListOfButtonsFromTreeGrid()
        {
            List<Button> list = new List<Button>();
            foreach (UIElement element in RegisterTreeGrid.Children)
            {
                if (Object.ReferenceEquals(element.GetType(), typeof(Button)))
                    list.Add((Button)element);
            }
            return list;
        }
        private List<Label> getListOfLabelsFromTreeGrid()
        {
            List<Label> list = new List<Label>();
            foreach (UIElement element in RegisterTreeGrid.Children)
            {
                if (Grid.GetColumn(element) == 0)
                {
                    if (Object.ReferenceEquals(element.GetType(), typeof(Label)))
                        list.Add((Label)element);
                }
            }
            return list;
        }

        private void AddTemplateAnswerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AnswerEditorSaveButton_Click(object sender, RoutedEventArgs e)
        {   //сохранить ответ/вопрос
            string index = QuestionIndexLabel.Content.ToString();
            string[] splitted = index.Split('.');

            RegisterQuestion question = getQuestion(splitted, Questionnaire.QuestionsList);
            question.Question = AnswerEditorTextBox.Text;
        }

        private void RegistersScrollViewer_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RegisterEditor.Visibility = Visibility.Hidden;
            AnswerEditor.Visibility = Visibility.Hidden;

            AnswerEditorTextBox.Text = "";

            ClearChoosenButtons();
        }

        private void QuestionButtonEditting_Click(object sender, RoutedEventArgs e)
        {
            ClearChoosenButtons();
            QuestionEditting(((Control)sender).Tag.ToString());            
        }
        private void RegisterEditorAnswerHyperlink_Click(object sender, RoutedEventArgs e)
        {
            ClearChoosenButtons();
            QuestionEditting(((Hyperlink)sender).Tag.ToString());
        }
        private void QuestionEditting(string index)
        {
            QuestionIndexLabel.Content = index;

            RegisterEditor.Visibility = Visibility.Visible;
            AnswerEditor.Visibility = Visibility.Visible;

            Button sender =  (Button)RegisterTreeGrid.FindName("QuestionButton" + index.Replace('.', '_'));

            HighlightButton(sender);
            //записать данные об подвопросах в грид
            string[] splitted = index.Split('.');
            RegisterQuestion question = getQuestion(splitted, Questionnaire.QuestionsList);

            RegisterRelatedListGrid.Children.Clear();

            if (question.Answers != null)
            {
                while (RegisterRelatedListGrid.RowDefinitions.Count < question.Answers.Count)
                {
                    RowDefinition rowDefinition = new RowDefinition();
                    RegisterRelatedListGrid.RowDefinitions.Add(rowDefinition);
                }

                int row = 0;
                foreach (RegisterQuestion answer in question.Answers)
                {
                    Label label = new Label();
                    Grid.SetRow(label, row);

                    Run text = new Run(answer.QuestionNumber);
                    Hyperlink hyperlink = new Hyperlink(text);

                    hyperlink.Tag = answer.QuestionNumber;
                    hyperlink.Click += RegisterEditorAnswerHyperlink_Click;
                    label.Content = hyperlink;

                    RegisterRelatedListGrid.Children.Add(label);
                    row++;
                }
            }

            //указать в AnswerEditor что это вопрос
            //и поместить его туда

            if (question.Answers != null)
            {
                if (question.Answers.Count != 0)
                {
                    AnswerEditorQuestionOrAnswerLabel.Content = "Текст вопроса: ";
                    AnswerEditorTextBox.Tag = "Текст вопроса: ";
                    ChooseAnswerRadioButtonsDockPanel.Visibility = Visibility.Hidden;
                }
                else
                {
                    AnswerEditorQuestionOrAnswerLabel.Content = "Текст ответа: ";
                    AnswerEditorTextBox.Tag = "Текст ответа: ";
                    ChooseAnswerRadioButtonsDockPanel.Visibility = Visibility.Visible;
                }
            }
            AnswerEditorTextBox.Text = question.Question;


        }
        private void ClearChoosenButtons()
        {
            foreach (Button button in getListOfButtonsFromTreeGrid())
            {
                button.ClearValue(BackgroundProperty);
            }
        }

        private void HighlightButton(Button button)
        {
            button.Background = Brushes.Aquamarine;
        }

        private void RegisterTreeGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            RegisterTreeLinesCanvas.Children.Clear();
            foreach (RegisterQuestion registerQuestion in Questionnaire.QuestionsList)
            {
                if (registerQuestion.Answers != null)
                {
                    if (registerQuestion.Answers.Count != 0)
                    DrawTreeLines(registerQuestion);
                }

            }
        }


    }
}
