using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.MVVM;

using Equality.Data;
using Equality.Extensions;
using Equality.Helpers;
using Equality.Http;
using Equality.Models;
using Equality.MVVM;
using Equality.Services;
using Equality.Validation;

namespace Equality.ViewModels
{
    public partial class BoardPageViewModel : ViewModel
    {
        protected IColumnService ColumnService { get; set; }

        #region Properties

        public Column DragColumn { get; set; }

        public CreateColumnControlViewModel CreateColumnVm { get; set; }

        public Column EditableColumn { get; set; }

        [Validatable]
        public string NewColumnName { get; set; }

        #endregion

        #region Commands

        #region CreateColumn

        public TaskCommand OpenCreateColumnWindow { get; private set; }

        private async Task OnOpenCreateColumnWindowExecuteAsync()
        {
            CreateColumnVm = MvvmHelper.CreateViewModel<CreateColumnControlViewModel>();
            CreateColumnVm.ClosedAsync += CreateColumnVmClosedAsync;
        }

        private Task CreateColumnVmClosedAsync(object sender, ViewModelClosedEventArgs e)
        {
            CreateColumnVm.ClosedAsync -= CreateColumnVmClosedAsync;

            if (CreateColumnVm.Result) {
                Columns.Add(CreateColumnVm.Column);

                // Open control again.
                OpenCreateColumnWindow.Execute();
            } else {
                CreateColumnVm = null;
            }

            return Task.CompletedTask;
        }

        #endregion CreateColumn

        #region EditColumn

        public Command<Column> StartEditColumn { get; private set; }

        private void OnStartEditColumnExecuteAsync(Column column)
        {
            NewColumnName = column.Name;
            EditableColumn = column;
        }

        public Command CancelEditColumn { get; private set; }

        private void OnCancelEditColumnExecute()
        {
            EditableColumn = null;
            NewColumnName = null;
            Validate(true);
        }

        public TaskCommand SaveNewColumnName { get; private set; }

        private async Task OnSaveNewColumnNameExecuteAsync()
        {
            if (FirstValidationHasErrors()) {
                return;
            }

            if (NewColumnName == EditableColumn.Name) {
                CancelEditColumn.Execute();

                return;
            }

            try {
                Column column = new()
                {
                    Id = EditableColumn.Id,
                    Name = NewColumnName,
                };

                var response = await ColumnService.UpdateColumnAsync(column);
                EditableColumn.Name = response.Object.Name;

                CancelEditColumn.Execute();
            } catch (UnprocessableEntityHttpException e) {
                HandleApiErrors(e.Errors, new() { { "name", nameof(NewColumnName) } });
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        #endregion EditColumn

        #region UpdateColumnOrder

        public TaskCommand UpdateColumnOrder { get; private set; }

        private async Task OnUpdateColumnOrderExecuteAsync()
        {
            if (DragColumn == null) {
                return;
            }

            try {
                var afterColumn = Columns
                    .TakeWhile(col => col.Id != DragColumn.Id)
                    .LastOrDefault();

                await ColumnService.UpdateColumnOrderAsync(DragColumn, afterColumn);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        #endregion UpdateColumnOrder

        #region DeleteColumn

        public TaskCommand<Column> DeleteColumn { get; private set; }

        private async Task OnDeleteColumnExecuteAsync(Column column)
        {
            var view = new Views.DeleteColumnDialog();
            bool result = (bool)await MaterialDesignThemes.Wpf.DialogHost.Show(view);
            if (!result) {
                return;
            }

            try {
                await ColumnService.DeleteColumnAsync(column);

                Columns.Remove(column);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        #endregion DeleteColumn

        #endregion

        #region Methods

        protected void RegisterColumnCommands(IColumnService columnService)
        {
            ColumnService = columnService;

            OpenCreateColumnWindow = new(OnOpenCreateColumnWindowExecuteAsync);
            StartEditColumn = new(OnStartEditColumnExecuteAsync);
            SaveNewColumnName = new(OnSaveNewColumnNameExecuteAsync, () => GetFieldErrors(nameof(NewColumnName)) == string.Empty);
            CancelEditColumn = new(OnCancelEditColumnExecute);
            UpdateColumnOrder = new(OnUpdateColumnOrderExecuteAsync);
            DeleteColumn = new(OnDeleteColumnExecuteAsync);
        }

        protected async void RegisterPusherForColumns()
        {
            await ColumnService.SubscribeCreateColumnAsync(StateManager.SelectedBoard, (Column col, ulong? afterColumnId) =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    if (afterColumnId == null) {
                        Columns.Add(col);

                        return;
                    }

                    if (afterColumnId == 0) {
                        Columns.Insert(0, col);

                        return;
                    }

                    var afterColumn = Columns.FirstOrDefault(col => col.Id == afterColumnId);
                    if (afterColumn == null) {
                        Columns.Add(col);
                    } else {
                        Columns.Insert(Columns.IndexOf(afterColumn) + 1, col);
                    }
                });
            });
            await ColumnService.SubscribeUpdateColumnAsync(StateManager.SelectedBoard, (Column column) =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    var col = Columns.FirstOrDefault(col => col.Id == column.Id);
                    if (col != null) {
                        col.SyncWithOnly(column, new string[]
                        {
                            nameof(col.Name),
                            nameof(col.CreatedAt),
                            nameof(col.UpdatedAt),
                        });
                    }
                });
            });
            await ColumnService.SubscribeUpdateColumnOrderAsync(StateManager.SelectedBoard, (ulong columnId, ulong afterId) =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    var currCol = Columns.FirstOrDefault(col => col.Id == columnId);
                    if (currCol == null) {
                        return;
                    }

                    if (afterId == 0) {
                        Columns.Remove(currCol);
                        Columns.Insert(0, currCol);

                        return;
                    }

                    var afterCol = Columns.FirstOrDefault(col => col.Id == afterId);
                    if (afterCol != null) {
                        Columns.Remove(currCol);
                        Columns.Insert(Columns.IndexOf(afterCol) + 1, currCol);
                    }
                });
            });
            await ColumnService.SubscribeDeleteColumnAsync(StateManager.SelectedBoard, (ulong columnId) =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    var col = Columns.FirstOrDefault(col => col.Id == columnId);
                    if (col == null) {
                        return;
                    }

                    // Disable dragging mode for the column.
                    if (DragColumn.Id == columnId) {
                        DragColumn = null;
                    }

                    Columns.Remove(col);
                });
            });
        }

        protected void UnregisterPusherForColumns()
        {
            ColumnService.UnsubscribeCreateColumn(StateManager.SelectedBoard);
            ColumnService.UnsubscribeUpdateColumn(StateManager.SelectedBoard);
            ColumnService.UnsubscribeUpdateColumnOrder(StateManager.SelectedBoard);
            ColumnService.UnsubscribeDeleteColumn(StateManager.SelectedBoard);
        }

        #endregion
    }
}
