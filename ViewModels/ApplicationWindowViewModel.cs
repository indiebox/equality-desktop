using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

using Equality.Core.ViewModel;
using Equality.Services;

namespace Equality.ViewModels
{
    class ApplicationWindowViewModel : ViewModel
    {
        protected IUIVisualizerService UIVisualizerService;

        protected INavigationService NavigationService;

        protected IUserService UserService;

        public ApplicationWindowViewModel(IUIVisualizerService uIVisualizerService, INavigationService navigationService, IUserService userService)
        {
            UIVisualizerService = uIVisualizerService;
            NavigationService = navigationService;
            UserService = userService;

            Logout = new TaskCommand(OnLogoutExecute);
        }

        public override string Title => "Equality";

        #region Properties

        public int ActiveTabIndex { get; set; }

        #endregion

        #region Commands

        public TaskCommand Logout { get; private set; }

        private async Task OnLogoutExecute()
        {
            try {
                await UserService.LogoutAsync();

                Properties.Settings.Default.api_token = null;
                Properties.Settings.Default.Save();

                await UIVisualizerService.ShowAsync<AuthorizationWindowViewModel>();
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion

        #region Methods

        private void OnActiveTabIndexChanged()
        {
            switch (ActiveTabIndex) {
                case 0:
                default:
                    NavigationService.Navigate<StartPageViewModel>();
                    break;
                case 2:
                    NavigationService.Navigate<ProjectsPageViewModel>();
                    break;
            }
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            OnActiveTabIndexChanged();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
