using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp
{
    public class FileReader
    {
        private const int MAX_ROWS = 5000;
        public DialogService Service { get; set; }
        public int Start { get; set; }
        public int Count { get; set; }
        
        public FileReader(DialogService dialog, int start)
        {
            Service = dialog;
            Start = start;
            Count = Start + MAX_ROWS;
        }
    }
}
