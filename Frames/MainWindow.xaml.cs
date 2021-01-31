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
        private Label RegisterName1;
        private bool RegisterShowed = false;
        string questionnaireName;

        private Excel excel;
        public MainWindow()
        {
            excel = new Excel();
            excel.CreateDoc();

            InitializeComponent();
            RegisterChooseScale.SelectedIndex = 0;
            RegisterCanvas.Visibility = Visibility.Hidden;
        }

        private void OpenTable(object sender, RoutedEventArgs e)
        {
            TablePresent table2 = new TablePresent();
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

                            RegisterName1 = new Label { Content = "Наименования:", FontSize = 14 };
                            Canvas.SetLeft(RegisterName1, 8);
                            Canvas.SetTop(RegisterName1, 192);
                            RegisterCanvas.Children.Add(RegisterName1);

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

            List<string> ls = new List<string>();
            foreach (UIElement el in RegisterCanvas.Children)
            {
                if (el.GetType().Name == "TextBox")
                {
                    TextBox tx = (TextBox)el;
                    ls.Add(tx.Text);
                }
            }

            //очистка интерфейса
            RegisterShowed = false;

            RegisterCanvas.Visibility = Visibility.Hidden;
            DeleteRegisterRectangle();

            //отправка в excel
            excel.AddRegister(ls);

        }

        private void DeleteRegisterRectangle()
        {
            for (int i = 0; i < RegisterCanvas.Children.Count; i++)
            {
                UIElement el = RegisterCanvas.Children[i];
                if (el.GetType().Name == "TextBox")
                {
                    TextBox tx = (TextBox)el;
                    if (tx.Name == "RegisterAnswerText") { tx.Text = ""; continue; }
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
        }

        private void AddRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterCanvas.Visibility = Visibility.Visible;

            //mainGrid
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

            Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            StartupFoldersConfigSection section = (StartupFoldersConfigSection)cfg.Sections["StartupFolders"];

            if (section.FolderItems[0].Path != "") FBD.SelectedPath = section.FolderItems[0].Path;

            if (FBD.ShowDialog() == WinForms.DialogResult.OK)
            {
                section.FolderItems[0].Path = FBD.SelectedPath;
                cfg.Save();
                excel.SetExcelFilePath(FBD.SelectedPath);
            }
        }
    }
}
