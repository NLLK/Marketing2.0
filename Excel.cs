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
using System.Windows.Forms;
using MegaMarketing2Reborn.SettingsSetup;
using DataGrid = System.Windows.Controls.DataGrid;

namespace MegaMarketing2Reborn
{
    public class Excel
    {
        //Excel
        private _Excel.Application app = null;
        private _Excel.Workbook workbook = null;
        private _Excel.Worksheet worksheet = null;
        private _Excel.Range workSheet_range = null;

        private string excelFileName = "excel.xlsx";

        //UI to Excel
        private string excelFilePath =
            new Uri(Directory.GetCurrentDirectory()/* + "/excel.xlsx"*/, UriKind.RelativeOrAbsolute).ToString();

        private bool IfFileExist = true;

        public Excel()
        {
            UpdateExcelFilePath();
        }

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
                worksheet = (_Excel.Worksheet) workbook.Sheets[1];
            }
            catch (Exception e)
            {
                Console.Write(e.ToString());
            }
        }

        public void OpenDoc()
        {
            workbook = app.Workbooks.Open(GetFullExcelFilePath());
            worksheet = (_Excel.Worksheet)workbook.Sheets[1];
        }

        public void Write(int i, int j, string text)
        {
            worksheet.Cells[i, j].Value = text;
        }

        public void Write(DataGrid dataGrid)
        {
            worksheet = (_Excel.Worksheet) workbook.Sheets.get_Item(1);
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
                int sum = 0;
                for (int j = 0; j < dataGrid.Items.Count; j++)
                {
                    string temp;
                    int temp2;
                    if (j != dataGrid.Items.Count - 1)
                    {
                        if ((dataGrid.Columns[i].GetCellContent(dataGrid.Items[j]) as TextBlock) == null ||
                            (dataGrid.Columns[i].GetCellContent(dataGrid.Items[j]) as TextBlock).Text.Equals(""))
                            worksheet.Cells[j + 2, i + 1].Value2 = "0";
                        else
                        {
                            worksheet.Cells[j + 2, i + 1].Value2 =
                                (dataGrid.Columns[i].GetCellContent(dataGrid.Items[j]) as TextBlock).Text;
                            temp = (dataGrid.Columns[i].GetCellContent(dataGrid.Items[j]) as TextBlock).Text;
                            //if (temp.Equals("")) temp2 = 0;
                            temp2 = int.Parse(temp);
                            if (temp2 == 1) sum++;
                        }
                    }
                    else
                    {
                        worksheet.Cells[j + 2, i + 1].Value2 = sum;
                    }
                }
            }

            workbook.Save();
        }

        public System.Data.DataView Read()
        {
            UpdateExcelFilePath();
            //OpenDoc();
            worksheet = (_Excel.Worksheet) workbook.Sheets.get_Item(1);
            workSheet_range = worksheet.UsedRange;
            System.Data.DataTable dt = new System.Data.DataTable();

            for (int Cnum = 1; Cnum <= workSheet_range.Columns.Count; Cnum++)
            {
                try
                {
                    string columnName = (workSheet_range.Cells[1, Cnum] as _Excel.Range).get_Value().ToString();
                    dt.Columns.Add(new DataColumn {ColumnName = columnName, DataType = typeof(int)});
                }
                catch
                {
                    return new DataView(dt);
                }


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

        public bool Save()
        {
            UpdateExcelFilePath();
            try
            {
                workbook.SaveAs(GetFullExcelFilePath(),
                    ConflictResolution: _Excel.XlSaveConflictResolution.xlLocalSessionChanges);
            }
            catch (Exception e)
            {
                MessageBox.Show(
                    "Ошибка доступа к записи. Закройте другие процессы, использующие этот файл",
                    "Ошибка",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Error,
                    MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.DefaultDesktopOnly);

                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        private void UpdateExcelFilePath()
        {
            Props props = new Props();
            //props.ReadXml();
            excelFilePath = props.Fields.ExcelFilePath;
        }

        public bool AddRegistersToExcel(List<RegisterQuestion> list)
        {

            int lastRegisterPlace = 1;
            int lastRegisterIndex = 0;

            foreach (RegisterQuestion register in list)
            {
                List<string> inputList = register.AnswersList;
                int start = lastRegisterPlace;

                if (register.Question.Equals("")) Write(1, start, $"{lastRegisterIndex + 1}");
                else
                {
                    Write(1, start, $"{lastRegisterIndex + 1}({register.Question})");
                }

                int i = 1;
                for (i = 1; i <= inputList.Count; i++)
                {
                    if (inputList[i - 1].Equals("")) Write(1, start + i, $"'{lastRegisterIndex + 1}\x2024{i}");
                    else
                    {
                        Write(1, start + i, $"{lastRegisterIndex + 1}\x2024{i}({inputList[i - 1]})");
                    }

                }

                lastRegisterIndex++;
                lastRegisterPlace = (start + i);
            }


            return Save();
        }

        public void CheckIfFileExist()
        {
            //если файл существует, то перезаписать тот, что был, единожды
            if (File.Exists(GetFullExcelFilePath()) && IfFileExist)
            {
                IfFileExist = false;
                try
                {
                    workbook.Close();
                }
                catch (System.Runtime.InteropServices.COMException e)
                {
                    Console.WriteLine(e);

                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(app);

                    app = new _Excel.Application();
                    app.Visible = false;
                    app.DisplayAlerts = false;
                }
                finally
                {
                    workbook = app.Workbooks.Add(1);
                    worksheet = (_Excel.Worksheet) workbook.Sheets[1];
                }
            }

        }

        public void SetExcelFilePath(string path)
        {
            excelFilePath = path;
        }

        public string GetFullExcelFilePath()
        {
            return excelFilePath + excelFileName;
        }
    }
}
