using Analyser.Interfaces;
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
    public class Module : ILogParser, IDisposable
    {

        public MenuItem FileNewMenuItem { get; private set; }
        public MenuItem ModuleMenu { get; private set; }

        private IContext context;
        private ILogParserService service;

        private bool disposed;

        public Module(IContext context)
        {
            this.context = context;
            this.service = (ILogParserService)this.context.GetService("LogParserService");
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

            ViewModel model = new ViewModel();
            View view = new View
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
