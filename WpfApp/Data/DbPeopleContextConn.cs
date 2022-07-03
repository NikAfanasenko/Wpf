using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class DbPeopleContextConn : DbContext
    {
        public DbPeopleContextConn() : base("DbConnect")
        {
        }
        public DbSet<People> People { get; set; }
    }
}
