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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Analyser.LogParser
{
    [Injectable("LogParserView")]
    public partial class View : UserControl, IView
    {
        private ILogParserModel viewModel;
        private bool disposed;
        private IContext context;

        public View(IContext context, ILogParserModel viewModel)
        {
            this.context = context;
            this.viewModel = viewModel;
            InitializeComponent();
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
                this.viewModel = null;
                disposed = true;
            }
        }

    }
}
