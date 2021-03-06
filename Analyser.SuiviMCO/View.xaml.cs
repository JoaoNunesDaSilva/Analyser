﻿using Analyser.Infrastructure.Interfaces;
using Analyser.Infrastructure.Model;
using Analyser.SuiviMCO.Helpers;
using Analyser.SuiviMCO.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Analyser.SuiviMCO
{
    [Injectable("SuiviMCOView")]
    public partial class View : UserControl, IView, IDisposable
    {
        private ISuiviMCOService service;
        private ISuiviMCOModel viewModel;
        private bool disposed;
        private IContext context;

        public View(IContext context, ISuiviMCOModel viewModel, ISuiviMCOService service)
        {
            this.service = service;
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

        private void FilterColumn_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void FilterColumn_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PopupFilterDialog(sender);
            e.Handled = true;
        }
        ToggleButton activeFilter = null;
        private void PopupFilterDialog(object sender)
        {
            activeFilter = (ToggleButton)sender;
            int colIndex = (int)activeFilter.Tag;
            viewModel.Filters = this.service.CreateColumnFilters(colIndex);
        }

        private void FilterColumnOk_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void FilterColumnOk_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Button okButton = (Button)sender;
            int colIndex = (int)okButton.Tag;
            this.viewModel.MCOData = this.service.ApplyMCOFilter(colIndex, viewModel.Filters);
            activeFilter.IsChecked = false; // Close popup
            ToggleButton toggle = (ToggleButton)((Panel)((FrameworkElement)((FrameworkElement)((FrameworkElement)((FrameworkElement)sender).Parent).Parent).Parent).Parent).Children[0];
            if (!viewModel.Filters.Any(p => p.Checked == false))
            {
                toggle.Background = Brushes.Transparent;
                toggle.Foreground = Brushes.Black;
            }
            else
            {
                toggle.Background = Brushes.Green;
                toggle.Foreground = Brushes.Red;
            }
            e.Handled = true;
        }

        private void FilterColumnClear_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void FilterColumnClear_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Button ClearButton = (Button)sender;
            int colIndex = (int)ClearButton.Tag;
            foreach (FilterModel model in viewModel.Filters.Where(p => p.Checked == true))
                model.Checked = false;

            e.Handled = true;
        }

        private void FilterColumnAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void FilterColumnAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Button ClearButton = (Button)sender;
            int colIndex = (int)ClearButton.Tag;
            foreach (FilterModel model in viewModel.Filters.Where(p => p.Checked == false))
                model.Checked = true;


            e.Handled = true;
        }

        private void RefreshList_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefreshList_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // reloads data and clears all filters
            this.service.RefreshList();
            this.viewModel.Filters = new ObservableCollection<FilterModel>();
            foreach (DataGridColumnHeader header in VisualTreeHelperClass.GetVisualChildCollection<DataGridColumnHeader>(dgMCO))
            {
                ToggleButton toggle = VisualTreeHelperClass.GetVisualChildCollection<ToggleButton>(header).First();
                toggle.Background = Brushes.Transparent;
                toggle.Foreground = Brushes.Black;
            }
            e.Handled = true;
        }
        private void ShowAll_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ShowAll_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // clears all filters and shows all loaded records
            this.viewModel.MCOData = this.service.ShowAll();
            this.viewModel.Filters = new ObservableCollection<FilterModel>();
            foreach (DataGridColumnHeader header in VisualTreeHelperClass.GetVisualChildCollection<DataGridColumnHeader>(dgMCO))
            {
                ToggleButton toggle = VisualTreeHelperClass.GetVisualChildCollection<ToggleButton>(header).First();
                toggle.Background = Brushes.Transparent;
                toggle.Foreground = Brushes.Black;
            }
            e.Handled = true;
        }

        private void dgMCO_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MCOData model = (MCOData)dgMCO.SelectedItem;
            NotesLinkHelper.OpenNotesDocument(model.Ticket.DocumentUniqueID);

        }
    }
}
