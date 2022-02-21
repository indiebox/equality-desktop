using System;
using System.Threading.Tasks;

using Catel.Services;

using Equality.Core.ViewModel;
using Equality.Models;
using Equality.Core.Extensions;
using Catel.MVVM;
using System.Diagnostics;
using System.Net.Http;
using Equality.Services;

namespace Equality.ViewModels
{
    public class TeamPageViewModel : ViewModel
    {
        protected INavigationService NavigationService;

        protected IOpenFileService OpenFileService;

        protected ITeamService TeamService;

        public TeamPageViewModel(INavigationService navigationService, IOpenFileService openFileService, ITeamService teamService)
        {
            NavigationService = navigationService;
            OpenFileService = openFileService;
            TeamService = teamService;

            UploadLogo = new TaskCommand(OnUploadLogoExecute);
            DeleteLogo = new TaskCommand(OnDeleteLogoExecute, () => !string.IsNullOrWhiteSpace(Team.Logo));

            NavigationCompleted += OnNavigated;
        }

        public enum Tab
        {
            Projects,
            Members,
            Stats,
            Settings,
        }

        #region Properties

        public Team Team { get; set; }

        public Tab ActiveTab { get; set; }

        #endregion

        #region Commands



        #endregion

        #region Methods

        private void OnNavigated(object sender, EventArgs e)
        {
            Team = NavigationContext.Values["team"] as Team;
        }

        private void OnActiveTabChanged()
        {
            switch (ActiveTab) {
                case Tab.Projects:
                default:
                    NavigationService.Navigate<TeamProjectsPageViewModel>(this);
                    break;
                case Tab.Members:
                    NavigationService.Navigate<TeamMembersPageViewModel>(this, new() { { "team", Team } });
                    break;
                case Tab.Stats:
                    break;
                case Tab.Settings:
                    NavigationService.Navigate<TeamSettingsPageViewModel>(this);
                    break;
            }
        }

        public TaskCommand UploadLogo { get; private set; }

        private async Task OnUploadLogoExecute()
        {
            DetermineOpenFileContext file = new()
            {
                Title = "Выберите изображение",
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Image|*.jpg;*.jpeg;*.png"
            };
            var selectedFile = await OpenFileService.DetermineFileAsync(file);

            try {
                var result = await TeamService.SetLogoAsync(Team, selectedFile.FileName);

                Team = result.Object;
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        public TaskCommand DeleteLogo { get; private set; }

        private async Task OnDeleteLogoExecute()
        {
            try {
                var result = await TeamService.DeleteLogoAsync(Team);

                Team = result.Object;
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
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
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
