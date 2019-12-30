using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Analyser.SuiviMCO
{
    public static class Commands
    {
        public static readonly RoutedUICommand NewSuiviMCO = new RoutedUICommand(
          "_New Suivi MCO",
          "NewSuiviMCO",
          typeof(Commands),
          new InputGestureCollection()
          {
                new KeyGesture(Key.N, ModifierKeys.Control)
          }
        );
        public static readonly RoutedUICommand ShowAll = new RoutedUICommand(
        "_Tout",
        "ShowAll",
        typeof(Commands),
        new InputGestureCollection()
        {
                new KeyGesture(Key.T, ModifierKeys.Control)
        }
      );

        public static readonly RoutedUICommand Config = new RoutedUICommand(
          "_Configurations",
          "ConfigSuiviMCO",
          typeof(Commands),
          new InputGestureCollection()
          {
                new KeyGesture(Key.C, ModifierKeys.Control)
          }
        );

        public static readonly RoutedUICommand LoadData = new RoutedUICommand(
          "_Load Data",
          "LoadDataSuiviMCO",
          typeof(Commands),
          new InputGestureCollection()
          {
                new KeyGesture(Key.L, ModifierKeys.Control)
          }
        );
        
        public static readonly RoutedUICommand ChooseLookupFileSuiviMCO = new RoutedUICommand(
          "Choose _Lookup File",
          "ChooseLookupFileSuiviMCO",
          typeof(Commands),
          new InputGestureCollection()
          {
                new KeyGesture(Key.L, ModifierKeys.Control)
          }
        );
        
        public static readonly RoutedUICommand ChooseMCOFileSuiviMCO = new RoutedUICommand(
          "Choose _MCO File",
          "ChooseMCOFileSuiviMCO",
          typeof(Commands),
          new InputGestureCollection()
          {
                new KeyGesture(Key.M, ModifierKeys.Control)
          }
        );
        public static readonly RoutedUICommand ChooseMCOFileSuiviMCOEspaceClient = new RoutedUICommand(
          "Choose MCO _E.C. File",
          "ChooseMCOFileSuiviMCOEspaceClient",
          typeof(Commands),
          new InputGestureCollection()
          {
                new KeyGesture(Key.E, ModifierKeys.Control)
          }
        );

        public static readonly RoutedUICommand ChooseDataFileSuiviMCO = new RoutedUICommand(
          "Choose _Data File" ,
          "ChooseDataFileSuiviMCO",
          typeof(Commands),
          new InputGestureCollection()
          {
                new KeyGesture(Key.D, ModifierKeys.Control)
          }
        );
        
        public static readonly RoutedUICommand SetDataFileSuiviMCO = new RoutedUICommand(
          "Set _Data File",
          "SetDataFileSuiviMCO",
          typeof(Commands),
          new InputGestureCollection()
          {
                new KeyGesture(Key.S, ModifierKeys.Control)
          }
        );

        public static readonly RoutedUICommand FilterColumnOk = new RoutedUICommand(
             "Ok",
             "FilterColumnOk",
             typeof(Commands)
           );

        public static readonly RoutedUICommand FilterColumnAll = new RoutedUICommand(
             "Ok",
             "FilterColumnOk",
             typeof(Commands)
           );

        public static readonly RoutedUICommand FilterColumnClear = new RoutedUICommand(
             "Clear",
             "FilterColumnClear",
             typeof(Commands)
           );


        public static readonly RoutedUICommand FilterColumn = new RoutedUICommand(
             "",
             "FilterColumn",
             typeof(Commands)
           );
    }
}
