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
            try
            {
                // Bootstrap
                Bootstrap();
                // Load Window
                this.MainWindow = new MainWindow();
                this.MainWindow.Show();
                // Tell context which is the shell
                context.Initialize(this.MainWindow);
            }
            catch { }
            finally { }
        }

        private void Bootstrap()
        {
            try
            {
                bootstrapper = new Bootstrapper();
                context = bootstrapper.Load();
            }
            catch
            {
                throw;
            }
        }
    }
}