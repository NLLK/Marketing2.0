using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Newtonsoft.Json;

namespace MegaMarketing2Reborn
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Drawing.Point NamesAddButtonPlace;
        int NamesLastTextBoxIndex = 2;
        Button NamesDeleteButton;
        Button NamesAddNamesButton;
        List<UIElement> NamesTextBoxesList = new List<UIElement>();
        List<string> JSONParts = new List<string>();
        private Label RegisterName1;
        string questionnaireName;
        public string[] FromPyConsole = new string[2];
        public MainWindow()
        {
            InitializeComponent();
            RegisterChooseScale.SelectedIndex = 0;
        }
        private void OpenTable(object sender, RoutedEventArgs e)
        {
            FromPyConsole[0] = "22";//для отладки
            FromPyConsole[1] = "1.1\r\n1.2\r\n1.3\r\n";
            TablePresent tablePresent = new TablePresent(FromPyConsole);
            ConstructorFrame.Navigate(tablePresent);
        }
        private void RegisterAddScaleButton_Click(object sender, RoutedEventArgs e)
        {
            switch (RegisterChooseScale.SelectedIndex)
            {
                case 0:
                    {
                        RegisterName1 = new Label { Content = "Наименования:", FontSize = 14 };
                        Canvas.SetLeft(RegisterName1, 8);
                        Canvas.SetTop(RegisterName1, 192);
                        RegisterCanvas.Children.Add(RegisterName1);

                        MegaMarketing.PlaceHolderTextBox p = new MegaMarketing.PlaceHolderTextBox { Text = "Наименование 1 ", Width = 247, Height = 20, Foreground = System.Windows.Media.Brushes.Gray };
                        MegaMarketing.PlaceHolderTextBox p2 = new MegaMarketing.PlaceHolderTextBox { Text = "Наименование 2 ", Width = 247, Height = 20, Foreground = System.Windows.Media.Brushes.Gray };
                        Canvas.SetLeft(p, 10); Canvas.SetTop(p, 225);
                        Canvas.SetLeft(p2, 10); Canvas.SetTop(p2, 250);
                        if (NamesTextBoxesList.Count != 0) NamesTextBoxesList.Clear();
                        RegisterCanvas.Children.Add(p); RegisterCanvas.Children.Add(p2);
                        NamesTextBoxesList.Add(p); NamesTextBoxesList.Add(p2);
                        NamesLastTextBoxIndex = 1;
                        NamesAddNamesButton = new Button { Content = "Добавить наименование", Name = "RegisterAddNames" };
                        NamesAddNamesButton.Click += RegisterAddNames_Click;
                        Canvas.SetLeft(NamesAddNamesButton, 10);
                        Canvas.SetTop(NamesAddNamesButton, 275);
                        NamesAddButtonPlace = new System.Drawing.Point(10, 275);
                        RegisterCanvas.Children.Add(NamesAddNamesButton);
                        RegisterNamesRectangle.Height = 106;
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
                case -1:
                    break;
            }
        }

        private void NamesDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            int x = NamesAddButtonPlace.X;
            int y = NamesAddButtonPlace.Y - 25;
            NamesAddButtonPlace = new System.Drawing.Point(x, y);

            RegisterCanvas.Children.Remove(NamesAddNamesButton);
            Canvas.SetLeft(NamesAddNamesButton, x);
            Canvas.SetTop(NamesAddNamesButton, y);
            RegisterCanvas.Children.Add(NamesAddNamesButton);

            RegisterCanvas.Children.Remove(NamesDeleteButton);
            Canvas.SetLeft(NamesDeleteButton, 261);
            Canvas.SetTop(NamesDeleteButton, y - 25);
            RegisterCanvas.Children.Add(NamesDeleteButton);

            RegisterNamesRectangle.Height -= 25;

            RegisterCanvas.Children.Remove(NamesTextBoxesList[NamesLastTextBoxIndex]);
            NamesTextBoxesList.RemoveAt(NamesLastTextBoxIndex);
            NamesLastTextBoxIndex--;
        }

        private void RegisterAddNames_Click(object sender, RoutedEventArgs e)
        {
            NamesLastTextBoxIndex++;
            //TODO: имена задать

            MegaMarketing.PlaceHolderTextBox p = new MegaMarketing.PlaceHolderTextBox { Text = "Наименование " + (NamesLastTextBoxIndex + 1).ToString(), Width = 247, Height = 20, Foreground = System.Windows.Media.Brushes.Gray };
            Canvas.SetLeft(p, NamesAddButtonPlace.X); Canvas.SetTop(p, NamesAddButtonPlace.Y);
            RegisterCanvas.Children.Add(p);
            NamesTextBoxesList.Add(p);

            RegisterCanvas.Children.Remove(NamesAddNamesButton);

            NamesAddNamesButton = new Button { Content = "Добавить наименование", Name = "RegisterAddNames" };
            NamesAddNamesButton.Click += RegisterAddNames_Click;//TODO: переместить
            int x = NamesAddButtonPlace.X;
            int y = NamesAddButtonPlace.Y + 25;
            Canvas.SetLeft(NamesAddNamesButton, x); Canvas.SetTop(NamesAddNamesButton, y);
            NamesAddButtonPlace = new System.Drawing.Point(x, y);
            RegisterCanvas.Children.Add(NamesAddNamesButton);

            RegisterNamesRectangle.Height += 25;
            RegisterCanvas.Children.Remove(NamesDeleteButton);

            NamesDeleteButton = new Button { Content = "Удалить", Name = "NamesDeleteButton" };
            NamesDeleteButton.Click += NamesDeleteButton_Click;
            Canvas.SetLeft(NamesDeleteButton, 261);
            Canvas.SetTop(NamesDeleteButton, y - 25);
            RegisterCanvas.Children.Add(NamesDeleteButton);
        }

        private void RegisterAddRegister_Click(object sender, RoutedEventArgs e)
        {
            List<string> ls = new List<string>();
            foreach (UIElement el in RegisterCanvas.Children)
            {
                if (el.GetType().Name == "PlaceHolderTextBox")
                {
                    TextBox tx = (TextBox)el;
                    if (tx.Name == "RegisterAnswerText" && tx.Text == "Текст вопроса (не обязательно)") tx.Text = "Без текста";
                    ls.Add(tx.Text);
                }
            }
            for (int i = 0; i < RegisterCanvas.Children.Count; i++)
            {
                UIElement el = RegisterCanvas.Children[i];
                if (el.GetType().Name == "PlaceHolderTextBox")
                {
                    TextBox tx = (TextBox)el;
                    if (tx.Name == "RegisterAnswerText") continue;
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

            NamesTextBoxesList.Clear();
            if (RegisterName1 != null)
            {
                RegisterName1.Content = "";
            }
            RegisterNamesRectangle.Visibility = Visibility.Hidden;
            NamesAddButtonPlace = new System.Drawing.Point(10, 275);

            string str = JSONrepresent.CreateJSONPart(ls, JSONParts.Count);
            JSONParts.Add(str);

        }

        private void CreateTable(object sender, RoutedEventArgs e)
        {
            string json = JSONrepresent.CreateFinalJSON(JSONParts, questionnaireName);
            string returned = PythonRepresent.CreateTable(json);
            FromPyConsole[0] = returned.Substring(0, returned.IndexOf('\n') - 1);
            int split = returned.IndexOf('\n', 0) + 1;
            FromPyConsole[1] = returned.Substring(split, returned.Length - split);
        }
        private void StartConstructor(object sender, RoutedEventArgs e)
        {
            InputName inputName = new InputName();
            inputName.ShowDialog();
            questionnaireName = inputName.Name;
            questionnaireNameLabel.Content = "Имя анкеты: " + questionnaireName;
        }
    }
}
}
