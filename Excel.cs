using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Controls;
using Microsoft.Office.Interop.Excel;
using _Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace MegaMarketing2Reborn
{
    class Excel
    {
        //Excel
        private _Excel.Application app = null;
        private _Excel.Workbook workbook = null;
        private _Excel.Worksheet worksheet = null;
        private _Excel.Range workSheet_range = null;

        private string excelFileName = "excel.xlsx";
        //UI to Excel
        private int lastRegisterIndex = 0;
        private string excelFilePath = new Uri(Directory.GetCurrentDirectory() + "/", UriKind.RelativeOrAbsolute).ToString();
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
			workbook = app.Workbooks.Open(excelFilePath + excelFileName);
        }

		public void Write(int i, int j, string text)
		{
			worksheet.Cells[i, j].Value = text;
		}

        public void Write(DataGrid dataGrid)
        {
            worksheet = (_Excel.Worksheet)workbook.Sheets.get_Item(1);
			//if (((DataView)dataGrid.ItemsSource).Table.HasErrors) return;
            for (int j = 0; j < dataGrid.Columns.Count; j++)
            {
                //Range myRange = (Range)worksheet.Cells[1, j + 1];
                worksheet.Cells[1, j + 1].Font.Bold = true;
                worksheet.Columns[j + 1].ColumnWidth = 15;
                //myRange.Value2 = dataGrid.Columns[j].Header;
                worksheet.Cells[1, j + 1].Value2 = dataGrid.Columns[j].Header;
            }

            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
				for (int j = 0; j < dataGrid.Items.Count; j++)
				{
					//TextBlock b = dataGrid.Columns[i].GetCellContent(dataGrid.Items[j]) as TextBlock;
					//_Excel.Range myRange = (_Excel.Range)worksheet.Cells[j + 2, i + 1];
					//myRange.Value2 = b.Text;
					if ((dataGrid.Columns[i].GetCellContent(dataGrid.Items[j]) as TextBlock) == null || 
						(dataGrid.Columns[i].GetCellContent(dataGrid.Items[j]) as TextBlock).Equals("")) worksheet.Cells[j + 2, i + 1].Value2 = "0";
					else
					worksheet.Cells[j + 2, i + 1].Value2 = (dataGrid.Columns[i].GetCellContent(dataGrid.Items[j]) as TextBlock).Text;
                }
            }

            workbook.Save();
        }

        public System.Data.DataView Read()
		{
            worksheet = (_Excel.Worksheet)workbook.Sheets.get_Item(1);
			workSheet_range = worksheet.UsedRange;
			System.Data.DataTable dt = new System.Data.DataTable();
            for (int Cnum = 1; Cnum <= workSheet_range.Columns.Count; Cnum++)
            {
                string columnName = (workSheet_range.Cells[1, Cnum] as _Excel.Range).get_Value().ToString();

                dt.Columns.Add(
				new DataColumn{ ColumnName = columnName, DataType = typeof(int)});
			}
            for (int Rnum = 2; Rnum <= workSheet_range.Rows.Count; Rnum++)
            {
                DataRow dr = dt.NewRow();
                for (int Cnum = 1; Cnum <= workSheet_range.Columns.Count; Cnum++)
                {
                    string cell = "";
                    try
                    {
                        cell = (workSheet_range.Cells[Rnum, Cnum] as Range).get_Value().ToString();
                    }
                    catch
                    {
						cell = "0";
                    }
                    finally
                    {
                        dr[Cnum - 1] = cell;
                    }
                }
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();
            return new DataView(dt);
        }

		public void Close()
		{
			workbook.Close();
			app.Quit();
			System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
			System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
			System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
		}
        public void WriteFormula(int i, int j, string text)
        {
            worksheet.Cells[i, j].Formula = text;
        }
        
        public void Save()
        {
            workbook.SaveAs(excelFilePath + excelFileName, ConflictResolution: _Excel.XlSaveConflictResolution.xlLocalSessionChanges);
        }

        public void AddRegister(List<string> inputList)
        {
            int start = lastRegisterPlace;
            int i = 0;
            for (i = 0; i < inputList.Count; i++)
            {
                if (i == 0)
                {
                    if (inputList[i].Equals("")) Write(1, start + i, $"{lastRegisterIndex + 1}");
                    else
                    {
                        Write(1, start + i, $"{lastRegisterIndex + 1}({inputList[i]})");
                    }

                }
                else
                {
                    if (inputList[i].Equals("")) Write(1, start + i, $"'{lastRegisterIndex + 1}\x2024{i}");
                    else
                    {
                        Write(1, start + i, $"{lastRegisterIndex + 1}\x2024{i}({inputList[i]})");
                    }
                }
            }

            lastRegisterIndex++;
            lastRegisterPlace = (start + i);
            Save();
			Close();
        }

        public void SetExcelFilePath(string path)
        {
            excelFilePath = path;
        }
    }
}
