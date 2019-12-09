using Analyser.Interfaces;
using Analyser.SuiviMCO.Models;
using Microsoft.Win32;
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

namespace Analyser.SuiviMCO
{
    /// <summary>
    /// Interaction logic for Shell.xaml
    /// </summary>
    public partial class View : UserControl, IView, IDisposable
    {
        ViewModel viewModel;
        private bool disposed;

        public View(ViewModel viewModel)
        {
            InitializeComponent();
            this.viewModel = viewModel;
        }

        ~View()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                }
                disposed = true;
            }
        }


        private void ChooseLookupFile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ChooseLookupFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            viewModel.LoadLookup();
            e.Handled = true;
        }

        private void ChooseMCOFile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ChooseMCOFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            viewModel.LoadMCO();
            e.Handled = true;
        }

        private void ChooseDataFile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ChooseDataFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            viewModel.LoadData();
            e.Handled = true;
        }
        private void SetDataFile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SetDataFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            viewModel.SetDataFile();
            e.Handled = true;
        }
    }
}
