using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

using Catel;
using Catel.Logging;
using Catel.MVVM;
using Catel.Reflection;
using Catel.Windows.Interactivity;

using Microsoft.Xaml.Behaviors;

namespace Equality.Behaviors
{
    /// <summary>
    /// Behavior to automatically scroll to the end of the specified direction when command is executed.
    /// </summary>
    public class AutoScrollOnCommandExecuted : BehaviorBase<ListBox>
    {
        protected ScrollViewer ScrollViewer;

        public AutoScrollOnCommandExecuted()
        {
        }

        #region Properties

        public ICatelCommand Command
        {
            get { return (ICatelCommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICatelCommand), typeof(AutoScrollOnCommandExecuted), new PropertyMetadata(null, OnIsScrollEnabledChangedCallback));
        private static void OnIsScrollEnabledChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AutoScrollOnCommandExecuted target = (AutoScrollOnCommandExecuted)d;

            if (e.OldValue is ICatelCommand oldCommand) {
                oldCommand.Executed -= target.Scroll;
            }

            if (e.NewValue is ICatelCommand command) {
                command.Executed += target.Scroll;
            }
        }

        /// <summary>
        /// You can use this command parameter to filter the command.
        /// </summary>
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register(nameof(CommandParameter), typeof(object), typeof(AutoScrollOnCommandExecuted), new PropertyMetadata(null));

        public Dock Position
        {
            get { return (Dock)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }
        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register(nameof(Position), typeof(Dock), typeof(AutoScrollOnCommandExecuted), new PropertyMetadata(Dock.Bottom));

        #endregion

        #region Methods

        /// <summary>
        /// Called when the <see cref="Behavior{T}.AssociatedObject"/> is loaded.
        /// </summary>
        protected override void OnAssociatedObjectLoaded()
        {
            ScrollViewer = (VisualTreeHelper.GetChild(AssociatedObject, 0) as Decorator).Child as ScrollViewer;
        }

        private void Scroll(object sender, CommandExecutedEventArgs e)
        {
            // Before scrolling we check if command parameter is the same.
            // So we wouldnt trigger command for all list boxes with the same command.
            if (CommandParameter != null && !e.CommandParameter.Equals(CommandParameter)) {
                return;
            }

            switch (Position) {
                case Dock.Top:
                    ScrollViewer.ScrollToTop();
                    break;
                case Dock.Bottom:
                default:
                    ScrollViewer.ScrollToEnd();
                    break;
                case Dock.Left:
                    ScrollViewer.ScrollToLeftEnd();
                    break;
                case Dock.Right:
                    ScrollViewer.ScrollToRightEnd();
                    break;
            }
        }

        #endregion
    }
}
