using MegaMarketing2Reborn.SettingsSetup;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
		private Props props;
		public Settings(Props props)
		{
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
	}
}
