using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using MegaMarketing2Reborn.SettingsSetup;
using MegaMarketing2Reborn.Models;
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
            RegisterQuestion newRegister = new RegisterQuestion();
            Questionnaire.QuestionsList.Add(newRegister);
            string prevQuestionNumber = AddParrentQuestionButtonAndLabel((Button)sender);
            newRegister.QuestionNumber = prevQuestionNumber;

            QuestionEditting(prevQuestionNumber);

            ChooseAnswerRadioButtonsDockPanel.Visibility = Visibility.Hidden;

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
            newQuestionLabel.Name = "QuestionLabel" + questionNumber.Replace('.', '_');

            int questionMainRow = RegisterTreeGrid.RowDefinitions.Count - 2;

            Grid.SetColumn(newQuestionLabel, 0);
            Grid.SetRow(newQuestionLabel, questionMainRow - 2);

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

            int buttonRow = questionMainRow - 1;
            int buttonColumn = 0;

            Grid.SetColumn(newQuestionButton, buttonColumn);
            Grid.SetRow(newQuestionButton, buttonRow);

            RegisterTreeGrid.Children.Add(newQuestionButton);
            RegisterName(newQuestionButton.Name, newQuestionButton);
            RegisterName(newQuestionLabel.Name, newQuestionLabel);
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

        private void AddAnswerButton_Click(object sender, RoutedEventArgs e)
        {
            AnswerEditorSaveButton_Click(null, null);
            ClearChoosenButtons();
            //добавить в объект новое поле //scale != 0
            string parentQuestionIndex = QuestionIndexLabel.Content.ToString();

            string[] splitted = parentQuestionIndex.Split('.');
            RegisterQuestion question = getQuestion(splitted, Questionnaire.QuestionsList);
            RegisterQuestion newAnswer = new RegisterQuestion(question);

            //добавить в грид кнопку и надпись
            Button newButton = AddQuestionButtonAndLabel(newAnswer);
            question.Answers.Add(newAnswer);

            //подвинуть кнопки
            MoveButtonsAndLabelsDown();
            //отрисовать линию
            RegisterTreeLinesCanvas.Children.Clear();
            foreach (RegisterQuestion registerQuestion in Questionnaire.QuestionsList)
            {
                DrawTreeLines(registerQuestion);
            }

            QuestionEditting(newAnswer.QuestionNumber);


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
            for (int i = 0; i < parrentQuestion.Answers.Count; i++)
            {
                //получаем количество ответов, которое уже есть в этом вопросе и ниже
                numberOfAnswers += getNumberOfAnswers(parrentQuestion.Answers[i]);
            }
            numberOfAnswers++;

            int column = getLevelOfQuestion(questionNumber);
            int buttonRow = getPrevQuestionButtonRow(parrentQuestion) + numberOfAnswers * 2;

            //добавить подпись к кнопке
            Label newQuestionLabel = (Label)CopyObject(AnswerLabelExample);
            newQuestionLabel.Name = "QuestionLabel" + question.QuestionNumber.Replace('.', '_');
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

            RegisterName(newQuestionLabel.Name, newQuestionLabel);
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

        private void MoveButtonsAndLabelsUp(int emptyRow)
        {
            RegisterTreeGrid.RowDefinitions.RemoveAt(emptyRow * 2);
            RegisterTreeGrid.RowDefinitions.RemoveAt(emptyRow * 2);
            for (int j = 0; j < TreeButtonsArray.Count; j++)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (TreeButtonsArray[j][i] != null)
                        Grid.SetRow(TreeButtonsArray[j][i], (j * 2) + 1);
                    if (TreeLabelsArray[j][i] != null)
                        Grid.SetRow(TreeLabelsArray[j][i], j * 2);
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
            if (parrentQuestion.Answers != null)
                foreach (RegisterQuestion answer in parrentQuestion.Answers)
                    number += getNumberOfAnswers(answer);
            else
                return number;
            return number;
        }
        private int getNumberOfAnswers(List<RegisterQuestion> parrentQuestions)
        {
            int commonNumber = 0;
            foreach (RegisterQuestion parrentQuestion in parrentQuestions)
            {
                int number = 1;
                if (parrentQuestion.Answers == null || parrentQuestion.Answers.Count == 0)
                {
                    commonNumber += number;
                    continue;
                }

                foreach (RegisterQuestion el in parrentQuestion.Answers)
                {
                    if (el.Answers != null)
                    {
                        number++;
                        number += getNumberOfAnswers(el);
                    }
                    else number++;
                }
                commonNumber += number;
            }
            return commonNumber;
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
                return question;
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

            if (splitted.Length != 1)
            {
                RegisterQuestion question = getQuestion(splitted, Questionnaire.QuestionsList);
                if (!index.Equals("0"))
                {
                    if (question.Question.Equals("") || question.Question.Equals("Не указано"))
                        question.Question = AnswerEditorTextBox.Text;
                }
                if (ChooseAnswerTypeName.IsChecked == true)
                {
                    question.Scale = 0;
                }
                else if (ChooseAnswerTypeNameComplicated.IsChecked == true)
                {
                    question.Scale = 1;
                }
                else if (ChooseAnswerTypeNameInteravls.IsChecked == true)
                {
                    question.Scale = 2;
                }
            }


        }

        private void RegistersScrollViewer_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AnswerEditorSaveButton_Click(null, null);
            RegisterEditor.Visibility = Visibility.Hidden;
            AnswerEditor.Visibility = Visibility.Hidden;

            AnswerEditorTextBox.Text = "";

            ClearChoosenButtons();
        }

        private void QuestionButtonEditting_Click(object sender, RoutedEventArgs e)
        {
            AnswerEditorSaveButton_Click(null, null);
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

            Button sender = (Button)RegisterTreeGrid.FindName("QuestionButton" + index.Replace('.', '_'));

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
            if (!question.Question.Equals("Не указано"))
                AnswerEditorTextBox.Text = question.Question;
            else
                AnswerEditorTextBox.Text = "";

            switch (question.Scale)
            {
                case 0:
                    ChooseAnswerTypeName.IsChecked = true;
                    ChooseAnswerTypeNameComplicated.IsChecked = false;
                    ChooseAnswerTypeNameInteravls.IsChecked = false;
                    break;
                case 1:
                    ChooseAnswerTypeName.IsChecked = false;
                    ChooseAnswerTypeNameComplicated.IsChecked = true;
                    ChooseAnswerTypeNameInteravls.IsChecked = false;
                    break;
                case 2:
                    ChooseAnswerTypeName.IsChecked = false;
                    ChooseAnswerTypeNameComplicated.IsChecked = false;
                    ChooseAnswerTypeNameInteravls.IsChecked = true;
                    break;
                default:
                    ChooseAnswerTypeName.IsChecked = true;
                    ChooseAnswerTypeNameComplicated.IsChecked = false;
                    ChooseAnswerTypeNameInteravls.IsChecked = false;
                    break;
            }


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

        private void DeleteRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить этот регистр?",
                                "Удалить регистр",
                                MessageBoxButton.YesNo,
                                MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                string index = QuestionIndexLabel.Content.ToString();
                string[] splitted = index.Split('.');

                RegisterQuestion question = getQuestion(splitted, Questionnaire.QuestionsList);
                DeleteQuestionButtons(question);

                Array.Resize(ref splitted, splitted.Length - 1);
                RegisterQuestion parrentQuestion;
                if (splitted.Length != 0)
                    parrentQuestion = getQuestion(splitted, Questionnaire.QuestionsList);
                else
                {
                    //удаляется коренной вопрос
                    int questionIndex = int.Parse(index) - 1;
                    Questionnaire.QuestionsList.RemoveAt(questionIndex);

                    for (int i = questionIndex; i < Questionnaire.QuestionsList.Count; i++)
                    {
                        RegisterQuestion register = Questionnaire.QuestionsList[i];
                        string newNumber = (i + 1).ToString();

                        RenameQuestionButtonAndLabel(register, newNumber);

                        RenameParentQuestionButtonsAndLabels(register);
                    }

                    RegisterQuestion lastParentQuestion = new RegisterQuestion();
                    lastParentQuestion.QuestionNumber = (Questionnaire.QuestionsList.Count + 2).ToString();

                    RenameQuestionButtonAndLabel(lastParentQuestion, (Questionnaire.QuestionsList.Count + 1).ToString());

                    return;
                }

                if (parrentQuestion.Answers.IndexOf(question) == parrentQuestion.Answers.Count - 1)
                {
                    parrentQuestion.Answers.Remove(question);
                }
                else
                {

                    int deletingQuestionIndex = parrentQuestion.Answers.IndexOf(question);
                    parrentQuestion.Answers.Remove(question);
                    for (int i = deletingQuestionIndex; i < parrentQuestion.Answers.Count; i++)
                    {
                        RegisterQuestion answer = parrentQuestion.Answers[i];

                        string[] splittedNumber = answer.QuestionNumber.Split('.');

                        int answerNumber = int.Parse(splittedNumber[splittedNumber.Length - 1]) - 1;

                        Array.Resize(ref splittedNumber, splittedNumber.Length - 1);

                        string newNumber = "";

                        foreach (string str in splittedNumber)
                        {
                            newNumber += str + ".";
                        }
                        newNumber += answerNumber.ToString();

                        RenameQuestionButtonAndLabel(answer, newNumber);

                    }

                }

                RegisterTreeGrid_SizeChanged(null, null);
                if (parrentQuestion.Answers.Count != 0)
                    QuestionEditting(parrentQuestion.Answers[parrentQuestion.Answers.Count - 1].QuestionNumber);
                else
                {
                    if (splitted.Length != 0)
                        QuestionIndexLabel.Content = splitted[0];

                    RegisterEditor.Visibility = Visibility.Hidden;
                    AnswerEditor.Visibility = Visibility.Hidden;
                }


            }
        }

        public void RenameParentQuestionButtonsAndLabels(RegisterQuestion question)
        {
            int newIndex = 1;
            foreach (RegisterQuestion answer in question.Answers)
            {
                if (answer.Answers.Count == 0)
                {
                    string newNumber = $"{question.QuestionNumber}.{newIndex}";
                    RenameQuestionButtonAndLabel(answer, newNumber);
                }
                else 
                {
                    RenameQuestionButtonAndLabel(answer, $"{question.QuestionNumber}.{newIndex}");
                    RenameParentQuestionButtonsAndLabels(answer);
                }
                newIndex++;
            }
        }

        private void RenameQuestionButtonAndLabel(RegisterQuestion question, string newNumber)
        {
            Button button = (Button)RegisterTreeGrid.FindName("QuestionButton" + question.QuestionNumber.Replace('.', '_'));
            Label label = (Label)RegisterTreeGrid.FindName("QuestionLabel" + question.QuestionNumber.Replace('.', '_'));

            button.Tag = newNumber;
            if (newNumber.Split('.').Length == 1)
            {
                label.Content = "Вопрос " + newNumber;
            }
            else
                label.Content = newNumber;

            UnregisterName(button.Name);
            UnregisterName(label.Name);

            button.Name = "QuestionButton" + newNumber.Replace('.', '_');
            label.Name = "QuestionLabel" + newNumber.Replace('.', '_');

            RegisterName(button.Name, button);
            RegisterName(label.Name, label);

            question.QuestionNumber = newNumber;
        }

        private void DeleteQuestionButtons(RegisterQuestion question)
        {
            if (question.Answers.Count == 0)
            {
                DeleteQuestionButton(question);
            }
            else
            {
                foreach (RegisterQuestion answer in question.Answers)
                {
                    DeleteQuestionButtons(answer);
                }
                DeleteQuestionButton(question);
            }

        }
        private void DeleteQuestionButton(RegisterQuestion question)
        {
            Button button = (Button)RegisterTreeGrid.FindName("QuestionButton" + question.QuestionNumber.Replace('.', '_'));
            Label label = (Label)RegisterTreeGrid.FindName("QuestionLabel" + question.QuestionNumber.Replace('.', '_'));

            int row = Grid.GetRow(button) / 2;

            TreeButtonsArray.RemoveAt(row);
            TreeLabelsArray.RemoveAt(row);

            UnregisterName(button.Name);
            UnregisterName(label.Name);

            RegisterTreeGrid.Children.Remove(button);
            RegisterTreeGrid.Children.Remove(label);

            MoveButtonsAndLabelsUp(row);
        }
    }
}
