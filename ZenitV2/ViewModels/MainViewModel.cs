using Microsoft.Win32;
using ZenitV2.BL.Interfaces;
using ZenitV2.BL.Realizations;
using ZenitV2.Data;
using ZenitV2.Models;
using ZenitV2.MVVMCore;
using ZenitV2.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ZenitV2.ViewModels
{
    [AttachToWindow(typeof(MainWindow))]
    public class MainViewModel : BaseViewModel
    {
        private string _name;


        private IWordReader _wordReader;
        private readonly ICordinateConverterBL _cordinateConverterBl;
        private ObservableCollection<List<WordInputData>> _input;
        private ObservableCollection<CordinateModel> _pz90;
        private ObservableCollection<CordinateModel> _sk42;
        private int _zone;
        private ObservableCollection<AdvancedCordinateModel> _meredian;
        private string _selectedCountOfSymbols;
        private AdvancedCordinateModel _sk42selectedPoint1;
        private AdvancedCordinateModel _sk42selectedPoint2;
        private ObservableCollection<AdvancedCordinateModel> _sk42PointsForDirectAngle;
        private ObservableCollection<DirectAngleCordinateModel> _sk42Direct;
        public List<string> AllowedCountOfSymbols { get; private set; }
        public string SelectedCountOfSymbols
        {
            get => _selectedCountOfSymbols;
            set => Set(ref _selectedCountOfSymbols, value);
        }
        public int Zone
        {
            get => _zone;
            set => Set(ref _zone, value);
        }
        public string Name
        {
            get => _name;
            set => Set(ref _name, value);
        }

        public ObservableCollection<List<WordInputData>> Input
        {
            get => _input;
            set => Set(ref _input, value);
        }
        public ObservableCollection<CordinateModel> PZ90
        {
            get => _pz90;
            set => Set(ref _pz90, value);
        }
        public ObservableCollection<CordinateModel> SK42
        {
            get => _sk42;
            set => Set(ref _sk42, value);
        }

        public ObservableCollection<AdvancedCordinateModel> Meredian
        {
            get => _meredian;
            set => Set(ref _meredian, value);
        }

        public AdvancedCordinateModel SK42SelectedPoint1
        {
            get => _sk42selectedPoint1;
            set => Set(ref _sk42selectedPoint1, value);
        }

        public AdvancedCordinateModel SK42SelectedPoint2
        {
            get => _sk42selectedPoint2;
            set => Set(ref _sk42selectedPoint2, value);
        }

        public ObservableCollection<AdvancedCordinateModel> SK42PointsPointsForDirectAngle
        {
            get => _sk42PointsForDirectAngle;
            set => Set(ref _sk42PointsForDirectAngle, value);
        }

        public ObservableCollection<DirectAngleCordinateModel> SK42Direct
        {
            get => _sk42Direct;
            set => Set(ref _sk42Direct, value);
        }

        public ICommand AddNewDirectAngleCommand { get; set; }
        public ICommand ShowName { get; set; }
        public MainViewModel(IWordReader wordReader, ICordinateConverterBL cordinateConverterBl)
        {
            ShowName = new Command(ShowNameCommand);
            AddNewDirectAngleCommand = new Command(AddNewDirectAngleCommandBehavior);
            _wordReader = wordReader;
            _cordinateConverterBl = cordinateConverterBl;

            Utils.EventManager.ChangeCountOfSymbols += EventManager_ChangeCountOfSymbols;
            Input = new ObservableCollection<List<WordInputData>>();

            AllowedCountOfSymbols = new List<string>() { "6", ">6" };
            SelectedCountOfSymbols = AllowedCountOfSymbols.First();
        }

        private void AddNewDirectAngleCommandBehavior(object obj)
        {
            if (SK42Direct == null)
                SK42Direct = new ObservableCollection<DirectAngleCordinateModel>();

            SK42Direct.Add(_cordinateConverterBl.DirectAngle(SK42SelectedPoint1, SK42SelectedPoint2));
            Notify(nameof(SK42Direct));
        }

        ~MainViewModel()
        {
            Utils.EventManager.ChangeCountOfSymbols -= EventManager_ChangeCountOfSymbols;
        }

        private void EventManager_ChangeCountOfSymbols()
        {
            if (SK42 != null && SK42.Count != 0)
            {
                Meredian?.Clear();
                MessageBox.Show("ti her");
                var advancedCordinates = _cordinateConverterBl.ConvertToAdvanced(SK42.ToList() ?? new List<CordinateModel>(), Zone);
                advancedCordinates.ForEach(el => Meredian?.Add(el));
            }

        }

        private void ShowNameCommand(object obj)
        {
            var files = GetFilePath();

            PZ90?.Clear();
            SK42?.Clear();
            Input = new ObservableCollection<List<WordInputData>>();
            Meredian = new ObservableCollection<AdvancedCordinateModel>();
            SK42PointsPointsForDirectAngle = new ObservableCollection<AdvancedCordinateModel>();

            foreach (var file in files)
            {
                _wordReader.BindFile(file);

                var result = _wordReader.MapFile();
                var mediumValues = result.Select(el => el[el.Count - 1]);

                switch (_wordReader.GetFileType())
                {
                    case IWordReader.FileType.PZ90:
                        {
                            if (PZ90 == null)
                            {
                                PZ90 = new ObservableCollection<CordinateModel>();
                                _cordinateConverterBl.ConvertToPZ90(mediumValues.ToList()).ForEach(el => PZ90.Add(el));
                            }
                            else
                                _cordinateConverterBl.ConvertToPZ90(mediumValues.ToList()).ForEach(el => PZ90.Add(el));
                            break;
                        }
                    case IWordReader.FileType.SK42:
                        {
                            if (SK42 == null)
                            {
                                SK42 = new ObservableCollection<CordinateModel>();
                                _cordinateConverterBl.ConvertToSK42(mediumValues.ToList(), Zone).ForEach(el => SK42.Add(el));
                                var advancedCordinates = _cordinateConverterBl.ConvertToAdvanced(SK42.ToList(), Zone);
                                advancedCordinates.ForEach(el => Meredian.Add(el));
                                advancedCordinates.ForEach(el => SK42PointsPointsForDirectAngle.Add(el));
                            }
                            else
                            {
                                _cordinateConverterBl.ConvertToSK42(mediumValues.ToList(), Zone).ForEach(el => SK42.Add(el));
                                var advancedCordinates = _cordinateConverterBl.ConvertToAdvanced(SK42.ToList(), Zone);
                                advancedCordinates.ForEach(el => Meredian.Add(el));
                                advancedCordinates.ForEach(el => SK42PointsPointsForDirectAngle.Add(el));
                            }
                            break;
                        }
                }

                _wordReader.UnbindFile();

                result.ForEach(el => Input.Add(el));
            }

            Notify(nameof(Input));
            Notify(nameof(Meredian));
            Notify(nameof(SK42PointsPointsForDirectAngle));
        }
        private string[] GetFilePath()
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            fileDialog.Filter = "Doc* | *.doc*";
            fileDialog.ShowDialog();
            return fileDialog.FileNames;

        }
    }
}
