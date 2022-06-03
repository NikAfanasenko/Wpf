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
                List<People> peopleCharacters = new List<People>();
                FileReader reader = arg as FileReader;
                foreach (string characters in File.ReadLines(reader.Service.FilePath).Skip(reader.Start).Take(MAX_COUNT))
                {
                    if (characters == null)
                    {
                        break;
                    }
                    peopleCharacters.Add(new People(characters));
                }
                SaveToDB(peopleCharacters: peopleCharacters);
                reader.Flag.Release();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
        public bool SaveToDB(List<People> peopleCharacters)
        {
            try
            {
                DbPeopleContextConn context = new DbPeopleContextConn();                
                try
                {
                    context.People.AddRange(peopleCharacters);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
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
