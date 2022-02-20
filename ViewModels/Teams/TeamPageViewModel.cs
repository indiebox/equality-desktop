﻿using System.Threading.Tasks;

using Catel.IoC;
using Catel.Services;

using Equality.Core.ViewModel;

namespace Equality.ViewModels
{
    class TeamPageViewModel : ViewModel
    {
        public TeamPageViewModel()
        {
        }

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
