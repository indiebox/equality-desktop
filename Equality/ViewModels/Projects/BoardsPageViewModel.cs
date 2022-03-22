using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Collections;
using Catel.Services;

using Equality.Data;
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

        public BoardsPageViewModel(IBoardService boardService)
        {
            BoardService = boardService;
        }

        #region Properties

        public ObservableCollection<Board> Boards { get; set; } = new();

        #endregion

        #region Commands



        #endregion

        #region Methods

        protected async Task LoadBoardsAsync()
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

            await LoadBoardsAsync();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
