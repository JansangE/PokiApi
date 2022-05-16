using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PokiApi.ViewModels;

using CSVManager;

namespace PokiApi.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new ViewModelMain();
            InitializeComponent();

            init();
        }

        private void init()
        {
            //AutoCompleteStringCollection stringCollection = new AutoCompleteStringCollection() {"String 1", "String 2", "String 3", };
            //txtSearch.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //txtSearch.AutoCompleteSource = AutoCompleteSource.CustomSource;
            //txtSearch.AutoCompleteCustomSource = stringCollection;
        }
    }
}
