using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Collections;

using Equality.Core.ViewModel;
using Equality.Models;
using Equality.Services;

namespace Equality.ViewModels
{
    public class StartPageViewModel : ViewModel
    {
        protected IInviteService InviteService;

        public StartPageViewModel(IInviteService inviteService)
        {
            InviteService = inviteService;

            Name = StateManager.CurrentUser.Name;
        }

        #region Properties

        public string Name { get; set; }

        public ObservableCollection<Invite> Invites { get; set; } = new();

        #endregion

        #region Commands

        #endregion

        #region Methods

        protected async Task LoadInvitesAsync()
        {
            try {
                var response = await InviteService.GetUserInvitesAsync();

                Invites.AddRange(response.Object);
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            await LoadInvitesAsync();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
