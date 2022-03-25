using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

using Catel;
using Catel.Logging;
using Catel.Reflection;
using Catel.Windows.Interactivity;

using Microsoft.Xaml.Behaviors;

namespace Equality.Behaviors
{
    /// <summary>
    /// Behavior to set focus to a <see cref="FrameworkElement"/> and select all text if element
    /// is <see cref="TextBox"/> or <see cref="PasswordBox"/>.
    /// </summary>
    public class FocusAndSelectText : Focus
    {
        #region Fields

        /// <summary>
        /// The log.
        /// </summary>
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private Catel.IWeakEventListener _weakEventListener;

        private readonly DispatcherTimer _timer = new DispatcherTimer();

        #endregion

        public FocusAndSelectText()
        {
        }

        #region Properties

        /// <summary>
        /// Gets or sets the source. This value is required when the <see cref="FocusMoment" /> property is either 
        /// <see cref="FocusMoment.PropertyChanged" /> or <see cref="FocusMoment.Event" />.
        /// </summary>
        /// <value>The source.</value>
        public new object Source
        {
            get { return (object)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for Source.  This enables animation, styling, binding, etc... 
        /// </summary>
        public new static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(object), typeof(FocusAndSelectText),
            new PropertyMetadata(null, (sender, e) => ((FocusAndSelectText)sender).OnSourceChanged(e)));

        /// <summary>
        /// Gets or sets the name of the property. This value is required when the <see cref="FocusMoment" /> property is 
        /// <see cref="FocusMoment.PropertyChanged" />.
        /// </summary>
        /// <value>The name of the property.</value>
        public new string PropertyName
        {
            get { return (string)GetValue(PropertyNameProperty); }
            set { SetValue(PropertyNameProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for PropertyName.  This enables animation, styling, binding, etc... 
        /// </summary>
        public new static readonly DependencyProperty PropertyNameProperty =
            DependencyProperty.Register(nameof(PropertyName), typeof(string), typeof(FocusAndSelectText), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the name of the event. This value is required when the <see cref="FocusMoment" /> property is 
        /// <see cref="FocusMoment.Event" />.
        /// </summary>
        /// <value>The name of the event.</value>
        public new string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for EventName.  This enables animation, styling, binding, etc...
        /// </summary>
        public static new readonly DependencyProperty EventNameProperty =
            DependencyProperty.Register(nameof(EventName), typeof(string), typeof(FocusAndSelectText), new PropertyMetadata(null));

        /// <summary>
        /// Gets a value indicating whether this instance is focus already set.
        /// </summary>
        /// <value><c>true</c> if this instance is focus already set; otherwise, <c>false</c>.</value>
        protected new bool IsFocusAlreadySet { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Called when the <see cref="Behavior{T}.AssociatedObject"/> is loaded.
        /// </summary>
        protected override void OnAssociatedObjectLoaded()
        {
            if (!IsFocusAlreadySet && (FocusMoment == FocusMoment.Loaded)) {
                StartFocus();
            }
        }

        /// <summary>
        /// Starts the focus.
        /// </summary>
        protected new void StartFocus()
        {
            var focusDelay = FocusDelay;
            if (focusDelay > 5000) {
                focusDelay = 5000;
            }

            Log.Debug("Starting focus on element '{0}' with a delay of '{1}' ms", AssociatedObject.GetType().GetSafeFullName(false), focusDelay);

            if (focusDelay > 25) {
                _timer.Stop();
                _timer.Tick -= OnTimerTick;

                _timer.Interval = new TimeSpan(0, 0, 0, 0, focusDelay);
                _timer.Tick += OnTimerTick;
                _timer.Start();
            } else {
                if (SetFocus()) {
                    IsFocusAlreadySet = true;
                }
            }
        }

        /// <summary>
        /// Called when the <see cref="DispatcherTimer.Tick" /> event occurs on the timer.
        /// </summary>
        private void OnTimerTick(object sender, EventArgs e)
        {
            IsFocusAlreadySet = true;

            _timer.Stop();
            _timer.Tick -= OnTimerTick;

            SetFocus();
        }

        /// <summary>
        /// Sets the focus to the assoicated object.
        /// </summary>
        private bool SetFocus()
        {
            if (!IsEnabled) {
                return false;
            }

            if (AssociatedObject.Focus()) {
                Log.Debug("Focused '{0}'", AssociatedObject.GetType().GetSafeFullName(false));

                if (AssociatedObject is TextBox textBox) {
                    textBox.SelectAll();
                }

                if (AssociatedObject is PasswordBox passwordBox) {
                    passwordBox.SelectAll();
                }

                return true;
            }

            Log.Debug("Failed to focus '{0}'", AssociatedObject.GetType().GetSafeFullName(false));

            return false;
        }

        /// <summary>
        /// Called when the event on the <see cref="Source" /> has occurred.
        /// </summary>
        private void OnSourceEventOccurred()
        {
            StartFocus();
        }

        /// <summary>
        /// Called when a property on the <see cref="Source" /> has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == PropertyName) {
                StartFocus();
            }
        }

        /// <summary>
        /// Called when the source has changed.
        /// </summary>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is not null) {
                switch (FocusMoment) {
                    case FocusMoment.Event:
                        _weakEventListener?.Detach();
                        _weakEventListener = null;
                        break;

                    case FocusMoment.PropertyChanged:
                        var sourceAsPropertyChanged = e.OldValue as INotifyPropertyChanged;
                        if (sourceAsPropertyChanged is not null) {
                            sourceAsPropertyChanged.PropertyChanged -= OnSourcePropertyChanged;
                        } else {
                            Log.Warning("Cannot unsubscribe from previous source because it does not implement 'INotifyPropertyChanged', this should not be possible and can lead to memory leaks");
                        }
                        break;
                }
            }

            if (e.NewValue is not null) {
                switch (FocusMoment) {
                    case FocusMoment.Event:
                        if (string.IsNullOrEmpty(EventName)) {
                            throw new InvalidOperationException("Property 'EventName' is required when FocusMode is 'FocusMode.Event'");
                        }

                        _weakEventListener = this.SubscribeToWeakEvent(Source, EventName, OnSourceEventOccurred);
                        break;

                    case FocusMoment.PropertyChanged:
                        if (string.IsNullOrEmpty(PropertyName)) {
                            throw new InvalidOperationException("Property 'PropertyName' is required when FocusMode is 'FocusMode.PropertyChanged'");
                        }

                        var sourceAsPropertyChanged = e.NewValue as INotifyPropertyChanged;
                        if (sourceAsPropertyChanged is null) {
                            throw new InvalidOperationException("Source does not implement interface 'INotifyfPropertyChanged', either implement it or change the 'FocusMode'");
                        }

                        sourceAsPropertyChanged.PropertyChanged += OnSourcePropertyChanged;
                        break;
                }
            }
        }

        #endregion
    }
}
