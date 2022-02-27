using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Collections;
using Catel.MVVM;
using Catel.Services;

using Equality.Core.Helpers;
using Equality.Core.ViewModel;
using Equality.Models;
using Equality.Services;

using MaterialDesignThemes.Wpf;

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
            Participants,
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
                case Tab.Participants:
                default:
                    NavigationService.Navigate<TeamMembersListViewModel>();
                    break;
                case Tab.Invitations:
                    NavigationService.Navigate<TeamInvitationsListViewModel>();
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
