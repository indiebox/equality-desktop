using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Collections;

using Equality.Core.ViewModel;
using Equality.Models;
using Equality.Services;

namespace Equality.ViewModels
{
    public class TeamMembersPageViewModel : ViewModel
    {
        protected ITeamService TeamService;

        public TeamMembersPageViewModel(ITeamService teamService)
        {
            TeamService = teamService;
        }

        public override string Title => "Equality";

        #region Properties

        public string FilterText { get; set; }

        private void OnFilterTextChanged()
        {
            FilterMembers();
        }

        public ObservableCollection<TeamMember> Members { get; set; } = new();

        public ObservableCollection<TeamMember> FilteredMembers { get; set; } = new();

        #endregion

        #region Commands



        #endregion

        #region Methods

        protected void FilterMembers()
        {
            if (string.IsNullOrEmpty(FilterText)) {
                FilteredMembers.ReplaceRange(Members);

                return;
            }

            FilteredMembers.ReplaceRange(Members.Where(user => user.Name.ToLower().Contains(FilterText.ToLower())));
        }

        protected async Task LoadMembersAsync()
        {
            try {
                var response = await TeamService.GetMembersAsync(1);

                Members.AddRange(response.Object);

                FilterMembers();
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            await LoadMembersAsync();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
