using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        public void LoadDataExcel(object arg)
        {
            try
            {
                FileReader reader = (FileReader)arg;
                if (People == null)
                {
                    MessageBox.Show("Найдите значения для экспорта!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                using (ExcelHelper helper = new ExcelHelper())
                {
                    if(helper.Open(path: Path.Combine(Environment.CurrentDirectory, "People.xlsx")))
                    {
                        Thread.Sleep(1);
                        helper.Set(peopleGroup: People);
                        helper.Save();
                        reader.Flag.Release();
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
