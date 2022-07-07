using System;
using System.Threading;
using System.Threading.Tasks;
using WpfApp.Command;
using System.IO;
using System.Linq;
using System.Windows;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace WpfApp.ViewModel
{
    public class ButtonViewModel
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
        private MainWindow _mainWindow;

        private string _test;

        public string NameTB
        {
            get
            {
                return _test;
            }
            set
            {
                _test = value;
            }
        }

        public string DateTB { get; set; }

        public string SurnameTB { get; set; }

        public string PatronymicTB { get; set; }

        public string CityTB { get; set; }

        public string CountryTB { get; set; }

        public string ResultTB { get; set; }
        public ButtonCommand Command { get; set; }

        public ButtonCommand ExcelCommand { get; set; }

        public ButtonCommand XMLCommand { get; set; }

        public ButtonCommand FindCommand { get; set; }

        public ButtonViewModel()
        {
            _semaphore = new Semaphore(MAX_THREADS, MAX_THREADS);
            Command = new ButtonCommand(ReadFileClick);
            ExcelCommand = new ButtonCommand(ExportExcelClick);
            XMLCommand = new ButtonCommand(ExportXMLClick);
            FindCommand = new ButtonCommand(FindClick);
            _start = 0;
            _loadData = new LoadData();
            _isLoadFile = true;
            DateTB = "dd-mm-yyyy";
        }
        public ButtonViewModel(MainWindow window) : this()
        {
            _mainWindow = window;
        }
        private async Task LoadData(DialogService dialogService, Action<object> actionInfo, int count)
        {
            Load LoadWindow = new Load();
            //CurrentProgress currentProgress = new CurrentProgress(minValue:0, maxValue: count);
            if (_isLoadFile)
                LoadWindow.Show();
            try
            {
                int current = count;
                //LoadWindow.LoadProgress.Maximum = count;
                await Task.Run(() =>
                {
                    while (current > 0)
                    {
                        _semaphore.WaitOne();
                        ThreadPool.QueueUserWorkItem(new WaitCallback(actionInfo), new FileReader(dialogService, _start, _semaphore));
                        Thread.Sleep(1);
                        if (current > MAX_ROWS)
                        {
                            current -= MAX_ROWS;
                            _start += MAX_ROWS;
                            //currentProgress.Load(MAX_ROWS);
                            //LoadWindow.LoadProgress.Value += MAX_ROWS;
                        }
                        else
                        {
                            _start += current;
                            if (_isLoadFile)
                            {
                                //LoadWindow.LoadProgress.Value += current;
                                //LoadWindow.LoadButton.IsEnabled = true;
                                //currentProgress.Load(current);
                                Console.WriteLine("dct");
                                current = 0;
                            }
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
        private async void ReadFileClick()
        {
            _dialogService = new DialogService();
            _loadFile = new LoadFile();
            _dialogService.OpenFileDialog();
            _actionInfo += _loadFile.LoadCSVFile;
            int count = File.ReadLines(path: _dialogService.FilePath).Count();
            await LoadData(dialogService: _dialogService, actionInfo: _actionInfo, count: count);
        }
        private async void FindClick()
        {
            DbPeopleContextConn context = new DbPeopleContextConn();
            await Task.Run(() => {
                Console.WriteLine(SurnameTB);
                Console.WriteLine(_test);
                Console.WriteLine(NameTB);
                //bool isHolder = IsPlaceholder();
                //bool isCorrectDate = CheckDate();
                /*DateTime date;
                if (!isHolder && !isCorrectDate)
                {
                    _mainWindow.DateTB.Focus();
                    MessageBox.Show("Проверьте дату", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (!isHolder && isCorrectDate)
                    date = GetDate();
                else
                    date = DateTime.Now;*/
                _loadData.People = (from human in context.People where
                                    (string.IsNullOrEmpty(NameTB) || human.Name == NameTB)
                                    && (string.IsNullOrEmpty(SurnameTB) || human.Surname == SurnameTB)
                                    && (string.IsNullOrEmpty(PatronymicTB) || human.Patronymic == PatronymicTB)
                                    && (string.IsNullOrEmpty(CityTB) || human.City == CityTB)
                                    && (string.IsNullOrEmpty(CountryTB) || human.Country == CountryTB)
                                    select human).ToList();
                ResultTB= _loadData.People.Count.ToString();
            });
        }
        private bool IsPlaceholder() => _mainWindow.DateTB.Text.CompareTo(PLACEHOLDER) == 0 ? true : false;

        private bool CheckDate()
        {
            Regex regex = new Regex(@"([0-3]{1}[0-9]{1})-([0-1]{1}[0-9]{1})-(19|20)([0-9]{2})");
            return regex.IsMatch(DateTB);
        }

        private DateTime GetDate()
        {
            string[] dates = _mainWindow.DateTB.Text.Split('-');
            return new DateTime(Int32.Parse(dates[2]), Int32.Parse(dates[1]), Int32.Parse(dates[0]));
        }
        private void ExportXMLClick()
        {
            _loadData.LoadDataXML();
        }

        private async void ExportExcelClick()
        {
            _actionInfo += _loadData.LoadDataExcel;
            await LoadData(_dialogService, _actionInfo, _loadData.People.Count);
        }
    }
}
