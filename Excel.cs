using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;

namespace MegaMarketing2Reborn
{
    class Excel
    {
		private _Excel.Application app = null;
		private _Excel.Workbook workbook = null;
		private _Excel.Worksheet worksheet = null;
		private _Excel.Range workSheet_range = null;
		public void CreateExcelDoc()
		{
			CreateDoc();
		}
		public void CreateDoc()
		{
			try
			{
				app = new _Excel.Application();
				app.Visible = false;
				workbook = app.Workbooks.Add(1);
				worksheet = (_Excel.Worksheet)workbook.Sheets[1];
				workbook.SaveAs("excel.xlsx");
			}
			catch (Exception e)
			{
                Console.Write(e.ToString());
			}
			finally
			{
			}
		}
	}
}
