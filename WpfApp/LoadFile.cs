using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class LoadFile
    {
        //public Action IncrementProgress; 
        async public void ReadFile(object arg)
        {
            try
            {
                DbService Service = new DbService();
                List<string> informations = new List<string>();
                lock (this)
                {
                    FileReader reader = arg as FileReader;
                    
                    foreach (string information in File.ReadLines(reader.Service.FilePath).Skip(reader.Start).Take(reader.Count))
                    {
                        if (information == null)
                        {
                            break;
                        }
                        informations.Add(information);
                    }
                }
                await Service.Create(informations);

            }
            catch (Exception)
            {
                throw;
            }
        }
        /*public LoadFile(Action increment)
        {
            IncrementProgress = increment;
        }*/
    }
}
