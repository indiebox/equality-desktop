using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Catel.Collections;
using Catel.Services;

using Equality.Models;
using Equality.MVVM;

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

        INavigationService NavigationService;

        public BoardsPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        #region Properties

        public ObservableCollection<Board> Boards { get; set; } = new();

        #endregion

        #region Commands



        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
