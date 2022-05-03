using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp
{
    public class Progress
    {
        public ProgressBar LoadProgressBar { get; set; }

        public Button ButtonOk { get; set; }

        public Progress(ProgressBar bar, Button button)
        {
            LoadProgressBar = bar;
            ButtonOk = button;
        }
    }
}
