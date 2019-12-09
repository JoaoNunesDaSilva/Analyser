using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace Analyser.Interfaces
{
    public interface IShell
    {
        void LoadView(string header, IView view, IModule module);
        MenuItem AddMenu(string header, string icon);
        MenuItem AddMenuItem(MenuItem parent, string header, string icon, ICommand click, CanExecuteRoutedEventHandler canExecute, ExecutedRoutedEventHandler execute);
        MenuItem AddFileNewMenuItem(string header, string icon, ICommand click, CanExecuteRoutedEventHandler canExecute, ExecutedRoutedEventHandler execute);
        void RemoveFileNewMenuItem(MenuItem newMenuItem);
        void RemoveMenu(MenuItem mainMenu);
    }
}
