using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Catel.Collections;
using Catel.MVVM;
using Catel.Services;

using Equality.Helpers;
using Equality.MVVM;
using Equality.Models;
using Equality.Services;

namespace Equality.ViewModels
{
    public class ProjectsPageViewModel : ViewModel
    {
        public bool IsFiltered = false;

        protected IUIVisualizerService UIVisualizerService;

        protected ITeamService TeamService;

        public ProjectsPageViewModel(IUIVisualizerService uIVisualizerService, ITeamService teamService)
        {
            UIVisualizerService = uIVisualizerService;
            TeamService = teamService;

            OpenCreateTeamWindow = new TaskCommand(OnOpenCreateTeamWindowExecute, () => CreateTeamVm is null);
            OpenTeamPage = new Command<ITeam>(OnOpenTeamPageExecute);
            FilterProjects = new Command<ITeam>(OnFilterProjectsExecute);
            ResetFilter = new Command(OnResetFilterExecute);
        }

        #region Properties

        public ObservableCollection<ITeam> Teams { get; set; } = new();

        public ObservableCollection<ITeam> FilteredTeams { get; set; } = new();

        public CreateTeamControlViewModel CreateTeamVm { get; set; }

        #endregion

        #region Commands

        public TaskCommand OpenCreateTeamWindow { get; private set; }

        private async Task OnOpenCreateTeamWindowExecute()
        {
            CreateTeamVm = MvvmHelper.CreateViewModel<CreateTeamControlViewModel>();
            CreateTeamVm.ClosedAsync += CreateTeamVmClosedAsync;
        }

        public Command<ITeam> OpenTeamPage { get; private set; }

        private void OnOpenTeamPageExecute(ITeam team)
        {
            var vm = MvvmHelper.GetFirstInstanceOfViewModel<ApplicationWindowViewModel>();
            vm.SelectedTeam = team;
            vm.ActiveTab = ApplicationWindowViewModel.Tab.Team;
        }

        public Command<ITeam> FilterProjects { get; private set; }

        private void OnFilterProjectsExecute(ITeam filterByTeam)
        {
            IsFiltered = true;

            FilteredTeams.ReplaceRange(Teams.Where(team => team == filterByTeam));
        }

        public Command ResetFilter { get; private set; }

        private void OnResetFilterExecute()
        {
            IsFiltered = false;

            FilteredTeams.ReplaceRange(Teams);
        }

        #endregion

        #region Methods

        private Task CreateTeamVmClosedAsync(object sender, ViewModelClosedEventArgs e)
        {
            if (e.Result ?? false) {
                Teams.Add(CreateTeamVm.Team);

                if (!IsFiltered) {
                    FilteredTeams.Add(CreateTeamVm.Team);
                }
            }

            CreateTeamVm.ClosedAsync -= CreateTeamVmClosedAsync;
            CreateTeamVm = null;

            return Task.CompletedTask;
        }

        protected async void LoadTeamsAsync()
        {
            var response = await TeamService.GetTeamsAsync();

            Teams.AddRange(response.Object);
            FilteredTeams.AddRange(Teams);
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            LoadTeamsAsync();
        }

        protected override async Task CloseAsync()
        {
            if (CreateTeamVm != null) {
                CreateTeamVm.ClosedAsync -= CreateTeamVmClosedAsync;
            }

            await base.CloseAsync();
        }
    }
}
