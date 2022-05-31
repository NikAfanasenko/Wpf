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
using System.Text.RegularExpressions;

namespace WpfApp
{
    public partial class MainWindow : Window
    {        
        private const int MAX_THREADS = 5;
        private const int MAX_ROWS = 5000;
        private int _start;
        /*private List<TextBox> _elements;
        private List<TextBox> _inputs;*/
        private LoadFile _loadFile;
        private Action<object> _actionInfo;
        private DialogService _dialogService;
        private Semaphore _semaphore;
        private LoadData _loadData;
        public MainWindow()
        {
            _semaphore = new Semaphore(MAX_THREADS, MAX_THREADS);
            InitializeComponent();
            _start = 0;
            _loadData = new LoadData();
        }


        private async void ReadFileButton_Click(object sender, RoutedEventArgs e)
        {
            _dialogService = new DialogService();
            _loadFile = new LoadFile();
            _actionInfo += _loadFile.ReadFile;
            int count = File.ReadLines(_dialogService.FilePath).Count();
            _dialogService.OpenFileDialog();
            await LoadData(_dialogService, _actionInfo, count);    

        }
        private async Task LoadData(DialogService dialogService,Action<object> actionInfo,int count)
        {
            Load LoadWindow = new Load();
            LoadWindow.Show();
            try
            {                               
                int current = count;
                LoadWindow.LoadProgress.Maximum = count;
                await Task.Run(()=>
                {
                    while (current>0)
                    {
                        _semaphore.WaitOne();
                        ThreadPool.QueueUserWorkItem(new WaitCallback(actionInfo), new FileReader(dialogService, _start,_semaphore));
                        Thread.Sleep(1);
                        if (current > MAX_ROWS)
                        {
                            current -= MAX_ROWS;
                            _start += MAX_ROWS;
                        }
                        else
                        {
                            _start += current;
                            current = 0;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private async void FindBut_Click(object sender, RoutedEventArgs e)
        {
            DbPeopleContextConn context = new DbPeopleContextConn();
            await Task.Run(() => {
                Dispatcher.Invoke(() => 
                {
                    _loadData.People = (from human in context.People
                                  where string.IsNullOrEmpty(TB2.Text) || human.Name == TB2.Text
                                  && string.IsNullOrEmpty(TB3.Text) || human.Surname == TB3.Text
                                  && string.IsNullOrEmpty(TB4.Text) || human.Patronymic == TB4.Text
                                  && string.IsNullOrEmpty(TB5.Text) || human.City == TB5.Text
                                  && string.IsNullOrEmpty(TB6.Text) || human.Country == TB6.Text
                                  select human).ToList();
                    ResultTb.Text = _loadData.People.Count.ToString();
                });
            });

        }

        
        private DateTime GetDate()
        {
            string[] dates = TB1.Text.Split('-');
            return new DateTime(Int32.Parse(dates[2]) , Int32.Parse(dates[1]) , Int32.Parse(dates[0]));            
        }


        private void ExportXMLBut_Click(object sender, RoutedEventArgs e)
        {
            _loadData.LoadDataXML();
        }

        private async void ExportExcelBut_Click(object sender, RoutedEventArgs e)
        {
            _actionInfo += _loadData.LoadDataExcel;
            await LoadData(_dialogService, _actionInfo,_loadData.People.Count);
            //_loadData.LoadDataExcel();
        }
    }
}
