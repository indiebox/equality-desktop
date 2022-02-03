using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

using Equality.Core.ApiClient;
using Equality.Core.ApiClient.Interfaces;

namespace Equality.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        public string Name { get; set; }
        protected IStateManager StateManager;
        protected IApiClient ApiClient;

        public StartPageViewModel(IStateManager stateManager, IApiClient apiClient)
        {
            StateManager = stateManager;
            ApiClient = apiClient;
            ApiClient.WithToken(Properties.Settings.Default.api_token.ToString());
            GetUserName = new TaskCommand(OnGetUserNameExecuteAsync);
            GetUserName.Execute();
        }

        public TaskCommand GetUserName { get; private set; }

        // TODO: Move code below to constructor
        // TODO: Move code above to constructor

        private async Task OnGetUserNameExecuteAsync()
        {
            var response = await ApiClient.GetAsync("user");
            Name = "Hello, " + response.Content["data"]["name"].ToString();
        }
    }
}
