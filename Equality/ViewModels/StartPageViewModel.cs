using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Collections;
using Catel.MVVM;

using Equality.MVVM;
using Equality.Models;
using Equality.Services;
using Equality.Data;
using Equality.Http;
using System.Linq;
using Equality.Helpers;

namespace Equality.ViewModels
{
    public class StartPageViewModel : ViewModel
    {
        protected IInviteService InviteService;

        protected IProjectService ProjectService;

        #region DesignModeConstructor

        public StartPageViewModel()
        {
            HandleDesignMode(() =>
            {
                Invites.AddRange(new Invite[]
                {
                    new Invite()
                    {
                        Team = new Team() { Name = "Long team name Long team name" },
                        Inviter = new User() { Name = "Long user name Long user name" },
                    },
                    new Invite()
                    {
                        Team = new Team() { Name = "Test team" },
                        Inviter = new User() { Name = "User 1" },
                    },
                    new Invite()
                    {
                        Team = new Team() { Name = "Test team 2" },
                        Inviter = new User() { Name = "User 2" },
                    },
                    new Invite()
                    {
                        Team = new Team() { Name = "Test team 2" },
                        Inviter = new User() { Name = "User 2" },
                    },
                });

                RecentProjects.AddRange(new Project[]
                {
                    new Project()
                    {
                        Name = "Project 1",
                    },
                    new Project()
                    {
                        Name = "Project 2",
                    },
                });

                Name = StateManager.CurrentUser.Name;
            });
        }

        #endregion

        public StartPageViewModel(IInviteService inviteService, IProjectService projectService)
        {
            InviteService = inviteService;
            ProjectService = projectService;

            LoadMoreInvites = new(OnLoadMoreInvitesExecuteAsync, () => InvitesPaginator?.HasNextPage ?? false);
            AcceptInvite = new TaskCommand<Invite>(OnAcceptInviteExecuteAsync);
            DeclineInvite = new TaskCommand<Invite>(OnDeclineInviteExecuteAsync);
            OpenProjectPage = new(OnOpenOpenProjectPageExecute);

            Name = StateManager.CurrentUser.Name;
        }

        #region Properties

        public string Name { get; set; }

        public ObservableCollection<Invite> Invites { get; set; } = new();

        public PaginatableApiResponse<Invite> InvitesPaginator { get; set; }

        public ObservableCollection<Project> RecentProjects { get; set; } = new();

        #endregion

        #region Commands

        public TaskCommand LoadMoreInvites { get; private set; }

        private async Task OnLoadMoreInvitesExecuteAsync()
        {
            try {
                InvitesPaginator = await InvitesPaginator.NextPageAsync();

                if (IsClosed) {
                    return;
                }

                Invites.AddRange(InvitesPaginator.Object);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        public TaskCommand<Invite> AcceptInvite { get; private set; }

        private async Task OnAcceptInviteExecuteAsync(Invite invite)
        {
            try {
                await InviteService.AcceptInviteAsync(invite);

                Invites.Remove(invite);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        public TaskCommand<Invite> DeclineInvite { get; private set; }

        private async Task OnDeclineInviteExecuteAsync(Invite invite)
        {
            try {
                await InviteService.DeclineInviteAsync(invite);

                Invites.Remove(invite);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        public Command<Project> OpenProjectPage { get; private set; }

        private void OnOpenOpenProjectPageExecute(Project project)
        {
            StateManager.SelectedProject = project;

            var vm = MvvmHelper.GetFirstInstanceOfViewModel<ApplicationWindowViewModel>();
            vm.ActiveTab = ApplicationWindowViewModel.Tab.Project;
        }

        #endregion

        #region Methods

        protected async void LoadInvitesAsync()
        {
            try {
                InvitesPaginator = await InviteService.GetUserInvitesAsync();

                if (IsClosed) {
                    return;
                }

                Invites.AddRange(InvitesPaginator.Object);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        protected async void LoadRecentProjectsAsync()
        {
            bool needSave = false;

            try {
                foreach (ulong projectId in SettingsManager.RecentProjects.Reverse()) {
                    try {
                        var response = await ProjectService.GetProjectAsync(projectId);

                        RecentProjects.Add(response.Object);
                    } catch (NotFoundHttpException) {
                        needSave = true;
                        SettingsManager.RecentProjects.Remove(projectId);
                    }
                }
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }

            if (needSave) {
                Properties.Settings.Default.Save();
            }
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            LoadInvitesAsync();
            LoadRecentProjectsAsync();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
