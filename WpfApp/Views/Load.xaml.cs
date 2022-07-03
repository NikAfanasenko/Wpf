using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp.ViewModel;

namespace WpfApp
{
    public partial class Load : Window
    {        
        public Load()
        {
            InitializeComponent();
            DataContext = new ButtonViewModel();
        }

        private void ReturnFormClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
        
    }
}
