using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp
{
    public class LoadFile
    {
        private const int MAX_COUNT = 5000;
        public void ReadFile(object arg)
        {
            try
            {
                Thread.Sleep(1);
                List<string> informations = new List<string>();
                FileReader reader = arg as FileReader;
                foreach (string information in File.ReadLines(reader.Service.FilePath).Skip(reader.Start).Take(MAX_COUNT))
                {
                    if (information == null)
                    {
                        break;
                    }
                    informations.Add(information);
                }
                SaveToDB(informations);
                reader.Flag.Release();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public bool SaveToDB(List<string> informations)
        {
            try
            {
                DbPeopleContextConn context = new DbPeopleContextConn();
                foreach (string data in informations)
                {
                    try
                    {                        
                        context.People.Add(new People(data));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                }
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
                throw;
            }
        }
    }
}
