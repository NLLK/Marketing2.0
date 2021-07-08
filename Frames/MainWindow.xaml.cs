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
        private Props props;
        private Excel excel;

        public MainWindow()
        { 
            InitializeComponent();

            //TODO: переместить создание документа в другое место
            //создание excel документа и класса
            excel = new Excel();
            excel.CreateDoc();

            //создание файла с настройками и чтение из него
            props = new Props();

            ConstructorFrame.Navigate(new ConstructotPage(excel, props));
        }
                
        private void OnClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            excel.Close();
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
        private void OpenSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ConstructorFrame.Navigate(new Settings(props));
        }

    }
}
