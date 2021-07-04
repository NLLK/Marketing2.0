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

namespace MegaMarketing2Reborn
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point NamesAddButtonPlace;
        int NamesLastTextBoxIndex = 0;
        Button NamesDeleteButton;
        Button NamesAddNamesButton;
        private List<UIElement> NamesTextBoxesList = new List<UIElement>();

        private Label RegistersName;//название регистра
        //string questionnaireName;//название анкеты
        private int questionNumber = 1;//номер последнего вопроса
        private bool RegisterEditing = false;

        private Button LastAddRegisterButton;//последняя кнопка "добавить регистр" слева, которая будет отображаться плюсиком

        private List<RegisterQuestion> RegisterList = new List<RegisterQuestion>();

        private Props props;
        private Excel excel;
        public MainWindow()
        { 
            //TODO: переместить создание документа в другое место
            //создание excel документа и класса
            excel = new Excel();
            //excel.CreateDoc();

            //создание файла с настройками и чтение из него
            props = new Props();

            InitializeComponent();
            //первоначальная инициализация окна
            RegisterChooseScale.SelectedIndex = 0;
            RegisterNamesRectangle.Height = 56;
            RegisterCanvas.Visibility = Visibility.Hidden;

            LastAddRegisterButton = AddRegisterButton1;

        }

        private void OpenTable(object sender, RoutedEventArgs e)
        {
            //отправка в excel. Если успешно, то открываем таблицу
            if (RegisterList.Count == 0)
            {
                excel.OpenDoc();
                TablePresent table2 = new TablePresent(excel);
                this.Content = table2;
            }
            else if (excel.AddRegistersToExcel(RegisterList))
            {
                TablePresent table2 = new TablePresent(excel);
                this.Content = table2;
            }
        }
        private void RegisterAddScaleButton_Click(object sender, RoutedEventArgs e)
        {
            if (RegisterNamesRectangle.Visibility == Visibility.Hidden)
            {
                switch (RegisterChooseScale.SelectedIndex)
                {
                    case 0:
                        {
                            //обязательно для работающих частей
                            RegisterNamesRectangle.Visibility = Visibility.Visible;
                            //
                            RegistersName = new Label { Content = "Наименования:", FontSize = 14 };
                            Canvas.SetLeft(RegistersName, 8);
                            Canvas.SetTop(RegistersName, 192);
                            RegisterCanvas.Children.Add(RegistersName);
                            //добавление полей ответов
                            NamesAddButtonPlace = new Point(10, 225);
                            for (int i = 1; i <= 2; i++)
                            {
                                AddTextboxToRegister($"Наименование {i}", "", NamesTextBoxesList, 10, 200 + 25 * i);
                            }
                            RegisterNamesRectangle.Visibility = Visibility.Visible;

                            break;
                        }
                    case 1:
                        {

                            break;
                        }
                    case 2:
                        {

                            break;
                        }
                    case 3:
                        {

                            break;
                        }
                }
            }
        }

        private void NamesDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (NamesLastTextBoxIndex > 2)
            {
                int x = (int)NamesAddButtonPlace.X;
                int y = (int)NamesAddButtonPlace.Y - 25;
                NamesAddButtonPlace = new Point(x, y);

                RegisterCanvas.Children.Remove(NamesAddNamesButton);
                Canvas.SetLeft(NamesAddNamesButton, x);
                Canvas.SetTop(NamesAddNamesButton, y);
                RegisterCanvas.Children.Add(NamesAddNamesButton);

                RegisterCanvas.Children.Remove(NamesDeleteButton);
                Canvas.SetLeft(NamesDeleteButton, 261);
                Canvas.SetTop(NamesDeleteButton, y - 25);
                RegisterCanvas.Children.Add(NamesDeleteButton);

                RegisterNamesRectangle.Height -= 25;


                RegisterCanvas.Children.Remove(NamesTextBoxesList[NamesLastTextBoxIndex - 1]);
                NamesTextBoxesList.RemoveAt(NamesLastTextBoxIndex - 1);
                NamesLastTextBoxIndex--;
            }
            if (NamesLastTextBoxIndex == 2)
            {
                RegisterCanvas.Children.Remove(NamesDeleteButton);
            }
        }

        private void RegisterAddNames_Click(object sender, RoutedEventArgs e)
        {
            AddTextboxToRegister($"Наименование {NamesLastTextBoxIndex + 1}", "", NamesTextBoxesList, NamesAddButtonPlace.X, NamesAddButtonPlace.Y);
        }

        private void AddTextboxToRegister(string name, string text, List<UIElement> list, double x, double y)
        {
            NamesLastTextBoxIndex++;
            TextBox p = new TextBox
            {
                Style = (Style)FindResource("placeHolder"),
                Tag = name,
                Width = 247,
                Height = 20,
                Foreground = System.Windows.Media.Brushes.Gray,
                Text = text
            };
            Canvas.SetLeft(p, x); Canvas.SetTop(p, y);
            RegisterCanvas.Children.Add(p);
            list.Add(p);

            RegisterCanvas.Children.Remove(NamesAddNamesButton);//удалили предыдущую кнопку "добавить"

            NamesAddNamesButton = new Button { Content = "Добавить наименование", Name = "RegisterAddNames" };
            NamesAddNamesButton.Click += RegisterAddNames_Click;//TODO: переместить
            int xAdd = (int)NamesAddButtonPlace.X;
            int yAdd = (int)NamesAddButtonPlace.Y + 25;
            Canvas.SetLeft(NamesAddNamesButton, xAdd); Canvas.SetTop(NamesAddNamesButton, yAdd);
            NamesAddButtonPlace = new Point(xAdd, yAdd);
            RegisterCanvas.Children.Add(NamesAddNamesButton);

            RegisterNamesRectangle.Height += 25;

            if (NamesLastTextBoxIndex > 2)
            {
                RegisterCanvas.Children.Remove(NamesDeleteButton);//удалили предыдущую кнопку "удалить"

                NamesDeleteButton = new Button { Content = "Удалить", Name = "NamesDeleteButton" };
                NamesDeleteButton.Click += NamesDeleteButton_Click;
                Canvas.SetLeft(NamesDeleteButton, 261);
                Canvas.SetTop(NamesDeleteButton, yAdd - 25);
                RegisterCanvas.Children.Add(NamesDeleteButton);
            }

        }

        private void RegisterAddRegister_Click(object sender, RoutedEventArgs e)
        {
            //если не показано меню для заполнения регистров
            if (RegisterNamesRectangle.Visibility == Visibility.Hidden) return;
            //добавление данных из формы
            string questionName = "";
            int scale = RegisterChooseScale.SelectedIndex;
            List<RegisterQuestion> answers = new List<RegisterQuestion>();
            int iterator = 0;
            foreach (UIElement el in RegisterCanvas.Children)
            {
                if (el.GetType().Name == "TextBox")
                {
                    TextBox tx = (TextBox)el;
                    if (tx.Name == "RegisterQuestionText")
                    {
                        questionName = tx.Text;
                        continue;
                    }
                    RegisterQuestion register = new RegisterQuestion(tx.Text, -1, null, iterator, 0);
                    iterator++;
                    answers.Add(register);
                }

            }

            if (!RegisterEditing)
            {//добавление регистра
                RegisterQuestion register = new RegisterQuestion(questionName, scale, answers, questionNumber,0);
                //сохранение в список
                RegisterList.Add(register);

                questionNumber++;//+1 вопрос

                //очистка интерфейса, изменение и добавление кнопок
                
                LastAddRegisterButton.Content = "изменить";

                mainGrid.RowDefinitions.Add(new RowDefinition { MinHeight = 30 });

                Button button = new Button();
                button.Tag = $"{questionNumber}";
                button.Name = $"{AddRegisterButton1.Name}{questionNumber}";
                button.Content = "+";
                button.Margin = AddRegisterButton1.Margin;
                button.Click += AddRegisterButton_Click;

                Grid.SetRow(button, questionNumber);
                Grid.SetColumn(button, 1);
                mainGrid.Children.Add(button);
                LastAddRegisterButton = button;
            }
            else
            {//изменение регистра
                int questionNumberFromLabel = int.Parse(RegisterQuestionNumber.Content.ToString());

                RegisterQuestion register = new RegisterQuestion(questionName, scale, answers, questionNumberFromLabel, 0);
                //изменение списка
                RegisterList[questionNumberFromLabel - 1] = register;
            }
            //очистка интерфейса
            RegisterNamesRectangle.Visibility = Visibility.Hidden;

            RegisterCanvas.Visibility = Visibility.Hidden;
            ResetRegisterRectangle();

        }
        private void ResetRegisterRectangle()
        {
            RegisterNamesRectangle.Visibility = Visibility.Hidden;
            for (int i = 0; i < RegisterCanvas.Children.Count; i++)
            {
                UIElement el = RegisterCanvas.Children[i];
                if (el.GetType().Name == "TextBox")
                {
                    TextBox tx = (TextBox)el;
                    if (tx.Name == "RegisterQuestionText") { tx.Text = ""; continue; }
                    else
                    {
                        RegisterCanvas.Children.Remove(el);
                        i--;
                    }
                }
                if (el.GetType().Name == "Button")
                {
                    Button tx = (Button)el;
                    if (tx.Name == "RegisterAddScaleButton" || tx.Name == "RegisterAddRegister") continue;
                    else
                    {
                        RegisterCanvas.Children.Remove(el);
                        i--;
                    }
                }
            }
            foreach (UIElement el in NamesTextBoxesList)
            {
                RegisterCanvas.Children.Remove(el);
            }
            NamesTextBoxesList.Clear();

            if (RegistersName != null)
            {
                RegistersName.Content = "";
            }
            RegisterNamesRectangle.Visibility = Visibility.Hidden;
            RegisterNamesRectangle.Height = 56;
            NamesLastTextBoxIndex = 0;
            NamesAddButtonPlace = new Point(10, 225);
        }

        private void AddRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (((Button) sender).Content.Equals("изменить"))
            {//по нажатию клавиши "+" слева в состоянии "изменить"
                RegisterEditing = true;

                ResetRegisterRectangle();

                RegisterTopLabel.Content = "Изменение регистра";
                RegisterAddRegister.Content = "Изменить регистр";

                Button buttonSender = (Button)sender;
                int registerNumber = int.Parse(buttonSender.Tag.ToString());
                RegisterQuestion register = RegisterList[registerNumber - 1];
                RegisterQuestionText.Tag = "Текст вопроса:";
                TextBox tb = new TextBox();
                RegisterQuestionText.FontStyle = tb.FontStyle;//TODO: wtf, но оно работает
                RegisterQuestionText.Text = register.Question;//изменение текста вопроса
                RegisterQuestionNumber.Content = register.QuestionNumber;//изменение номера вопроса

                RegisterChooseScale.SelectedIndex = register.Scale;
                RegisterNamesRectangle.Visibility = Visibility.Visible;

                for (int i = 1; i <= register.Answers.Count; i++)
                {
                    AddTextboxToRegister($"Наименование {i}", register.Answers[i - 1].Question, NamesTextBoxesList, 10, 200 + 25 * i);
                }

                NamesLastTextBoxIndex = register.Answers.Count;
                //
                RegistersName = new Label { Content = "Наименования:", FontSize = 14 };
                Canvas.SetLeft(RegistersName, 8);
                Canvas.SetTop(RegistersName, 192);
                RegisterCanvas.Children.Add(RegistersName);
            }
            else
            {//по нажатию клавиши "+" слева в состоянии "добавить"

                ResetRegisterRectangle();
                
                RegisterEditing = false;
                RegisterTopLabel.Content = "Добавление регистров";
                RegisterAddRegister.Content = "Добавить регистр";
                RegisterQuestionNumber.Content = questionNumber;//изменение номера вопроса
            }

            RegisterCanvas.Visibility = Visibility.Visible;

        }
        
        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            excel.Close();
        }

        private void RegisterChooseScale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ResetRegisterRectangle();
        }

        private void ChooseExcelLocationButton_OnClick(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog FBD = new WinForms.FolderBrowserDialog();
            FBD.ShowNewFolderButton = true;
            FBD.Description = "Выберите путь файла Excel...";
            props.ReadXml();
            if (props.Fields.ExcelFilePath != Environment.CurrentDirectory)
            {
                FBD.SelectedPath = props.Fields.ExcelFilePath;
            }

            if (FBD.ShowDialog() == WinForms.DialogResult.OK)
            {
                props.Fields.ExcelFilePath = FBD.SelectedPath;
                props.WriteXml();
                excel.SetExcelFilePath(FBD.SelectedPath);
            }


        }
        private void OpenWebButton_Click(object sender, RoutedEventArgs e)
        {
            WebWorking webWorking = new WebWorking();
            var temp = webWorking.PostQuestionListToAPIAsync(RegisterList);
        }
    }
}
