using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        /*public Action IncrementProgress;
        public ProgressBar Bar;
        public Button OkButton;*/
        LoadData XMLFile;
        public MainWindow()
        {
            InitializeComponent();
            XMLFile = new LoadData();
            //IncrementProgress += IncrementProgressBar;
        }

        /*private void IncrementProgressBar()
        {
            Bar.Value++;
            if (Bar.Value == Bar.Maximum)
            {
                OkButton.IsEnabled = true;
            }
        }*/

        private void ReadFileButton_Click(object sender, RoutedEventArgs e)
        {
            XMLFile.LoadDataExcel();
            /*DialogService dialogService = new DialogService();
            dialogService.OpenFileDialog();
            LoadData(dialogService);    */

        }
        private void LoadData(DialogService dialogService)
        {
            Load LoadWindow = new Load();
            LoadWindow.Show();
            try
            {               
                int start = 0;
                int count = File.ReadLines(dialogService.FilePath).Count();
                LoadWindow.LoadProgress.Maximum = count;             
                int countContent = count;
                int number = 5000;
                LoadFile loadFile = new LoadFile(new Progress(LoadWindow.LoadProgress,LoadWindow.LoadButton));

                for (int i = 0; i < 5; i++)
                {
                    FileReader fileReader = new FileReader(dialogService, start, number);
                    ThreadPool.QueueUserWorkItem(loadFile.ReadFile, fileReader);
                    start += number;
                    countContent -= number;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
