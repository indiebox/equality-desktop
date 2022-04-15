using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Collections;
using Catel.Data;
using Catel.MVVM;
using Catel.Services;
using Catel.ExceptionHandling;

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
    public class BoardPageViewModel : ViewModel
    {
        #region DesignModeConstructor

        public BoardPageViewModel()
        {
            HandleDesignMode(() =>
            {
                Columns.AddRange(new Column[] {
                    new Column()
                    {
                        Name = "Column1",
                        Cards = new()
                        {
                            new Card() { Name = "Card 1" },
                            new Card() { Name = "Card with very long name. Card with very long name. Card with very long name." },
                        }
                    },
                    new Column()
                    {
                        Name = "Column2",
                        Cards = new()
                        {
                            new Card() { Name = "Card 1" },
                            new Card() { Name = "Card 2" },
                            new Card() { Name = "Card 3" },
                        }
                    },
                    new Column()
                    {
                        Name = "Empty column",
                    },
                });
            });

        }

        #endregion

        protected INavigationService NavigationService { get; set; }

        protected IColumnService ColumnService { get; set; }

        protected ICardService CardService { get; set; }

        public BoardPageViewModel(INavigationService navigationService, IColumnService columnService, ICardService cardService)
        {
            NavigationService = navigationService;
            ColumnService = columnService;
            CardService = cardService;

            ToBoards = new(OnToBoardsExecute);
            OpenCreateColumnWindow = new(OnOpenCreateColumnWindowExecuteAsync);
            OpenCreateCardWindow = new(OnOpenCreateCardWindowExecuteAsync);

            StartEditColumn = new(OnStartEditColumnExecuteAsync);
            SaveNewColumnName = new(OnSaveNewColumnNameExecuteAsync, () => EditableColumn != null && GetFieldErrors("name") == string.Empty);
            CancelEditColumn = new(OnCancelEditColumnExecute);
            UpdateColumnOrder = new(OnUpdateColumnOrderExecuteAsync);
            DeleteColumn = new(OnDeleteColumnExecuteAsync);

            StartEditCard = new(OnStartEditCardExecuteAsync);
            SaveNewCardName = new(OnSaveNewCardNameExecuteAsync, () => EditableCard != null && GetFieldErrors("name") == string.Empty);
            CancelEditCard = new(OnCancelEditCardExecute);
            UpdateCardOrder = new(OnUpdateCardOrderExecuteAsync);
            DeleteCard = new(OnDeleteCardExecuteAsync);
        }

        #region Properties

        public Project Project { get; set; } = StateManager.SelectedProject;

        [Model]
        public Board Board { get; set; }

        public ObservableCollection<Column> Columns { get; set; } = new();

        #region ColumnProperties

        public Column DragColumn { get; set; }

        public Card DragCard { get; set; }

        public CreateColumnControlViewModel CreateColumnVm { get; set; }

        public Column EditableColumn { get; set; }

        [Validatable]
        public string NewColumnName { get; set; }

        #endregion ColumnProperties

        #region CardProperties

        public CreateCardControlViewModel CreateCardVm { get; set; }

        public Column ColumnForNewCard { get; set; }

        public Card EditableCard { get; set; }

        [Validatable]
        public string NewCardName { get; set; }

        #endregion CardProperties

        #endregion

        #region Commands

        public Command ToBoards { get; private set; }

        private void OnToBoardsExecute()
        {
            NavigationService.Navigate<BoardsPageViewModel, ProjectPageViewModel>();
        }

        #region CreateColumn

        public TaskCommand OpenCreateColumnWindow { get; private set; }

        private async Task OnOpenCreateColumnWindowExecuteAsync()
        {
            CreateColumnVm = MvvmHelper.CreateViewModel<CreateColumnControlViewModel>();
            CreateColumnVm.ClosedAsync += CreateColumnVmClosedAsync;
        }

        private Task CreateColumnVmClosedAsync(object sender, ViewModelClosedEventArgs e)
        {
            if (e.Result ?? false) {
                Columns.Add(CreateColumnVm.Column);
            }

            CreateColumnVm.ClosedAsync -= CreateColumnVmClosedAsync;
            CreateColumnVm = null;

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
                HandleApiErrors(e.Errors);
            } catch (HttpRequestException e) {
                Data.ExceptionHandler.Handle(e);
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
                var afterColumn = Columns.Contains(DragColumn)
                    ? Columns.TakeWhile(col => col.Id != DragColumn.Id).LastOrDefault()
                    : null;
                await ColumnService.UpdateColumnOrderAsync(DragColumn, afterColumn);
            } catch (HttpRequestException e) {
                Data.ExceptionHandler.Handle(e);
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
                Data.ExceptionHandler.Handle(e);
            }
        }

        #endregion DeleteColumn

        #region CreateCard

        public TaskCommand<Column> OpenCreateCardWindow { get; private set; }

        private async Task OnOpenCreateCardWindowExecuteAsync(Column column)
        {
            if (CreateCardVm != null) {
                CreateCardVm.ClosedAsync -= CreateCardVmClosedAsync;
            }

            ColumnForNewCard = column;

            CreateCardVm = MvvmHelper.CreateViewModel<CreateCardControlViewModel>(ColumnForNewCard);
            CreateCardVm.ClosedAsync += CreateCardVmClosedAsync;
        }

        private Task CreateCardVmClosedAsync(object sender, ViewModelClosedEventArgs e)
        {
            if (e.Result ?? false) {
                ColumnForNewCard.Cards.Add(CreateCardVm.Card);
            }

            CreateCardVm.ClosedAsync -= CreateCardVmClosedAsync;
            CreateCardVm = null;
            ColumnForNewCard = null;

            return Task.CompletedTask;
        }

        #endregion CreateCard

        #region EditCard

        public Command<Card> StartEditCard { get; private set; }

        private void OnStartEditCardExecuteAsync(Card card)
        {
            NewCardName = card.Name;
            EditableCard = card;
        }

        public Command CancelEditCard { get; private set; }

        private void OnCancelEditCardExecute()
        {
            EditableCard = null;
            NewCardName = null;
            Validate(true);
        }

        public TaskCommand SaveNewCardName { get; private set; }

        private async Task OnSaveNewCardNameExecuteAsync()
        {
            if (FirstValidationHasErrors()) {
                return;
            }

            if (NewCardName == EditableCard.Name) {
                CancelEditCard.Execute();

                return;
            }

            try {
                Card card = new()
                {
                    Id = EditableCard.Id,
                    Name = NewCardName,
                };

                var response = await CardService.UpdateCardAsync(card);

                EditableCard.SyncWith(response.Object);

                CancelEditCard.Execute();
            } catch (UnprocessableEntityHttpException e) {
                HandleApiErrors(e.Errors);
            } catch (HttpRequestException e) {
                Data.ExceptionHandler.Handle(e);
            }
        }

        #endregion EditCard

        #region UpdateCardOrder

        public TaskCommand UpdateCardOrder { get; private set; }

        private async Task OnUpdateCardOrderExecuteAsync()
        {
            if (DragCard == null) {
                return;
            }

            try {

                var afterColumn = Columns
                    .Where(column => column.Cards.Contains(DragCard))
                    .FirstOrDefault();
                Card afterCard = null;
                if (afterColumn != null) {
                    afterCard = afterColumn.Cards
                       .TakeWhile(card => card.Id != DragCard.Id)
                       .LastOrDefault();
                }
                await CardService.UpdateCardOrderAsync(DragCard, afterCard);
            } catch (HttpRequestException e) {
                Data.ExceptionHandler.Handle(e);
            }
        }

        #endregion UpdateCardOrder

        #region DeleteCard

        public TaskCommand<Card> DeleteCard { get; private set; }

        private async Task OnDeleteCardExecuteAsync(Card card)
        {
            var view = new Views.DeleteCardDialog();
            bool result = (bool)await MaterialDesignThemes.Wpf.DialogHost.Show(view);
            if (!result) {
                return;
            }

            try {
                await CardService.DeleteCardAsync(card);

                foreach (var column in Columns) {
                    column.Cards.Remove(card);
                }
            } catch (HttpRequestException e) {
                Data.ExceptionHandler.Handle(e);
            }
        }

        #endregion DeleteCard

        #endregion

        #region Methods

        protected async Task LoadColumnsAsync()
        {
            try {
                var response = await ColumnService.GetColumnsAsync(StateManager.SelectedBoard);
                Columns.AddRange(response.Object);

                foreach (var column in Columns) {
                    var cards = (await CardService.GetCardsAsync(column)).Object;
                    column.Cards.AddRange(cards);
                }

            } catch (HttpRequestException e) {
                Data.ExceptionHandler.Handle(e);
            }
        }

        protected async Task SubscribePusherAsync()
        {
            await ColumnService.SubscribeCreateColumnAsync(StateManager.SelectedBoard, (Column col, ulong? afterColumnId) =>
            {
                App.Current.Dispatcher.Invoke(() =>
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
                App.Current.Dispatcher.Invoke(() =>
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
                App.Current.Dispatcher.Invoke(() =>
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
                App.Current.Dispatcher.Invoke(() =>
                {
                    Columns.Remove(Columns.FirstOrDefault(col => col.Id == columnId));
                });
            });
        }

        protected void UnsubscribePusherAsync()
        {
            ColumnService.UnsubscribeCreateColumn(StateManager.SelectedBoard);
            ColumnService.UnsubscribeUpdateColumn(StateManager.SelectedBoard);
            ColumnService.UnsubscribeUpdateColumnOrder(StateManager.SelectedBoard);
            ColumnService.UnsubscribeDeleteColumn(StateManager.SelectedBoard);
        }

        #endregion

        #region Validation

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            var validator = new Validator(validationResults);

            if (EditableCard != null) {
                validator.ValidateField(nameof(NewCardName), NewCardName, new()
                {
                    new NotEmptyStringRule(),
                    new MaxStringLengthRule(255),
                });
            }

            if (EditableColumn != null) {
                validator.ValidateField(nameof(NewColumnName), NewColumnName, new()
                {
                    new NotEmptyStringRule(),
                    new MaxStringLengthRule(255),
                });
            }
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            await LoadColumnsAsync();
            await Data.ExceptionHandler.Service.ProcessWithRetryAsync(SubscribePusherAsync);
        }

        protected override async Task CloseAsync()
        {
            UnsubscribePusherAsync();

            await base.CloseAsync();
        }
    }
}
