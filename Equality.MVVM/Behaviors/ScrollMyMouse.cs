using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

using Catel;
using Catel.Logging;
using Catel.Reflection;
using Catel.Windows.Interactivity;

using Microsoft.Xaml.Behaviors;

namespace Equality.Behaviors
{
    /// <summary>
    /// Behavior to scroll <see cref="ListBox"/> by mouse position.
    /// </summary>
    public class ScrollByMouse : BehaviorBase<ListBox>
    {
        protected ScrollViewer ScrollViewer;

        public ScrollByMouse()
        {
        }

        #region Properties

        public bool IsScrollEnabled
        {
            get { return (bool)GetValue(IsScrollEnabledProperty); }
            set { SetValue(IsScrollEnabledProperty, value); }
        }
        public static readonly DependencyProperty IsScrollEnabledProperty =
            DependencyProperty.Register(nameof(IsScrollEnabled), typeof(bool), typeof(ScrollByMouse), new PropertyMetadata(true, OnIsScrollEnabledChangedCallback));
        private static void OnIsScrollEnabledChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScrollByMouse target = (ScrollByMouse)d;

            if (target != null && ((bool)e.NewValue)) {
                target.BindEvent();
            } else {
                target.UnbindEvent();
            }
        }

        public double Tolerance
        {
            get { return (double)GetValue(ToleranceProperty); }
            set { SetValue(ToleranceProperty, value); }
        }
        public static readonly DependencyProperty ToleranceProperty =
            DependencyProperty.Register(nameof(Tolerance), typeof(double), typeof(ScrollByMouse), new PropertyMetadata(300d));

        public double MaxStep
        {
            get { return (double)GetValue(MaxStepProperty); }
            set { SetValue(MaxStepProperty, value); }
        }
        public static readonly DependencyProperty MaxStepProperty =
            DependencyProperty.Register(nameof(MaxStep), typeof(double), typeof(ScrollByMouse), new PropertyMetadata(2d));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(ScrollByMouse), new PropertyMetadata(Orientation.Vertical));

        #endregion

        #region Methods

        protected void BindEvent()
        {
            if (!IsAssociatedObjectLoaded) {
                return;
            }

            AssociatedObject.MouseMove += AssociatedObjectMouseMove;
        }

        protected void UnbindEvent()
        {
            if (!IsAssociatedObjectLoaded) {
                return;
            }

            AssociatedObject.MouseMove -= AssociatedObjectMouseMove;
        }

        /// <summary>
        /// Called when the <see cref="Behavior{T}.AssociatedObject"/> is loaded.
        /// </summary>
        protected override void OnAssociatedObjectLoaded()
        {
            ScrollViewer = (VisualTreeHelper.GetChild(AssociatedObject, 0) as Decorator).Child as ScrollViewer;

            if (IsScrollEnabled) {
                BindEvent();
            }
        }

        private void AssociatedObjectMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!IsScrollEnabled) {
                UnbindEvent();

                return;
            }

            double tolerance = Tolerance;
            double maxStep = MaxStep;

            if (Orientation == Orientation.Horizontal) {
                double position = e.GetPosition(AssociatedObject).X;

                if (position < tolerance) {
                    double offset = Math.Min(maxStep, tolerance / position - 0.95);
                    ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset - offset);
                } else if (position > AssociatedObject.ActualWidth - tolerance) {
                    double offset = Math.Min(maxStep, tolerance / (AssociatedObject.ActualWidth - position) - 0.95);
                    ScrollViewer.ScrollToHorizontalOffset(ScrollViewer.HorizontalOffset + offset);
                }
            } else {
                double position = e.GetPosition(AssociatedObject).Y;

                if (position < tolerance) {
                    double offset = Math.Min(maxStep, tolerance / position - 0.95);
                    ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - offset);
                } else if (position > AssociatedObject.ActualHeight - tolerance) {
                    double offset = Math.Min(maxStep, tolerance / (AssociatedObject.ActualHeight - position) - 0.95);
                    ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset + offset);
                }
            }
        }

        #endregion
    }
}
