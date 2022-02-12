using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

using Equality.Core.ViewModel;
using Equality.Services;

namespace Equality.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        protected IUIVisualizerService UIVisualizerService;

        protected IUserService UserService;

        protected IViewModelFactory ViewModelFactory;

        public MainWindowViewModel(IUIVisualizerService uIVisualizerService, IUserService userService, IViewModelFactory viewModelFactory)
        {
            UIVisualizerService = uIVisualizerService;
            UserService = userService;
            ViewModelFactory = viewModelFactory;

            Logout = new TaskCommand(OnLogoutExecute);

            OnActiveTabIndexChanged();
        }

        public override string Title => "Equality";

        #region Properties

        public Dictionary<int, IViewModel> ViewModelTabs { get; set; } = new();

        public IViewModel ViewModelTab { get; set; }

        public int ActiveTabIndex { get; set; }

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

                await UIVisualizerService.ShowAsync<AuthorizationWindowViewModel>();
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion

        #region Methods

        private void OnActiveTabIndexChanged()
        {
            if (!ViewModelTabs.ContainsKey(ActiveTabIndex)) {
                IViewModel vm = ActiveTabIndex switch
                {
                    0 => ViewModelFactory.CreateViewModel<StartPageViewModel>(null),
                    _ => null,
                };

                ViewModelTabs.Add(ActiveTabIndex, vm);
            }

            ViewModelTab = ViewModelTabs[ActiveTabIndex];
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
