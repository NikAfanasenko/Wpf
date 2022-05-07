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
        private event Action IncrementProgress;

        private Progress _progress;
        async public void ReadFile(object arg)
        {
            try
            {
                List<string> informations = new List<string>();
                FileReader reader = arg as FileReader;
                    
                foreach (string information in File.ReadLines(reader.Service.FilePath).Skip(reader.Start).Take(reader.Count))
                {
                    if (information == null)
                    {
                        break;
                    }
                    informations.Add(information);
                }
                await AddRowsToDB(informations);

            }
            catch (Exception)
            {
                throw;
            }
        }
        async public Task AddRowsToDB(List<string> informations)
        {
            try
            {
                DbPeopleContextConn context = new DbPeopleContextConn();
                foreach (string data in informations)
                {
                    try
                    {
                        var human = new People(data);
                        context.People.Add(human);
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
                context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public LoadFile(Progress progress)
        {
            _progress = progress;
            IncrementProgress += ChangeProgress;
        }
        private void ChangeProgress()
        {
            _progress.LoadProgressBar.Dispatcher.Invoke(() =>
            {
                _progress.LoadProgressBar.Value++;
                if (_progress.LoadProgressBar.Value == _progress.LoadProgressBar.Maximum)
                {
                    _progress.ButtonOk.IsEnabled = true;
                }

            });
            
            
        }
        /*public LoadFile(Action increment)
        {
            IncrementProgress = increment;
        }*/
    }
}
