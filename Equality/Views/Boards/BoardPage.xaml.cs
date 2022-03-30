using System.Collections.ObjectModel;
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

        ColumnControl DragColumn { get; set; }

        private void Card_MouseMove(object sender, MouseEventArgs e)
        {
            ObservableCollection<Column> Columns = ((ObservableCollection<Column>)ListBoxColumns.ItemsSource);
            if (sender is ColumnControl column && e.LeftButton == MouseButtonState.Pressed) {

                if (DragColumn == null) {

                    Debug.WriteLine("DO");
                    DragColumn = column;
                    Columns.Remove(column.Column);
                } else if (DragColumn != null && DragColumn != sender) {

                    Debug.WriteLine("Drag");
                    int dragIndex = Columns.IndexOf(DragColumn.Column);
                    int senderIndex = Columns.IndexOf(column.Column);
                    if (dragIndex < senderIndex) {
                        Columns.Insert(senderIndex + 1, DragColumn.Column);
                        try {
                            Columns.RemoveAt(dragIndex);
                        } catch { }
                    } else {
                        int dragIdx = dragIndex + 1;
                        if (Columns.Count + 1 > dragIdx) {
                            Columns.Insert(senderIndex + 1, DragColumn.Column);
                            try {
                                Columns.RemoveAt(dragIdx);
                            } catch { }
                        }
                    }
                }
            }
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            Column droppedData = e.Data.GetData(typeof(Column)) as Column;
            Column target = ((ColumnControl)(sender)).DataContext as Column;

            int removedIdx = ListBoxColumns.Items.IndexOf(droppedData);
            int targetIdx = ListBoxColumns.Items.IndexOf(target);

            if (removedIdx < targetIdx) {
                ListBoxColumns.Items.Insert(targetIdx + 1, droppedData);
                ListBoxColumns.Items.Remove(removedIdx);
                //((ObservableCollection<Column>)ListBoxColumns.InputBindings).Insert(targetIdx + 1, droppedData);
                //_empList.RemoveAt(removedIdx);
            } else {
                int remIdx = removedIdx + 1;
                if (ListBoxColumns.Items.Count + 1 > remIdx) {
                    ListBoxColumns.Items.Insert(targetIdx, droppedData);
                    ListBoxColumns.Items.RemoveAt(remIdx);
                }
            }
        }
    }
}
