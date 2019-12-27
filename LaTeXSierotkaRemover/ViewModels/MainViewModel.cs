using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace LaTeXSierotkaRemover.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields

        #endregion

        #region Properties
        private ObservableCollection<string> _sierotkiList;
        public ObservableCollection<string> SierotkiList
        {
            get { return _sierotkiList; }
            set { Set(ref _sierotkiList, value); }
        }

        private string _sierotkiBaseAddress;
        public string SierotkiBaseAddress
        {
            get { return _sierotkiBaseAddress; }
            set { Set(ref _sierotkiBaseAddress, value); }
        }

        private bool _isVisibleLoadingLockGrid;
        public bool IsVisibleLoadingLockGrid
        {
            get { return _isVisibleLoadingLockGrid; }
            set { Set(ref _isVisibleLoadingLockGrid, value); }
        }

        #endregion

        #region Constructors
        public MainViewModel()
        {
            InitializeCommands();
            InitializeVariables();
        }
        #endregion

        #region Commands
        private ICommand _readBaseCommand;
        public ICommand ReadBaseCommand
        {
            get { return _readBaseCommand; }
            set { Set(ref _readBaseCommand, value); }
        }

        private ICommand _removeSierotkiCommand;
        public ICommand RemoveSierotkiCommand
        {
            get { return _removeSierotkiCommand; }
            set { Set(ref _removeSierotkiCommand, value); }
        }
        #endregion

        #region Public methods

        #endregion

        #region Private methods
        private void InitializeCommands()
        {
            this.ReadBaseCommand = new RelayCommand(LoadSierotkiBase);
            this.RemoveSierotkiCommand = new RelayCommand(RemoveSierotki);
        }

        private void InitializeVariables()
        {
            this.SierotkiBaseAddress = "./base.txt";
        }

        private void LoadSierotkiBase()
        {
            try
            {
                this.IsVisibleLoadingLockGrid = true;
                var output = File.ReadAllLines(SierotkiBaseAddress);
                this.SierotkiList = new ObservableCollection<string>(output);
            }
            finally
            {
                this.IsVisibleLoadingLockGrid = false;
            }
        }

        private void RemoveSierotki()
        {

        }
        #endregion
    }
}
