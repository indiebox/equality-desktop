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
        protected ICardService CardService { get; set; }

        #region Properties

        public Card DragCard { get; set; }

        /// <summary>
        /// The column in which the draggable card.
        /// </summary>
        public Column DraggableCardColumn { get; set; }

        public CreateCardControlViewModel CreateCardVm { get; set; }

        public Column ColumnForNewCard { get; set; }

        public Card EditableCard { get; set; }

        [Validatable]
        public string NewCardName { get; set; }

        #endregion

        #region Commands

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
            CreateCardVm.ClosedAsync -= CreateCardVmClosedAsync;

            if (CreateCardVm.Result) {
                ColumnForNewCard.Cards.Add(CreateCardVm.Card);

                // Open control again.
                OpenCreateCardWindow.Execute(ColumnForNewCard);
            } else {
                CreateCardVm = null;
                ColumnForNewCard = null;
            }

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
                HandleApiErrors(e.Errors, new() { { "name", nameof(NewCardName) } });
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
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
                var afterCard = DraggableCardColumn.Cards
                   .TakeWhile(card => card.Id != DragCard.Id)
                   .LastOrDefault();

                await CardService.UpdateCardOrderAsync(DragCard, afterCard);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        #endregion UpdateCardOrder

        #region MoveCard

        public TaskCommand MoveCardToColumn { get; private set; }

        private async Task OnMoveCardToColumnAsync()
        {
            if (DragCard == null || DraggableCardColumn == null) {
                return;
            }
            try {
                var afterCard = DraggableCardColumn.Cards
                   .TakeWhile(card => card.Id != DragCard.Id)
                   .LastOrDefault();

                await CardService.MoveCardToColumnAsync(DragCard, DraggableCardColumn, afterCard);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        #endregion MoveCard

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
                ExceptionHandler.Handle(e);
            }
        }

        #endregion DeleteCard

        #endregion

        #region Methods

        public void MoveCard(Card card, Column oldColumn, Column newColumn, int index)
        {
            oldColumn.Cards.Remove(card);
            newColumn.Cards.Insert(index, card);
        }

        protected void RegisterCardCommands(ICardService cardService)
        {
            CardService = cardService;

            OpenCreateCardWindow = new(OnOpenCreateCardWindowExecuteAsync);
            StartEditCard = new(OnStartEditCardExecuteAsync);
            SaveNewCardName = new(OnSaveNewCardNameExecuteAsync, () => GetFieldErrors(nameof(NewCardName)) == string.Empty);
            CancelEditCard = new(OnCancelEditCardExecute);
            UpdateCardOrder = new(OnUpdateCardOrderExecuteAsync);
            DeleteCard = new(OnDeleteCardExecuteAsync);
            MoveCardToColumn = new(OnMoveCardToColumnAsync);
        }

        protected async void RegisterPusherForCards()
        {
            await CardService.SubscribeCreateCardAsync(StateManager.SelectedBoard, (Card card, ulong columnId, ulong? afterCardId) =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    var column = Columns.FirstOrDefault(col => col.Id == columnId);
                    if (column == null) {
                        return;
                    }

                    if (afterCardId == null) {
                        column.Cards.Add(card);

                        return;
                    }

                    if (afterCardId == 0) {
                        column.Cards.Insert(0, card);

                        return;
                    }

                    var afterColumn = column.Cards.FirstOrDefault(col => col.Id == afterCardId);
                    if (afterColumn == null) {
                        column.Cards.Add(card);
                    } else {
                        column.Cards.Insert(column.Cards.IndexOf(afterColumn) + 1, card);
                    }
                });
            });
            await CardService.SubscribeUpdateCardAsync(StateManager.SelectedBoard, (Card updatedCard) =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    var (_, card) = FindColumnAndCard(updatedCard.Id);

                    if (card != null) {
                        card.SyncWithOnly(updatedCard, new string[]
                        {
                            nameof(updatedCard.Name),
                            nameof(updatedCard.CreatedAt),
                            nameof(updatedCard.UpdatedAt),
                        });
                    }
                });
            });
            await CardService.SubscribeUpdateCardOrderAsync(StateManager.SelectedBoard, (ulong cardId, ulong afterId) =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    var (column, card) = FindColumnAndCard(cardId);
                    if (column == null || card == null) {
                        return;
                    }

                    if (afterId == 0) {
                        column.Cards.Move(column.Cards.IndexOf(card), 0);

                        return;
                    }

                    var afterCard = column.Cards.FirstOrDefault(card => card.Id == afterId);
                    if (afterCard != null) {
                        // If we move card to up, then we need to +1 to our index,
                        // so we insert new card after the specified one, not before.
                        int cardIndex = column.Cards.IndexOf(card);
                        int afterCardIndex = column.Cards.IndexOf(afterCard);
                        if (cardIndex > afterCardIndex) {
                            afterCardIndex++;
                        }

                        column.Cards.Move(cardIndex, afterCardIndex);
                    }
                });
            });
            await CardService.SubscribeMoveCardToColumnAsync(StateManager.SelectedBoard, (ulong cardId, ulong columnId, ulong afterId) =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    var newColumn = Columns.FirstOrDefault(c => c.Id == columnId);
                    if (newColumn == null) {
                        return;
                    }

                    var (oldColumn, card) = FindColumnAndCard(cardId);
                    if (oldColumn == null || card == null) {
                        return;
                    }

                    if (afterId == 0) {
                        MoveCard(card, oldColumn, newColumn, 0);

                        return;
                    }

                    var afterCard = newColumn.Cards.FirstOrDefault(card => card.Id == afterId);
                    if (afterCard != null) {
                        MoveCard(card, oldColumn, newColumn, newColumn.Cards.IndexOf(afterCard) + 1);
                    }
                });
            });
            await CardService.SubscribeDeleteCardAsync(StateManager.SelectedBoard, (ulong cardId) =>
            {
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    var (column, card) = FindColumnAndCard(cardId);
                    if (column == null || card == null) {
                        return;
                    }

                    // Disable dragging mode for the card.
                    if (DragCard.Id == cardId) {
                        DragCard = null;
                        DraggableCardColumn = null;
                    }

                    column.Cards.Remove(card);
                });
            });
        }

        protected void UnregisterPusherForCards()
        {
            CardService.UnsubscribeCreateCard(StateManager.SelectedBoard);
            CardService.UnsubscribeUpdateCard(StateManager.SelectedBoard);
            CardService.UnsubscribeUpdateCardOrder(StateManager.SelectedBoard);
            CardService.UnsubscribeMoveCardToColumn(StateManager.SelectedBoard);
            CardService.UnsubscribeDeleteCard(StateManager.SelectedBoard);
        }

        #endregion
    }
}
