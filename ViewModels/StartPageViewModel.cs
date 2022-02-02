using System;
using System.Collections.Generic;
using System.Text;

using Catel.MVVM;
using Catel.Services;

using Equality.Core.ApiClient.Interfaces;

namespace Equality.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        public string Name { get; set; }
        public IStateManager StateManager;

        public StartPageViewModel(IStateManager stateManager)
        {
            StateManager = stateManager;
            Name = "Hello, " + StateManager.User.Name;
        }
    }
}
