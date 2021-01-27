using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;

namespace MegaMarketing2Reborn
{
    class Excel
    {
        //Excel
		private _Excel.Application app = null;
		private _Excel.Workbook workbook = null;
		private _Excel.Worksheet worksheet = null;
		private _Excel.Range workSheet_range = null;
        //UI to Excel
        private int lastRegisterIndex = 0;
        private int lastRegisterPlace = 1;

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
				app.DisplayAlerts = false;
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

		public void OpenDoc()
		{
			app = new _Excel.Application();
			app.Visible = false;
			workbook = app.Workbooks.Open("excel.xlsx");
		}

		public void Write(int i, int j, string text)
		{
			worksheet.Cells[i, j].Value = text;
		}

		public System.Data.DataView Read()
		{
			worksheet = (_Excel.Worksheet)workbook.Sheets.get_Item(1);
			workSheet_range = worksheet.UsedRange;
			System.Data.DataTable dt = new System.Data.DataTable();
			for (int Cnum = 1; Cnum <= workSheet_range.Columns.Count; Cnum++)
			{
				dt.Columns.Add(
				new DataColumn((workSheet_range.Cells[1, Cnum] as _Excel.Range).Value2.ToString()));
			}
			for (int Rnum = 2; Rnum <= workSheet_range.Rows.Count; Rnum++)
			{
				DataRow dr = dt.NewRow();
				for (int Cnum = 1; Cnum <= workSheet_range.Columns.Count; Cnum++)
				{
					if ((workSheet_range.Cells[Rnum, Cnum] as _Excel.Range).Value2 != null)
					{
						dr[Cnum - 1] =
						(workSheet_range.Cells[Rnum, Cnum] as _Excel.Range).Value2.ToString();
					}
				}
				dt.Rows.Add(dr);
				dt.AcceptChanges();
			}
			DataView dv = new DataView(dt);
			return dv;
		}

		public void Save()
		{
			workbook.SaveAs("excel.xlsx");
		}

		public void Close()
		{
			workbook.Close();
			app.Quit();
			System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
			System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
			System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
		}

        public void AddRegister(List<string> inputList)
        {
            int start = lastRegisterPlace;
            for (int i = 0; i < inputList.Count; i++)
            {
                if (i == 0)
                {
                    Write(1, start + i, $"{lastRegisterIndex + 1}({inputList[i]})");
                }
                else
                {
                    Write(1, start + i, $"{lastRegisterIndex + 1}.{i}({inputList[i]})");
                }
            }
        }
	}
}
