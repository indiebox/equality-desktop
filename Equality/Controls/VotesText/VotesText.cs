using System;
using System.Windows;
using System.Windows.Controls;

using Catel.Data;
using Catel.MVVM;

namespace Equality.Controls
{
    public class VotesText : Control
    {
        static VotesText()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(VotesText), new FrameworkPropertyMetadata(typeof(VotesText)));
        }

        public string Votes
        {
            get => (string)GetValue(VotesProperty);
            set => SetValue(VotesProperty, value);
        }
        public static readonly DependencyProperty VotesProperty =
          DependencyProperty.Register(nameof(Votes), typeof(string), typeof(VotesText), new PropertyMetadata("0 голосов"));
    }
}