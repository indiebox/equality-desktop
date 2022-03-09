using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Catel.Collections;
using Catel.MVVM;

using Equality.Helpers;
using Equality.MVVM;
using Equality.Models;
using Equality.Services;
using Equality.Data;

using System.Net.Http;
using System.Diagnostics;

namespace Equality.ViewModels
{
    public class TeamProjectsPageViewModel : ViewModel
    {
        protected IProjectService ProjectService;

        public TeamProjectsPageViewModel(IProjectService projectService)
        {
            ProjectService = projectService;

            OpenCreateProjectWindow = new TaskCommand(OnOpenCreateProjectWindowExecuteAsync);
        }

        #region Properties

        public ObservableCollection<Project> Projects { get; set; } = new();

        public CreateProjectControlViewModel CreateProjectVm { get; set; }

        #endregion

        #region Commands

        public TaskCommand OpenCreateProjectWindow { get; private set; }

        private async Task OnOpenCreateProjectWindowExecuteAsync()
        {
            CreateProjectVm = MvvmHelper.CreateViewModel<CreateProjectControlViewModel>();
            CreateProjectVm.ClosedAsync += CreateProjectVmClosedAsync;
        }

        #endregion

        #region Methods

        private Task CreateProjectVmClosedAsync(object sender, ViewModelClosedEventArgs e)
        {
            if (e.Result ?? false) {
                Projects.Add(CreateProjectVm.Project);
            }

            CreateProjectVm.ClosedAsync -= CreateProjectVmClosedAsync;
            CreateProjectVm = null;

            return Task.CompletedTask;
        }

        protected async Task LoadProjectsAsync()
        {
            try {
                var response = await ProjectService.GetProjectsAsync(StateManager.SelectedTeam);

                Projects.AddRange(response.Object);
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }


        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            await LoadProjectsAsync();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
