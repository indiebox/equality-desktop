using System.Threading.Tasks;

using Catel.MVVM;

using Catel.Services;

using Equality.MVVM;

namespace Equality.ViewModels
{
    public class BoardPageViewModel : ViewModel
    {
        #region DesignModeConstructor

        public BoardPageViewModel()
        {
            HandleDesignMode();
        }

        #endregion

        INavigationService NavigationService { get; set; }

        public BoardPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

            ToBoards = new Command(OnToBoardsExecute);
        }

        #region Properties



        #endregion

        #region Commands


        public Command ToBoards { get; private set; }

        private void OnToBoardsExecute()
        {
            NavigationService.Navigate<BoardsPageViewModel>();
        }

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
