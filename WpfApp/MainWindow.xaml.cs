using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        private const string PLACEHOLDER = "dd-mm-yyyy";
        private const int MAX_THREADS = 5;
        private const int MAX_ROWS = 5000;
        private int _start;
        private LoadFile _loadFile;
        private Action<object> _actionInfo;
        private DialogService _dialogService;
        private Semaphore _semaphore;
        private LoadData _loadData;
        private bool _isLoadFile;
        private SideMenu _sideMenu;

        private void panelHeader_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
        public MainWindow()
        {
            _semaphore = new Semaphore(MAX_THREADS, MAX_THREADS);
            InitializeComponent();
            _sideMenu = new SideMenu(sideMenu: sideMenu);
            DateTB.GotFocus += RemovePlaceHolder;
            DateTB.LostFocus += AddPlaceHolder;
            _start = 0;
            _loadData = new LoadData();
            _isLoadFile = true;
        }

        private void AddPlaceHolder(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(DateTB.Text))
            {
                DateTB.Text = PLACEHOLDER;
            }               
        }

        private void RemovePlaceHolder(object sender, RoutedEventArgs e)
        {
            if (DateTB.Text == PLACEHOLDER)
            {
                DateTB.Text = "";
            }
        }

        private async Task LoadData(DialogService dialogService,Action<object> actionInfo,int count)
        {
            Load LoadWindow = new Load();
            if (_isLoadFile)
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
                            if (_isLoadFile)
                                Dispatcher.Invoke(() => LoadWindow.LoadProgress.Value += MAX_ROWS);
                        }
                        else
                        {
                            _start += current;
                            if (_isLoadFile)
                            {
                                Dispatcher.Invoke(() => {
                                    LoadWindow.LoadProgress.Value += current;
                                });
                            }
                            Dispatcher.Invoke(() => {
                                LoadWindow.LoadButton.IsEnabled = true;
                            });
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
        private bool IsPlaceholder() => DateTB.Text.CompareTo(PLACEHOLDER) == 0 ? true : false;

        private bool CheckDate()
        {
            Regex regex = new Regex(@"([0-3]{1}[0-9]{1})-([0-1]{1}[0-9]{1})-(19|20)([0-9]{2})");
            return regex.IsMatch(DateTB.Text);
        }

        private DateTime GetDate()
        {         
            string[] dates = DateTB.Text.Split('-');
            return new DateTime(Int32.Parse(dates[2]) , Int32.Parse(dates[1]) , Int32.Parse(dates[0]));            
        }



        private void Menu_Button_Click(object sender, RoutedEventArgs e)
        {
            _sideMenu.StartTimerEventHandler?.Invoke();
        }

        private async void ReadFileClick(object sender, MouseButtonEventArgs e)
        {
            _dialogService = new DialogService();
            _loadFile = new LoadFile();
            _dialogService.OpenFileDialog();
            _actionInfo += _loadFile.ReadFile;
            int count = File.ReadLines(path: _dialogService.FilePath).Count();
            await LoadData(dialogService: _dialogService, actionInfo: _actionInfo, count: count);
        }

        private async void FindClick(object sender, MouseButtonEventArgs e)
        {
            DbPeopleContextConn context = new DbPeopleContextConn();
            await Task.Run(() => {
                Dispatcher.Invoke(() =>
                {
                    bool isHolder = IsPlaceholder();
                    bool isCorrectDate = CheckDate();
                    DateTime date;
                    if (!isHolder && !isCorrectDate)
                    {
                        DateTB.Focus();
                        MessageBox.Show("Проверьте дату", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (!isHolder && isCorrectDate)
                        date = GetDate();
                    else
                        date = DateTime.Now;
                    _loadData.People = (from human in context.People
                                        where (isHolder ? true : human.Date == date)
                                        && (string.IsNullOrEmpty(NameTB.Text) || human.Name == NameTB.Text)
                                        && (string.IsNullOrEmpty(SurnameTB.Text) || human.Surname == SurnameTB.Text)
                                        && (string.IsNullOrEmpty(PatronymicTB.Text) || human.Patronymic == PatronymicTB.Text)
                                        && (string.IsNullOrEmpty(CityTB.Text) || human.City == CityTB.Text)
                                        && (string.IsNullOrEmpty(CountryTB.Text) || human.Country == CountryTB.Text)
                                        select human).ToList();
                    ResultTb.Text = _loadData.People.Count.ToString();
                });
            });
        }

        private void ExportXMLClick(object sender, MouseButtonEventArgs e)
        {
            _loadData.LoadDataXML();
        }

        private async void ExportExcelClick(object sender, MouseButtonEventArgs e)
        {
            _actionInfo += _loadData.LoadDataExcel;
            await LoadData(_dialogService, _actionInfo, _loadData.People.Count);
        }

        
    }
}
