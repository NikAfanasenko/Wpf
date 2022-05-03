using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp
{
    public class DbService
    {
        //public Action IncrementProgeress;
        async public Task Create(List<string> informations)
        {
            try
            {
                foreach (string data in informations)
                {
                    using (var context = new DbPeopleContextConn())
                    {                        
                        var human = new People(data);
                        context.People.Add(human);
                        context.SaveChanges();
                        //IncrementProgeress.Invoke();
                    }
                }                
            }
            catch (Exception)
            {                
                throw;
            }
            
        }
        /*public DbService(Action action)
        {
            IncrementProgeress = action;
        }*/
    }
}
