﻿using System.Collections.ObjectModel;
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

            OpenProjectPage = new Command<Project>(OnOpenOpenProjectPageExecute);
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

        public Command<Project> OpenProjectPage { get; private set; }

        private void OnOpenOpenProjectPageExecute(Project project)
        {
            StateManager.SelectedProject = project;

            var vm = MvvmHelper.GetFirstInstanceOfViewModel<ApplicationWindowViewModel>();
            vm.ActiveTab = ApplicationWindowViewModel.Tab.Project;
        }

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
                var response = await ProjectService.GetProjectsAsync(StateManager.SelectedTeam, new()
                {
                    Fields = new[]
                    {
                        new Field("projects", "id", "name", "description", "image", "created_at", "updated_at")
                    }
                });

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
