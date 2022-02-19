using System;
using System.Threading.Tasks;

using Equality.Core.ViewModel;
using Equality.Models;

namespace Equality.ViewModels
{
    public class TeamPageViewModel : ViewModel
    {
        public TeamPageViewModel()
        {
            NavigationCompleted += OnNavigated;
        }

        #region Properties

        public Team Team { get; set; }

        #endregion

        #region Methods

        private void OnNavigated(object sender, EventArgs e)
        {
            Team = NavigationContext.Values["team"] as Team;
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
