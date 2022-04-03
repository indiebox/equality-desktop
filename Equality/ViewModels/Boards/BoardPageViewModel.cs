using System.Collections;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Collections;
using Catel.MVVM;

using Catel.Services;

using Equality.Data;
using Equality.Extensions;
using Equality.Helpers;
using Equality.Models;
using Equality.MVVM;
using Equality.Services;

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
        }

        #region Properties


        [Model]
        public Board Board { get; set; }

        public ObservableCollection<Column> Columns { get; set; } = new();

        public CreateColumnControlViewModel CreateColumnVm { get; set; }

        public CreateCardControlViewModel CreateCardVm { get; set; }

        public Column ColumnForNewCard { get; set; }

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
