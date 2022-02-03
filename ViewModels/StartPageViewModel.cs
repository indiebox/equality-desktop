using System.Threading.Tasks;

using Catel.MVVM;

using Equality.Core.ApiClient.Interfaces;
using Equality.Core.StateManager;

namespace Equality.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        protected IStateManager StateManager;

        protected IApiClient ApiClient;

        public StartPageViewModel(IStateManager stateManager, IApiClient apiClient)
        {
            StateManager = stateManager;
            ApiClient = apiClient;

            GetUserName = new TaskCommand(OnGetUserNameExecuteAsync);
        }

        #region Properties

        public string Name { get; set; }

        #endregion

        #region Commands

        public TaskCommand GetUserName { get; private set; }

        private async Task OnGetUserNameExecuteAsync()
        {
            var response = await ApiClient.GetAsync("user");

            Name = "Hello, " + response.Content["data"]["name"].ToString();
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            ApiClient.WithToken(Properties.Settings.Default.api_token.ToString());

            GetUserName.Execute();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
