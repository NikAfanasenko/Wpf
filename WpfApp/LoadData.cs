using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace WpfApp
{
    public class LoadData
    {
        public List<People> People { get; set; }

        public void LoadDataXML()
        {
            if (People == null)
            {
                MessageBox.Show("Найдите значения для экспорта!","Warning",MessageBoxButton.OK,MessageBoxImage.Warning);
                return;
            }
            var xmlFormatter = new XmlSerializer(typeof(List<People>));
            using (var file = new FileStream("People.xml",FileMode.OpenOrCreate))
            {
                xmlFormatter.Serialize(file, People);
            }
        }
        public void LoadDataExcel()
        {
            try
            {
                if (People == null)
                {
                    MessageBox.Show("Найдите значения для экспорта!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                using (ExcelHelper helper = new ExcelHelper())
                {
                    if(helper.Open(Path.Combine(Environment.CurrentDirectory, "People.xlsx")))
                    {
                        helper.Set(People);
                        helper.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
