﻿using System.Windows;
using System.Windows.Controls;

using Equality.Models;

namespace Equality.Controls
{
    public sealed class ColumnControl : Control
    {
        static ColumnControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColumnControl), new FrameworkPropertyMetadata(typeof(ColumnControl)));
        }

        public Column Column
        {
            get => (Column)GetValue(ColumnProperty);
            set => SetValue(ColumnProperty, value);
        }
        public static readonly DependencyProperty ColumnProperty =
          DependencyProperty.Register(nameof(Column), typeof(Column), typeof(ColumnControl), new PropertyMetadata(null));
    }
}