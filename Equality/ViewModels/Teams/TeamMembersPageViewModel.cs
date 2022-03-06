using System.Collections.Generic;
using System.Threading.Tasks;

using Catel.Services;

using Equality.Extensions;
using Equality.MVVM;

namespace Equality.ViewModels
{
    public class TeamMembersPageViewModel : ViewModel
    {
        protected INavigationService NavigationService;

        public TeamMembersPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public enum Tab
        {
            Members,
            Invitations,
        }

        #region Properties

        public Tab ActiveTab { get; set; }

        public Dictionary<string, object> NavigationParameters { get; set; }

        #endregion

        #region Methods

        private void OnActiveTabChanged()
        {
            switch (ActiveTab) {
                case Tab.Members:
                default:
                    NavigationService.Navigate<TeamMembersListViewModel>(this, NavigationParameters);
                    break;
                case Tab.Invitations:
                    NavigationService.Navigate<TeamInvitationsListViewModel>(this, NavigationParameters);
                    break;
            }

            NavigationParameters = null;
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            OnActiveTabChanged();
        }

        protected override async Task CloseAsync()
        {
            await base.CloseAsync();
        }
    }
}
