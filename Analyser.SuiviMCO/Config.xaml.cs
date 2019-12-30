using Analyser.Infrastructure.Interfaces;
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
    /// Interaction logic for Config.xaml
    /// </summary>
    public partial class Config : UserControl, IView, IDisposable
    {
        private ISuiviMCOService service;
        private ISuiviMCOModel viewModel;
        private bool disposed;
        private IContext context;
        public Config(IContext context, ISuiviMCOModel viewModel, ISuiviMCOService service)
        {
            this.service = service;
            this.context = context;
            this.viewModel = viewModel;
            InitializeComponent();
        }
        ~Config()
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
                this.viewModel = null;
                disposed = true;
            }
        }


        private void ChooseLookupFile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ChooseLookupFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                viewModel.LookupFile = openFileDialog.FileName;
            e.Handled = true;
        }

        private void ChooseMCOFile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ChooseMCOFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                viewModel.MCOFile = openFileDialog.FileName;
            e.Handled = true;
        }

        private void ChooseMCOFileEspaceClient_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ChooseMCOFileEspaceClient_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                viewModel.MCOFileEspaceClient = openFileDialog.FileName;
            e.Handled = true;
        }

        private void ChooseDataFile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ChooseDataFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                viewModel.DataFile = openFileDialog.FileName;
            e.Handled = true;
        }
        private void SetDataFile_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void SetDataFile_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                viewModel.DataFile = saveFileDialog.FileName;
            e.Handled = true;
        }
    }
}
