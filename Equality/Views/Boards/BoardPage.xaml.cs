using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

using Equality.Controls;
using Equality.Models;

namespace Equality.Views
{
    public partial class BoardPage
    {
        public BoardPage()
        {
            InitializeComponent();
        }

        private void Card_MouseMove(object sender, MouseEventArgs e)
        {
            if (sender is ColumnControl column && e.LeftButton == MouseButtonState.Pressed) {
                //DragDrop.DoDragDrop(column, column.Column, DragDropEffects.Move);
                //column.
            }
        }
    }
}
