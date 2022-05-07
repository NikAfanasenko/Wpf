using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WpfApp
{
    public class LoadData
    {
        public void LoadDataXML()
        {
            
            var xmlFormatter = new XmlSerializer(typeof(List<People>));
            List<People> people = new List<People>()
            {
                new People("20160303;asdasd;asdasd;asdasd;asdasd;asdasdasdasd"),
            };
            using (var file = new FileStream("People.xml",FileMode.OpenOrCreate))
            {
                xmlFormatter.Serialize(file, people);
            }
        }
        public void LoadDataExcel()
        {
            try
            {
                using (ExcelHelper helper = new ExcelHelper())
                {
                    if(helper.Open(Path.Combine(Environment.CurrentDirectory, "People.xlsx")))
                    {
                        helper.Set("A", 1, "asdasd");
                        helper.Set("A", 2, DateTime.Now);
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
