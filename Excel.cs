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
			}
			catch (Exception e)
			{
                Console.Write(e.ToString());
			}
			finally
			{
			}
		}

		public void Write(int i, int j, string text)
		{
			worksheet.Cells[i, j].Value = text;
		}

		public void Close()
		{
			workbook.SaveAs("excel.xlsx");
			workbook.Close();
			app.Quit();
		}
	}
}
