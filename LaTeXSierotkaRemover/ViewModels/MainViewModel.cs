using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace LaTeXSierotkaRemover.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private static readonly string[] prefixes = { "~", " ", Environment.NewLine, "\r\n" };
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

        private string _documentAddress;
        public string DocumentAddress
        {
            get { return _documentAddress; }
            set { Set(ref _documentAddress, value); }
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
            this.DocumentAddress = "./index.tex";
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
            try
            {
                this.IsVisibleLoadingLockGrid = true;
                string document = File.ReadAllText(this.DocumentAddress);
                foreach (var sierotka in SierotkiList)
                {
                    document = this.ReplaceDocument(document, sierotka.ToString().Trim());
                }
                File.WriteAllText(this.DocumentAddress, document);
            }
            catch (FileNotFoundException FNFex)
            {
                MessageBox.Show(FNFex.Message, "File Not Found");
            }
            finally
            {
                this.IsVisibleLoadingLockGrid = false;
            }
        }

        private string ReplaceDocument(string document, string sierotka)
        {
            string result = document;
            foreach (string prefix in prefixes)
            {
                foreach (string variantOfSierotka in GetVariantsSierotka(sierotka))
                {
                    result = result.Replace(prefix + variantOfSierotka + " ", prefix + variantOfSierotka + "~");
                }
            }

            return result;
        }

        private List<string> GetVariantsSierotka(string sierotka)
        {
            List<string> result = new List<string>();
            if (sierotka.Length >= 2)
            {
                result.Add(char.ToUpper(sierotka[0]) + sierotka.Substring(1));
            }
            result.Add(sierotka);
            result.Add(sierotka.ToUpper());
            result.Add(sierotka.ToLower());

            result = result.Distinct().ToList();

            return result;
        }
        #endregion
    }
}
