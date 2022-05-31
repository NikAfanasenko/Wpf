using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace WpfApp
{
    public class ExcelHelper : IDisposable
    {
        private Application _excel;
        private Workbook _workBook;
        private string _path;

        public ExcelHelper()
        {
            _excel = new Application();
        }

        public void Dispose()
        {
            try
            {
                _workBook.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        internal bool Open(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    _workBook = _excel.Workbooks.Open(path);
                }
                else
                {
                    _workBook = _excel.Workbooks.Add();
                    _path = path;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }  
        }

        internal void Save()
        {
            if (!string.IsNullOrEmpty(_path))
            {
                _workBook.SaveAs(_path);
                _path = null;
            }
            else
            {
                _workBook.Save();
            }
        }

        internal bool Set(List<People> data)
        {
            try
            {
                int row = 1;
                foreach (var item in data)
                {
                    ((Worksheet)_excel.ActiveSheet).Cells[row, "A"] = item.Date;
                    ((Worksheet)_excel.ActiveSheet).Cells[row, "B"] = item.Name;
                    ((Worksheet)_excel.ActiveSheet).Cells[row, "C"] = item.Surname;
                    ((Worksheet)_excel.ActiveSheet).Cells[row, "D"] = item.Patronymic;
                    ((Worksheet)_excel.ActiveSheet).Cells[row, "E"] = item.City;
                    ((Worksheet)_excel.ActiveSheet).Cells[row, "F"] = item.Country;
                    row++;
                }
                
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
