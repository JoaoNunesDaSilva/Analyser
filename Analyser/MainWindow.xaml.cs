using Analyser.Infrastructure.Interfaces;
using Analyser.Models;
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

namespace Analyser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IShell
    {

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Menu Handling
        public MenuItem AddMenu(string header, string icon)
        {
            MenuItem newMenu = new MenuItem();
            newMenu.Header = header;
            newMenu.Icon = LoadIcon(icon);
            this.MainMenu.Items.Add(newMenu);
            return newMenu;
        }
        public MenuItem AddMenuItem(MenuItem parent, string header, string icon, ICommand click, CanExecuteRoutedEventHandler canExecute, ExecutedRoutedEventHandler execute)
        {
            MenuItem newMenuItem = new MenuItem();
            newMenuItem.Header = header;
            newMenuItem.Icon = LoadIcon(icon);
            newMenuItem.Command = click;
            CommandManager.AddCanExecuteHandler(newMenuItem, canExecute);
            CommandManager.AddExecutedHandler(newMenuItem, execute);
            parent.Items.Add(newMenuItem);
            return newMenuItem;
        }
        public void RemoveMenu(MenuItem moduleMenu)
        {
            MainMenu.Items.Remove(moduleMenu);
        }
        public MenuItem AddFileNewMenuItem(string header, string icon, ICommand click, CanExecuteRoutedEventHandler canExecute, ExecutedRoutedEventHandler execute)
        {
            MenuItem newMenuItem = new MenuItem();
            newMenuItem.Header = header;
            newMenuItem.Icon = LoadIcon(icon);
            newMenuItem.Command = click;
            CommandManager.AddCanExecuteHandler(newMenuItem, canExecute);
            CommandManager.AddExecutedHandler(newMenuItem, execute);
            FileNewMI.Items.Add(newMenuItem);
            return newMenuItem;
        }
        public void RemoveFileNewMenuItem(MenuItem newMenuItem)
        {
            FileNewMI.Items.Remove(newMenuItem);
        }
        private object LoadIcon(string icon)
        {
            return null;
        }

        #endregion

        #region ContentView Handling

        public void LoadView(string header, IView view, IModule module)
        {
            ModuleView modView = new ModuleView(view, module);
            TabItem tab = new TabItem() { Header = header, Tag = modView };
            tab.Content = view;
            this.ContentView.Items.Add(tab);
            tab.Focus();
        }

        #endregion

        #region Sidebar Handling

        #endregion

        #region New Command

        private void CanExecuteNew(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = FileNewMI.HasItems;
        }
        private void ExecuteNew(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        #endregion

        #region Open Command

        private void CanExecuteOpen(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }
        private void ExecuteOpen(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        #endregion

        #region Close Command

        private void CanExecuteClose(object sender, CanExecuteRoutedEventArgs e)
        {

            e.CanExecute = ContentView.HasItems;
        }
        private void ExecuteClose(object sender, ExecutedRoutedEventArgs e)
        {
            TabItem tab = (TabItem)ContentView.Items[ContentView.SelectedIndex];
            ModuleView modView = (ModuleView)tab.Tag;
            modView.Module.DestroyView();
            ContentView.Items.Remove(tab);
            e.Handled = true;
        }

        #endregion

        #region Save Command

        private void CanExecuteSave(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ContentView.HasItems;
        }
        private void ExecuteSave(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        #endregion

        #region Delete Command

        private void CanExecuteDelete(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ContentView.HasItems;
        }
        private void ExecuteDelete(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        #endregion

        #region Exit Command

        private void CanExecuteExit(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ExecuteExit(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }



        #endregion



    }
}
