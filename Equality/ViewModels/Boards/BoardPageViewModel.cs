using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Collections;
using Catel.Data;
using Catel.MVVM;

using Catel.Services;

using Equality.Controls;
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
            DeleteColumn = new(OnDeleteColumnExecuteAsync);
            StartEditCard = new(OnStartEditCardExecuteAsync);
            CancelEditCard = new(OnCancelEditCardExecute);
            SaveNewCardName = new(OnSaveNewCardNameExecuteAsync, () => !HasErrors);
            DeleteCard = new(OnDeleteCardExecuteAsync);

            ApiFieldsMap = new Dictionary<string, string>()
            {
                { nameof(NewCardName), "name" },
            };
        }

        #region Properties


        [Model]
        public Board Board { get; set; }

        public Project Project { get; set; } = StateManager.SelectedProject;

        public ObservableCollection<Column> Columns { get; set; } = new();

        public CreateColumnControlViewModel CreateColumnVm { get; set; }

        public CreateCardControlViewModel CreateCardVm { get; set; }

        public Column ColumnForNewCard { get; set; }

        public Card EditableCard { get; set; }

        [Validatable]
        public string NewCardName { get; set; }

        public ColumnControl DragColumn { get; set; }

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
                ExceptionHandler.Handle(e);
            }
        }

        #endregion EditCard

        #region DeleteCard

        public TaskCommand<Card> DeleteCard { get; private set; }

        private async Task OnDeleteCardExecuteAsync(Card card)
        {
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
                ExceptionHandler.Handle(e);
            }
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
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            await LoadColumnsAsync();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
