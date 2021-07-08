using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using WinForms = System.Windows.Forms;
using System.Windows.Controls;
using MegaMarketing2Reborn.Frames;
using MegaMarketing2Reborn.SettingsSetup;

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
        private List<UIElement> NamesTextBoxesList = new List<UIElement>();
        private Label RegistersName;
        private bool RegisterShowed = false;
        string questionnaireName;
        private int questionNumber = 1;

        private Button LastAddRegisterButton;

        private List<UsersRegister> UsersRegisterList = new List<UsersRegister>();

        private Props props;
        private Excel excel;
        public MainWindow()
        {
            excel = new Excel();
            excel.CreateDoc();

            props = new Props();
            
            InitializeComponent();
            RegisterChooseScale.SelectedIndex = 0;
            RegisterCanvas.Visibility = Visibility.Hidden;

            LastAddRegisterButton = AddRegisterButton;

        }

        private void OpenTable(object sender, RoutedEventArgs e)
        {
            TablePresent table2 = new TablePresent(excel);
            this.Content = table2;
        }
        private void RegisterAddScaleButton_Click(object sender, RoutedEventArgs e)
        {
            if (RegisterShowed == false)
            {

                switch (RegisterChooseScale.SelectedIndex)
                {
                    case 0:
                        {
                            //обязательно для работающих частей
                            RegisterShowed = true;

                            RegistersName = new Label { Content = "Наименования:", FontSize = 14 };
                            Canvas.SetLeft(RegistersName, 8);
                            Canvas.SetTop(RegistersName, 192);
                            RegisterCanvas.Children.Add(RegistersName);

                            TextBox p = new TextBox
                            {
                                Style = (Style)FindResource("placeHolder"),
                                Tag = "Наименование 1 ",
                                Width = 247,
                                Height = 20,
                                Foreground = System.Windows.Media.Brushes.Gray
                            };
                            TextBox p2 = new TextBox
                            {
                                Style = (Style)FindResource("placeHolder"),
                                Tag = "Наименование 2 ",
                                Width = 247,
                                Height = 20,
                                Foreground = System.Windows.Media.Brushes.Gray
                            };
                            Canvas.SetLeft(p, 10);
                            Canvas.SetTop(p, 225);
                            Canvas.SetLeft(p2, 10);
                            Canvas.SetTop(p2, 250);
                            if (NamesTextBoxesList.Count != 0) NamesTextBoxesList.Clear();
                            RegisterCanvas.Children.Add(p);
                            RegisterCanvas.Children.Add(p2);
                            NamesTextBoxesList.Add(p);
                            NamesTextBoxesList.Add(p2);
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
            //если не показано меню для заполнения регистров
            if (!RegisterShowed) return;
            //добавление данных в список

            string questionName="";
            int scale = RegisterChooseScale.SelectedIndex;
            List<string> answersList = new List<string>();
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
                    answersList.Add(tx.Text);
                }
            }
            UsersRegister register = new UsersRegister(questionName, scale, answersList);

            //сохранение в список
            UsersRegisterList.Add(register);
            //отправка в excel. Если успешно, то закрываем лавочку
            if (excel.AddRegister(register))
            {
                questionNumber++;//+1 вопрос
                //очистка интерфейса
                RegisterShowed = false;

                RegisterCanvas.Visibility = Visibility.Hidden;
                DeleteRegisterRectangle();

                LastAddRegisterButton.Content = "изменить";
                LastAddRegisterButton.Click += AddRegisterButton_Edit;

                Button button = new Button();
                button.Tag = $"{questionNumber}";
                button.Name = $"{AddRegisterButton.Name}{questionNumber}";
                button.Content = "+";
                button.Margin = AddRegisterButton.Margin;
                button.Click += AddRegisterButton_Add;

                Grid.SetRow(button, questionNumber);
                Grid.SetColumn(button,1);
                mainGrid.Children.Add(button);
                LastAddRegisterButton = button;
            }



        }

        private void DeleteRegisterRectangle()
        {
            RegisterShowed = false;
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
            NamesAddButtonPlace = new Point(10, 275);
        }

        private void AddRegisterButton_Add(object sender, RoutedEventArgs e)
        {
            RegisterCanvas.Visibility = Visibility.Visible;

        }

        private void AddRegisterButton_Edit(object sender, RoutedEventArgs e)
        {
            //RegisterCanvas.Visibility = Visibility.Visible;

        }

        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            excel.Close();
        }

        private void RegisterChooseScale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeleteRegisterRectangle();
        }

        private void ChooseExcelLocationButton_OnClick(object sender, RoutedEventArgs e)
        {
            //TODO: работает сохранение настроек в районах одного запуска
            
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
                props.Fields.ExcelFilePath = FBD.SelectedPath;//.Replace("\\", "/");
                props.WriteXml();
                excel.SetExcelFilePath(FBD.SelectedPath);
            }


        }
    }
}
