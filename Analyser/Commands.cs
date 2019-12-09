using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Analyser
{
    public static class Commands
    {

        public static readonly RoutedUICommand New = new RoutedUICommand(
            "_New",
            "New",
            typeof(Commands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.N, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand Open = new RoutedUICommand(
            "_Open",
            "Open",
            typeof(Commands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.O, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand Close = new RoutedUICommand(
            "_Close",
            "Close",
            typeof(Commands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F4, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand Save = new RoutedUICommand(
            "_Save",
            "Save",
            typeof(Commands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.S, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand Delete = new RoutedUICommand(
            "_Delete",
            "Delete",
            typeof(Commands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.D, ModifierKeys.Control)
            }
        );

        public static readonly RoutedUICommand Exit = new RoutedUICommand(
            "_Exit",
            "Exit",
            typeof(Commands),
            new InputGestureCollection()
            {
                new KeyGesture(Key.F4, ModifierKeys.Alt)
            }
        );
    }
}
