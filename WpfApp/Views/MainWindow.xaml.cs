using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;
using System.Windows.Input;
using WpfApp.ViewModel;

namespace WpfApp
{
    public partial class MainWindow : Window 
    {
        private SideMenu _sideMenu;

        public MainWindow()
        {
            InitializeComponent();            
            _sideMenu = new SideMenu(sideMenu: sideMenu);
            DataContext = new ButtonViewModel(this);            
        }
        private void Menu_Button_Click(object sender, RoutedEventArgs e)
        {
            _sideMenu.StartTimerEventHandler?.Invoke();
        }
        private void panelHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
