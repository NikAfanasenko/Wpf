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
        private List<TextBox> _elements;
        private List<TextBox> _inputs;
        private LoadFile _loadFile;
        private Func<object,bool> _readerInfo;
        private DialogService _dialogService;

        LoadData loadData;
        public MainWindow()
        {
            InitializeComponent();
            _start = 0;
            loadData = new LoadData();
            _elements = new List<TextBox>() {  TB2, TB3, TB4, TB5,TB6 };
            _inputs = new List<TextBox>();
        }


        private async void ReadFileButton_Click(object sender, RoutedEventArgs e)
        {
            _dialogService = new DialogService();
            _dialogService.OpenFileDialog();
            _loadFile = new LoadFile();
            _readerInfo += _loadFile.ReadFile;
            int count = File.ReadLines(_dialogService.FilePath).Count();
            await LoadData(_dialogService, _readerInfo,count);    
            //CheckTB();

        }
        private async Task LoadData(DialogService dialogService, Func<object,bool> actionMethd,int count)
        {
            Load LoadWindow = new Load();
            LoadWindow.Show();
            try
            {                               
                int current = count;
                LoadWindow.LoadProgress.Maximum = count;                          
                
                await Task.Run(() => 
                {
                    while (current > 0)
                    {
                        var threads = new List<Thread>();
                        for (int i = 0; i < MAX_THREADS; i++)
                        {
                            try
                            {
                                 threads.Add(new Thread((s) =>
                                 {
                                     actionMethd.Invoke(new FileReader(dialogService, _start));
                                     //loadFile.ReadFile(new FileReader(dialogService, _start));
                                 })
                                 { IsBackground = true });
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
                                 
                                 threads[i].Start();

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                throw;
                            }
                        }
                        threads.ForEach(thread => 
                        {                            
                            thread.Join();
                            LoadWindow.LoadProgress.Dispatcher.Invoke(() => 
                            {
                                LoadWindow.LoadProgress.Value += MAX_ROWS;
                                if (LoadWindow.LoadProgress.Value == LoadWindow.LoadProgress.Maximum)
                                {
                                    LoadWindow.LoadButton.IsEnabled = true;
                                }
                            });
                        });
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
                    loadData.People = (from human in context.People
                                  where string.IsNullOrEmpty(TB2.Text) || human.Name == TB2.Text
                                  && string.IsNullOrEmpty(TB3.Text) || human.Surname == TB3.Text
                                  && string.IsNullOrEmpty(TB4.Text) || human.Patronymic == TB4.Text
                                  && string.IsNullOrEmpty(TB5.Text) || human.City == TB5.Text
                                  && string.IsNullOrEmpty(TB6.Text) || human.Country == TB6.Text
                                  select human).ToList();
                    ResultTb.Text = loadData.People.Count.ToString();
                });
            });

        }

        private void CheckTB()
        {
            (from tb in _inputs select tb).All(tb =>
            {
                if (new Regex(@"\d*").IsMatch(tb.Text))
                {
                    MessageBox.Show("Не корректный ввод!","Warning",MessageBoxButton.OK,MessageBoxImage.Warning);
                    tb.Focus();
                    return false;
                }
                return true;
            });
            
        }

        private void CheckDate()
        {
            if (!string.IsNullOrEmpty(TB1.Text))
            {
                if (!new Regex(@"[0-3][0-9]-[0-1][0-9]-19|20[0-9]{2}").IsMatch(TB1.Text))
                {
                    MessageBox.Show("Дата введена не правильно!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    TB1.Focus();
                }
            }
        }

        private DateTime GetDate()
        {
            string[] dates = TB1.Text.Split('-');
            return new DateTime(Int32.Parse(dates[2]) , Int32.Parse(dates[1]) , Int32.Parse(dates[0]));            
        }


        private void ExportXMLBut_Click(object sender, RoutedEventArgs e)
        {
            loadData.LoadDataXML();
        }

        private async void ExportExcelBut_Click(object sender, RoutedEventArgs e)
        {
            loadData.LoadDataExcel();
            //_readerInfo += ;
            //await LoadData(_dialogService, _readerInfo,loadData.People.Count);
        }
    }
}
