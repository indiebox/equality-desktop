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
using Equality.Models;
using Equality.MVVM;
using Equality.Services;
using Equality.Validation;

namespace Equality.ViewModels
{
    public partial class BoardPageViewModel : ViewModel
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

                Columns[1].CantMoveCardMessages.Add("Тестовое сообщение о запрете перемещения.");
            });
        }

        #endregion

        protected INavigationService NavigationService { get; set; }

        public BoardPageViewModel(INavigationService navigationService, IColumnService columnService, ICardService cardService)
        {
            NavigationService = navigationService;
            RegisterColumnCommands(columnService);
            RegisterCardCommands(cardService);

            ToBoards = new(OnToBoardsExecute);
        }

        #region Properties

        public Project Project { get; set; } = StateManager.SelectedProject;

        public ObservableCollection<Column> Columns { get; set; } = new();

        public bool IsDragging => DragCard != null || DragColumn != null;

        #endregion

        #region Commands

        public Command ToBoards { get; private set; }

        private void OnToBoardsExecute()
        {
            NavigationService.Navigate<BoardsPageViewModel, ProjectPageViewModel>();
        }

        #endregion

        #region Methods

        public (Column, Card) FindColumnAndCard(ulong cardId)
        {
            Column column = null;
            Card existingCard = null;

            foreach (var col in Columns) {
                var card = col.Cards.FirstOrDefault(card => card.Id == cardId);
                if (card != null) {
                    column = col;
                    existingCard = card;

                    break;
                }
            }

            return (column, existingCard);
        }

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
            RegisterPusherForColumns();
            RegisterPusherForCards();
        }

        protected void UnsubscribePusherAsync()
        {
            UnregisterPusherForColumns();
            UnregisterPusherForCards();
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

            if (CreateColumnVm != null) {
                CreateColumnVm.ClosedAsync -= CreateColumnVmClosedAsync;
            }

            if (CreateCardVm != null) {
                CreateCardVm.ClosedAsync -= CreateCardVmClosedAsync;
            }

            await base.CloseAsync();
        }
    }
}
