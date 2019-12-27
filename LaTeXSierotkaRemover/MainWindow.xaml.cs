using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.IO;

namespace LaTeXSierotkaRemover
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly string[] prefixes = { "~", " ", Environment.NewLine, "\r\n" };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ReadFileButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingLockGrid.Visibility = Visibility.Visible;
                var output = File.ReadAllLines(UrlBaseTextBox.Text);
                SierotkiView.ItemsSource = output;
            }
            finally
            {
                LoadingLockGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void DoThis_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadingLockGrid.Visibility = Visibility.Visible;
                string document = File.ReadAllText(UrlLatexTextBox.Text);
                foreach(var sierotka in SierotkiView.Items)
                {
                    document = this.ReplaceDocument(document, sierotka.ToString().Trim());// document.Replace(" " + sierotka + " ", " " + sierotka + "~");
                }
                File.WriteAllText(UrlLatexTextBox.Text, document);
            }
            finally
            {
                LoadingLockGrid.Visibility = Visibility.Collapsed;
            }

        }

        private string ReplaceDocument(string document, string sierotka)
        {
            string result = document;
            foreach(string prefix in prefixes)
            {
                foreach(string variantOfSierotka in GetVariantsSierotka(sierotka))
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
    }
}
