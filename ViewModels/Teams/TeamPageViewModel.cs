using System;
using System.Threading.Tasks;

using Catel.Services;

using Equality.Core.ViewModel;
using Equality.Models;
using Equality.Core.Extensions;

namespace Equality.ViewModels
{
    public class TeamPageViewModel : ViewModel
    {
        protected INavigationService NavigationService;

        public TeamPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

            NavigationCompleted += OnNavigated;
        }

        public enum Tab
        {
            Projects,
            Members,
            Stats,
            Settings,
        }

        #region Properties

        public Team Team { get; set; }

        public Tab ActiveTab { get; set; }

        #endregion

        #region Commands

        private void OnActiveTabChanged()
        {
            switch (ActiveTab) {
                case Tab.Projects:
                default:
                    NavigationService.Navigate<TeamProjectsPageViewModel>(this);
                    break;
                case Tab.Members:
                    NavigationService.Navigate<TeamMembersPageViewModel>(this, new() { { "team", Team } });
                    break;
                case Tab.Stats:
                    break;
                case Tab.Settings:
                    break;
            }
        }

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

            OnActiveTabChanged();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
