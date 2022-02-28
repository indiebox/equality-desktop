using System.Threading.Tasks;

using Catel.Services;

using Equality.Core.Extensions;
using Equality.Core.ViewModel;

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

        #endregion

        #region Commands



        #endregion

        #region Methods

        private void OnActiveTabChanged()
        {
            switch (ActiveTab) {
                case Tab.Members:
                default:
                    NavigationService.Navigate<TeamMembersListViewModel>(this);
                    break;
                case Tab.Invitations:
                    NavigationService.Navigate<TeamInvitationsListViewModel>(this);
                    break;
            }
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
