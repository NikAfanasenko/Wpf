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

        internal bool Set(string column, int row, object data)
        {
            try
            {
                ((Worksheet)_excel.ActiveSheet).Cells[row, column] = data;
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
