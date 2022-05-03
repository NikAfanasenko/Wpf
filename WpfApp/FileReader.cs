using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class FileReader
    {
        //public Action Increment;
        public DialogService Service { get; set; }
        public int Start { get; set; }
        public int Count { get; set; }
        
        public FileReader(DialogService dialog, int start, int count)
        {
            Service = dialog;
            Start = start;
            Count = count;
            //Increment = increment;
        }
    }
}
