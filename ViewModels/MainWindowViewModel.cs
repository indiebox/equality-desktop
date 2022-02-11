using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.IoC;
using Catel.MVVM;
using Catel.Services;

using Equality.Core.ViewModel;
using Equality.Services;

namespace Equality.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        protected IUserService UserService;

        protected IViewModelFactory ViewModelFactory;

        public MainWindowViewModel(IUserService userService, IViewModelFactory viewModelFactory)
        {
            UserService = userService;
            ViewModelFactory = viewModelFactory;

            Logout = new TaskCommand(OnLogoutExecute);

            ViewModelTabs.Add(0, ViewModelFactory.CreateViewModel<StartPageViewModel>(null));
            ViewModelTab = ViewModelTabs[ActiveTabIndex];
        }

        public override string Title => "Equality";

        #region Properties

        public Dictionary<int, IViewModel> ViewModelTabs { get; set; } = new();

        public IViewModel ViewModelTab { get; set; }

        public int ActiveTabIndex { get; set; }

        private void OnActiveTabIndexChanged()
        {
            if (!ViewModelTabs.ContainsKey(ActiveTabIndex)) {
                ViewModelTab = null;

                return;

                //IViewModel vm;

                //switch (ActiveTabIndex) {
                //    case 1:
                //        vm = ViewModelFactory.CreateViewModel<StartPageViewModel>(null);
                //        break;
                //    default:
                //        break;
                //}
            }

            ViewModelTab = ViewModelTabs[ActiveTabIndex];
        }

        #endregion

        #region Commands

        public TaskCommand Logout { get; private set; }

        private async Task OnLogoutExecute()
        {
            try {
                await UserService.LogoutAsync(StateManager.ApiToken);

                StateManager.ApiToken = null;
                StateManager.CurrentUser = null;
                Properties.Settings.Default.api_token = null;
                Properties.Settings.Default.Save();

                var uiService = this.GetDependencyResolver().Resolve<IUIVisualizerService>();
                _ = uiService.ShowOrActivateAsync<AuthorizationWindowViewModel>(null, null, null);
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
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
