using Analyser.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace Analyser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        Bootstrapper bootstrapper;
        IContext context;
        public App() : base()
        {
            StartApplication();
        }

        private void StartApplication()
        {
            try
            {
                bootstrapper = new Bootstrapper();
                // Get context singleton from boot up process
                context = bootstrapper.Load();
                // Load main window
                this.MainWindow = new MainWindow();
                this.MainWindow.Show();
                // Initialize context singleton
                context.Initialize(this.MainWindow);
            }
            catch
            {
                throw;
            }
        }
    }
}