using Analyser.Interfaces;
using Analyser.SuiviMCO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Analyser.SuiviMCO
{
    public class Module : ISuiviMCO, IDisposable
    {

        private IView view;
        private IContext context;
        private ISuiviMCOService service;

        public MenuItem FileNewMenuItem { get; private set; }
        public MenuItem ModuleMenu { get; private set; }

        private MenuItem setSourcesMenuItem;
        
        private bool disposed;

        public Module(IContext context)
        {
            this.context = context;
            this.service = (ISuiviMCOService)this.context.GetService("SuiviMCOService");
            this.Initialize();
        }

        #region IDisposable Implementation
        
        ~Module()
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
                //context.Shell.RemoveFileNewMenuItem(FileNewMenuItem);
                disposed = true;
            }
        }

        #endregion

        private void Initialize()
        {
            // Add File New Menu
            FileNewMenuItem = context.Shell.AddFileNewMenuItem("Suivi _MCO", "NewSuiviMCO", Commands.NewSuiviMCO, CanExecuteNew, ExecuteNew);
        }

        #region Load Data
        private void ExecuteLoadData(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
        }
        private void CanExecuteLoadData(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        #region New
        private void ExecuteNew(object sender, ExecutedRoutedEventArgs e)
        {
            if (ModuleMenu != null) return;
            
            // Load Menu
            ModuleMenu = context.Shell.AddMenu("Suivi _MCO", "SuiviMCO");
            setSourcesMenuItem = context.Shell.AddMenuItem(ModuleMenu, "_Load Data", "LoadDataSuiviMCO", Commands.LoadData, CanExecuteLoadData, ExecuteLoadData);

            ViewModel model = new ViewModel();
            view = new View(model)
            {
                DataContext = model
            };
            context.Shell.LoadView("Suivi MCO", view, this);
            e.Handled = true;
        }
        private void CanExecuteNew(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        public void DestroyView()
        {
            context.Shell.RemoveMenu(ModuleMenu);
            ModuleMenu = null;
        }
        #endregion
    }
}
