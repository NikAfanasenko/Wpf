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
        public MainWindow()
        {
            InitializeComponent();
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
            DialogService dialogService = new DialogService();
            dialogService.OpenFileDialog();
            LoadData(dialogService);            

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
                int number = count / 5;
                int currentNumber;
                LoadFile loadFile = new LoadFile();
                ParameterizedThreadStart readFileDel = new ParameterizedThreadStart(loadFile.ReadFile);
                for (int i = 0; i < 5; i++)
                {
                    if (countContent > number)
                    {
                        countContent -= number;
                        currentNumber = number;
                    }
                    else
                    {
                        currentNumber = countContent;
                    }
                    FileReader fileReader = new FileReader(dialogService, start, currentNumber);
                    new Thread(readFileDel).Start(fileReader);
                    start += currentNumber;
                }
                Console.WriteLine(LoadWindow.LoadProgress.Value);
            }
            catch (Exception ex)
            {
                throw;
            }
        }        
        
    }
}
