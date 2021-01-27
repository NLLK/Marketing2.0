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
        //Excel
        private _Excel.Application app = null;
        private _Excel.Workbook workbook = null;
        private _Excel.Worksheet worksheet = null;
        private _Excel.Range workSheet_range = null;

        private string excelFilePath = "excel.xlsx";
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

        public void Write(int i, int j, string text)
        {
            worksheet.Cells[i, j].Value = text;
        }

        public void WriteFormula(int i, int j, string text)
        {
            worksheet.Cells[i, j].Formula = text;
        }

        public void Close()
        {
            Save();
            workbook.Close();
            app.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
        }

        public void Save()
        {
            workbook.SaveAs(excelFilePath, ConflictResolution: _Excel.XlSaveConflictResolution.xlLocalSessionChanges);
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
                    if (inputList[i].Equals("")) Write(1, start + i, $"'{lastRegisterIndex + 1}.{i}");
                    else
                    {
                        Write(1, start + i, $"{lastRegisterIndex + 1}.{i}({inputList[i]})");
                    }
                }
            }

            lastRegisterIndex++;
            lastRegisterPlace = (start + i);
            Save();
        }
    }
}
