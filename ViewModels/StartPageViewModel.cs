using System.Threading.Tasks;

using Catel.MVVM;

using Equality.Core.ApiClient;
using Equality.Core.ViewModel;

namespace Equality.ViewModels
{
    public class StartPageViewModel : ViewModel
    {
        protected IApiClient ApiClient;

        public StartPageViewModel(IApiClient apiClient)
        {
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
            if (StateManager.CurrentUser != null) {
                Name = StateManager.CurrentUser.Name;
            } else {
                var response = await ApiClient.WithToken(Properties.Settings.Default.api_token).GetAsync("user");

                Name = response.Content["data"]["name"].ToString();
            }
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            Name = "Test";
            //GetUserName.Execute();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
