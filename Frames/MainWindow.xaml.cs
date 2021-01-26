using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Newtonsoft.Json;
using MegaMarketing2Reborn.Frames;

namespace MegaMarketing2Reborn
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point NamesAddButtonPlace;
        int NamesLastTextBoxIndex = 2;
        Button NamesDeleteButton;
        Button NamesAddNamesButton;
        List<UIElement> NamesTextBoxesList = new List<UIElement>();
        List<string> JSONParts = new List<string>();
        private Label RegisterName1;
        string questionnaireName;
        public string[] FromPyConsole = new string[2];
        private Excel excel;
        public MainWindow()
        {
            InitializeComponent();
            excel = new Excel();
            excel.CreateDoc();
			excel.Write(1, 1, "blabla");
			excel.Close();
            RegisterChooseScale.SelectedIndex = 0;
        }
        private void OpenTable(object sender, RoutedEventArgs e)
        {
            //TablePresent tablePresent = new TablePresent(FromPyConsole);
            //ConstructorFrame.Navigate(tablePresent);
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

                        TextBox p = new TextBox { Style = (Style)FindResource("placeHolder"), Tag = "Наименование 1 ", Width = 247, Height = 20, Foreground = System.Windows.Media.Brushes.Gray };
                        TextBox p2 = new TextBox { Style = (Style)FindResource("placeHolder"), Tag = "Наименование 2 ", Width = 247, Height = 20, Foreground = System.Windows.Media.Brushes.Gray };
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
                        NamesAddButtonPlace = new Point(10, 275);
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
            if (NamesLastTextBoxIndex > 1)
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


                RegisterCanvas.Children.Remove(NamesTextBoxesList[NamesLastTextBoxIndex]);
                NamesTextBoxesList.RemoveAt(NamesLastTextBoxIndex);
                NamesLastTextBoxIndex--;
            }
            if (NamesLastTextBoxIndex == 1)
            {
                RegisterCanvas.Children.Remove(NamesDeleteButton);
            }
        }

        private void RegisterAddNames_Click(object sender, RoutedEventArgs e)
        {
            NamesLastTextBoxIndex++;
            //TODO: имена задать

            TextBox p = new TextBox { Style = (Style)FindResource("placeHolder"), Tag = "Наименование " + (NamesLastTextBoxIndex + 1).ToString(), Width = 247, Height = 20, Foreground = System.Windows.Media.Brushes.Gray };
            Canvas.SetLeft(p, NamesAddButtonPlace.X); Canvas.SetTop(p, NamesAddButtonPlace.Y);
            RegisterCanvas.Children.Add(p);
            NamesTextBoxesList.Add(p);

            RegisterCanvas.Children.Remove(NamesAddNamesButton);

            NamesAddNamesButton = new Button { Content = "Добавить наименование", Name = "RegisterAddNames" };
            NamesAddNamesButton.Click += RegisterAddNames_Click;//TODO: переместить
            int x = (int)NamesAddButtonPlace.X;
            int y = (int)NamesAddButtonPlace.Y + 25;
            Canvas.SetLeft(NamesAddNamesButton, x); Canvas.SetTop(NamesAddNamesButton, y);
            NamesAddButtonPlace = new Point(x, y);
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
                if (el.GetType().Name == "TextBox")
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
            foreach (UIElement el in NamesTextBoxesList)
            {
                RegisterCanvas.Children.Remove(el);
            }
            NamesTextBoxesList.Clear();

            if (RegisterName1 != null)
            {
                RegisterName1.Content = "";
            }
            RegisterNamesRectangle.Visibility = Visibility.Hidden;
            NamesAddButtonPlace = new Point(10, 275);


            //string str = JSONrepresent.CreateJSONPart(ls, JSONParts.Count);
            //JSONParts.Add(str);

        }

        private void CreateTable(object sender, RoutedEventArgs e)
        {
            //string json = JSONrepresent.CreateFinalJSON(JSONParts, questionnaireName);
            // string returned = PythonRepresent.CreateTable(json);
            //FromPyConsole[0] = returned.Substring(0, returned.IndexOf('\n') - 1);
            // int split = returned.IndexOf('\n', 0) + 1;
            //FromPyConsole[1] = returned.Substring(split, returned.Length - split);
        }
        private void StartConstructor(object sender, RoutedEventArgs e)
        {
            // InputName inputName = new InputName();
            // inputName.ShowDialog();
            //questionnaireName = inputName.Name;
            // questionnaireNameLabel.Content = "Имя анкеты: " + questionnaireName;
        }
    }
}
