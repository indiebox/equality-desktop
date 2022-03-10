using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Collections;

using Equality.Services;
using Equality.Models;
using Equality.MVVM;

namespace Equality.ViewModels
{
    public class ProjectPageViewModel : ViewModel
    {
        ITeamService TeamService;

        IProjectService ProjectService;

        public ProjectPageViewModel(ITeamService teamService, IProjectService projectService)
        {
            TeamService = teamService;

            ProjectService = projectService;
        }

        public override string Title => "View model title";

        #region Properties

        public ObservableCollection<Team> TeamsWithProjects { get; set; } = new();

        #endregion

        #region Commands



        #endregion

        #region Methods

        protected async Task LoadMembersAsync()
        {
            try {
                var response = await TeamService.GetTeamsAsync();

                foreach (var team in response.Object) {

                    var responseProjects = await ProjectService.GetProjectsAsync(team);

                    team.Projects = responseProjects.Object;

                    TeamsWithProjects.Add(team);
                }

            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
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
