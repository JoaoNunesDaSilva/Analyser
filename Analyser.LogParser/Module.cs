using Analyser.Infrastructure.Interfaces;
using Analyser.Infrastructure.Model;
using Analyser.LogParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Analyser.LogParser
{
    [Injectable("LogParser")]
    public class Module : ILogParser, IDisposable
    {
        private ILogParserService service;
        public MenuItem FileNewMenuItem { get; private set; }
        public MenuItem ModuleMenu { get; private set; }

        private IView view;
        private ILogParserModel model;
        private IContext context;

        private bool disposed;

        public Module(IContext context, ILogParserService service)
        {
            this.service = service;
            this.context = context;
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
            FileNewMenuItem = context.Shell.AddFileNewMenuItem("_Prima Log Parser", "PrimaLogParser", Commands.New, CanExecuteNew, ExecuteNew);
            // Add Own Menu (Disabled)
        }

        private void ExecuteNew(object sender, ExecutedRoutedEventArgs e)
        {
            if (ModuleMenu != null) return;
            // Load Menu
            ModuleMenu = context.Shell.AddMenu("_Prima Log Parser", "PrimaLogParser");

            model = new ViewModel();
            view = new View(context, model)
            {
                DataContext = model
            };
            context.Shell.LoadView("Prima Log Parser", view, this);
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
    }
}
