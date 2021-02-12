using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MegaMarketing2Reborn.Frames
{
    /// <summary>
    /// Логика взаимодействия для TablePresent.xaml
    /// </summary>
    public partial class TablePresent : Page
    {
        private DataTable dataTable;
        public Excel excel;
        public TablePresent(Excel excel1)
        {
            this.excel = excel1;
            InitializeComponent();
        }

		private void ButtonLoad_Click(object sender, RoutedEventArgs e)
		{
            DataView dv = excel.Read();
            dataGrid.ItemsSource = dv;
            dataTable = dv.Table;
        }

		private void ButtonChange_Click(object sender, RoutedEventArgs e)
		{
            excel.Write(dataGrid);
        }
	}
}
