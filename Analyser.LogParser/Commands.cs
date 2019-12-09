using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Analyser.LogParser
{
    public static class Commands
    {
        public static readonly RoutedUICommand New = new RoutedUICommand(
          "_New Suivi MCO",
          "NewSuiviMCO",
          typeof(Commands),
          new InputGestureCollection()
          {
                new KeyGesture(Key.N, ModifierKeys.Control)
          }
      );
    }
}
