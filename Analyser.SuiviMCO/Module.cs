﻿using Analyser.Infrastructure.Interfaces;
using Analyser.Infrastructure.Model;

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
    [InjectableAttribute("SuiviMCO")]
    public class Module : ISuiviMCO, IDisposable
    {
        private ISuiviMCOService service;
        private IView view;
        private IView config;
        private IContext context;
        private MenuItem loadDataMenuItem;
        private MenuItem configMenuItem;
        private bool disposed;

        public ISuiviMCOModel Model { get; private set; }
        public MenuItem FileNewMenuItem { get; private set; }
        public MenuItem ModuleMenu { get; private set; }

        public Module(IContext context, ISuiviMCOService service)
        {
            Model = new ViewModel();
            this.context = context;
            this.service = service;
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
            view = new View(context, Model, service)
            {
                DataContext = Model
            };
            context.Shell.LoadView("Suivi MCO", view, this);
            this.service.LoadDataFromFiles(this);
            e.Handled = true;
        }
        private void CanExecuteLoadData(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(this.Model.DataFile) &&
                !string.IsNullOrEmpty(this.Model.LookupFile) &&
                !string.IsNullOrEmpty(this.Model.MCOFile);
            e.Handled = true;
        }
        #endregion
        #region Config
        private void ExecuteConfig(object sender, ExecutedRoutedEventArgs e)
        {
            config = new Config(context, Model, service)
            {
                DataContext = Model
            };
            context.Shell.LoadView("Configurations", config, this);
            e.Handled = true;
        }
        private void CanExecuteConfig(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }
        #endregion

        #region New Suivi MCO
        private void ExecuteNew(object sender, ExecutedRoutedEventArgs e)
        {
            if (ModuleMenu != null) return;

            // Load Menu
            ModuleMenu = context.Shell.AddMenu("Suivi _MCO", "SuiviMCO");
            configMenuItem = context.Shell.AddMenuItem(ModuleMenu, "_Configurations", "ConfigSuiviMCO", Commands.Config, CanExecuteConfig, ExecuteConfig);
            loadDataMenuItem = context.Shell.AddMenuItem(ModuleMenu, "_Load Data", "LoadDataSuiviMCO", Commands.LoadData, CanExecuteLoadData, ExecuteLoadData);

            e.Handled = true;
        }
        private void CanExecuteNew(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        public void DestroyView()
        {
            //context.Shell.RemoveMenu(ModuleMenu);
            ModuleMenu = null;
        }
        #endregion
    }
}
