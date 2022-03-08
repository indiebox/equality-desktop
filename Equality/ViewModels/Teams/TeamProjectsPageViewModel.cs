using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Catel.Collections;

using Equality.Helpers;
using Equality.MVVM;
using Equality.Models;
using Equality.Services;
using System.Net.Http;
using System.Diagnostics;

namespace Equality.ViewModels
{
    public class TeamProjectsPageViewModel : ViewModel
    {
        protected Team Team;

        protected IProjectService ProjectService;

        public TeamProjectsPageViewModel(IProjectService projectService)
        {
            ProjectService = projectService;

            //Projects.AddRange(new Project[] {
            //    new Project { Name = "Project I"},
            //    new Project { Name = "Project II"},
            //    new Project { Name = "Project III"},
            //});
        }

        #region Properties

        public ObservableCollection<Project> Projects { get; set; } = new();


        #endregion

        #region Commands



        #endregion

        #region Methods

        protected async Task LoadProjectsAsync()
        {
            try {
                var response = await ProjectService.GetProjectsAsync(Team);

                Projects.AddRange(response.Object);
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }


        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            Team = MvvmHelper.GetFirstInstanceOfViewModel<TeamPageViewModel>().Team;

            await LoadProjectsAsync();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
