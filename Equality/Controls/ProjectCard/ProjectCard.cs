using System.Windows;
using System.Windows.Controls;

using Equality.Models;

namespace Equality.Controls
{
    public class ProjectCard : Button
    {
        public enum CardSize
        {
            Custom,
            Medium,
            Big,
        }

        static ProjectCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProjectCard), new FrameworkPropertyMetadata(typeof(ProjectCard)));
        }

        public Project Project
        {
            get => (Project)GetValue(ProjectProperty);
            set => SetValue(ProjectProperty, value);
        }
        public static readonly DependencyProperty ProjectProperty =
          DependencyProperty.Register(nameof(Project), typeof(Project), typeof(ProjectCard), new PropertyMetadata(null));

        public CardSize Size
        {
            get => (CardSize)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }
        public static readonly DependencyProperty SizeProperty =
          DependencyProperty.Register(nameof(Size), typeof(CardSize), typeof(ProjectCard), new PropertyMetadata(CardSize.Custom));
    }
}
