using System.Collections.ObjectModel;
using System.Diagnostics;
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
    public class BoardsPageViewModel : ViewModel
    {
        #region DesignModeConstructor

        public BoardsPageViewModel()
        {
            HandleDesignMode(() =>
            {
                Boards.AddRange(new Board[]
                {
                    new() { Id = 1, Name = "Board" },
                    new() { Id = 2, Name = "Board1" },
                });
                ;
            });
        }

        #endregion

        IBoardService BoardService;

        INavigationService NavigationService;

        public BoardsPageViewModel(IBoardService boardService, INavigationService navigationService)
        {
            BoardService = boardService;
            NavigationService = navigationService;

            OpenBoardPage = new Command<Board>(OnOpenOpenBoardPageExecute);
            OpenCreateBoardWindow = new TaskCommand(OnOpenCreateBoardWindowExecuteAsync, () => CreateBoardVm is null);
        }

        #region Properties

        public ObservableCollection<Board> Boards { get; set; } = new();

        public CreateBoardControlViewModel CreateBoardVm { get; set; }

        #endregion

        #region Commands

        public Command<Board> OpenBoardPage { get; private set; }

        private void OnOpenOpenBoardPageExecute(Board board)
        {
            StateManager.SelectedBoard = board;

            NavigationService.Navigate<BoardPageViewModel, ProjectPageViewModel>();
        }


        public TaskCommand OpenCreateBoardWindow { get; private set; }

        private async Task OnOpenCreateBoardWindowExecuteAsync()
        {
            CreateBoardVm = MvvmHelper.CreateViewModel<CreateBoardControlViewModel>();
            CreateBoardVm.ClosedAsync += CreateBoardVmClosedAsync;
        }

        #endregion

        #region Methods

        private Task CreateBoardVmClosedAsync(object sender, ViewModelClosedEventArgs e)
        {
            if (e.Result ?? false) {
                Boards.Add(CreateBoardVm.Board);
            }

            CreateBoardVm.ClosedAsync -= CreateBoardVmClosedAsync;
            CreateBoardVm = null;

            return Task.CompletedTask;
        }

        protected async void LoadBoardsAsync()
        {
            try {
                var response = await BoardService.GetBoardsAsync(StateManager.SelectedProject);

                Boards.AddRange(response.Object);

            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            LoadBoardsAsync();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            if (CreateBoardVm != null) {
                CreateBoardVm.ClosedAsync -= CreateBoardVmClosedAsync;
            }

            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
