using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Net.Http;

using Catel.Collections;
using Catel.MVVM;
using Catel.Fody;

using Equality.Helpers;
using Equality.MVVM;
using Equality.Models;
using Equality.Services;
using Equality.Data;
using Equality.Http;

namespace Equality.ViewModels
{
    public class TeamProjectsPageViewModel : ViewModel
    {
        protected IProjectService ProjectService;

        #region DesignModeConstructor

        public TeamProjectsPageViewModel()
        {
            HandleDesignMode(() =>
            {
                Projects.AddRange(new Project[] {
                    new Project { Name = "Project I"},
                    new Project { Name = "Project II"},
                    new Project { Name = "Project III"}
                });
            });
        }

        #endregion

        public TeamProjectsPageViewModel(IProjectService projectService)
        {
            ProjectService = projectService;

            LoadMoreProjects = new(OnLoadMoreProjectsExecuteAsync, () => ProjectsPaginator?.HasNextPage ?? false);
            OpenProjectPage = new Command<Project>(OnOpenOpenProjectPageExecute);
            CreateProject = new TaskCommand(OnCreateProjectExecuteAsync, () => CreateProjectVm is null);
        }

        #region Properties

        public string FilterText { get; set; }

        private async void OnFilterTextChanged()
        {
            var text = FilterText?.ToLower()?.Trim();
            if (text == CurrentFilterText) {
                return;
            }

            CurrentFilterText = text;
            await LoadProjectsAsync();
        }

        [NoWeaving]
        public string CurrentFilterText { get; set; }

        public ObservableCollection<Project> Projects { get; set; } = new();

        public PaginatableApiResponse<Project> ProjectsPaginator { get; set; }

        public CreateProjectControlViewModel CreateProjectVm { get; set; }

        #endregion

        #region Commands

        public TaskCommand LoadMoreProjects { get; private set; }

        private async Task OnLoadMoreProjectsExecuteAsync()
        {
            try {
                ProjectsPaginator = await ProjectsPaginator.NextPageAsync();

                if (IsClosed) {
                    return;
                }

                Projects.AddRange(ProjectsPaginator.Object);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        public TaskCommand CreateProject { get; private set; }

        private async Task OnCreateProjectExecuteAsync()
        {
            CreateProjectVm = MvvmHelper.CreateViewModel<CreateProjectControlViewModel>();
            CreateProjectVm.ClosedAsync += CreateProjectVmClosedAsync;
        }

        private Task CreateProjectVmClosedAsync(object sender, ViewModelClosedEventArgs e)
        {
            if (CreateProjectVm.Result) {
                Projects.Insert(0, CreateProjectVm.Project);
            }

            CreateProjectVm.ClosedAsync -= CreateProjectVmClosedAsync;
            CreateProjectVm = null;

            return Task.CompletedTask;
        }

        #endregion

        #region Methods

        public Command<Project> OpenProjectPage { get; private set; }

        private void OnOpenOpenProjectPageExecute(Project project)
        {
            StateManager.SelectedProject = project;

            var vm = MvvmHelper.GetFirstInstanceOfViewModel<ApplicationWindowViewModel>();
            vm.ActiveTab = ApplicationWindowViewModel.Tab.Project;
        }

        protected async Task LoadProjectsAsync()
        {
            var query = new QueryParameters()
            {
                Fields = new[]
                {
                    new Field("projects", "id", "name", "description", "image")
                }
            };

            if (!string.IsNullOrWhiteSpace(CurrentFilterText)) {
                query.Filters = new[] { new Filter("name", CurrentFilterText) };
            }

            try {
                ProjectsPaginator = await ProjectService.GetProjectsAsync(StateManager.SelectedTeam, query);
                
                if (IsClosed) {
                    return;
                }
                
                Projects.ReplaceRange(ProjectsPaginator.Object);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
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
            if (CreateProjectVm != null) {
                CreateProjectVm.ClosedAsync -= CreateProjectVmClosedAsync;
            }

            await base.CloseAsync();
        }
    }
}
