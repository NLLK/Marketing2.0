using MegaMarketing2Reborn.SettingsSetup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WinForms = System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MegaMarketing2Reborn.Frames
{
	public partial class Settings : Page
	{
		private Excel excel;
		private Props props;

		public Settings(Excel excel, Props props)
		{
			this.excel = excel;
			this.props = props;
			InitializeComponent();
		}

		public void SaveSettings(object sender, RoutedEventArgs e)
		{
			TextBox personnelNumberTextBox = (TextBox)this.FindName("personnelNumber");
			String personnelNumber = personnelNumberTextBox.Text;

			TextBox FIOTextBox = (TextBox)this.FindName("FIO");
			String FIO = FIOTextBox.Text;

			TextBox formNameTextBox = (TextBox)this.FindName("formName");
			String formName = formNameTextBox.Text;

			props.ChangeFields(personnelNumber, FIO, formName);
		}

		public void QuitPage(object sender, RoutedEventArgs e)
		{
			NavigationService.GoBack();
		}

		public void ChooseExcelLocation(object sender, RoutedEventArgs e)
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
	}
}
