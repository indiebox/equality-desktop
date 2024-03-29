﻿using System.Net.Http;
using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

using Equality.Extensions;
using Equality.MVVM;
using Equality.Models;
using Equality.Services;
using Equality.Data;

namespace Equality.ViewModels
{
    public class TeamPageViewModel : ViewModel
    {
        protected INavigationService NavigationService;

        protected IOpenFileService OpenFileService;

        protected ITeamService TeamService;

        #region DesignModeConstructor

        public TeamPageViewModel()
        {
            HandleDesignMode(() =>
            {
                Name = StateManager.SelectedTeam.Name;
            });
        }

        #endregion

        public TeamPageViewModel(INavigationService navigationService, IOpenFileService openFileService, ITeamService teamService)
        {
            NavigationService = navigationService;
            OpenFileService = openFileService;
            TeamService = teamService;

            UploadLogo = new TaskCommand(OnUploadLogoExecute);
            DeleteLogo = new TaskCommand(OnDeleteLogoExecute, () => !string.IsNullOrWhiteSpace(Logo));

            Team = StateManager.SelectedTeam;
        }

        public enum Tab
        {
            Projects,
            Members,
            Stats,
            Settings,
        }

        #region Properties

        public Tab ActiveTab { get; set; }

        [Model]
        public Team Team { get; set; }

        [ViewModelToModel(nameof(Team))]
        public string Name { get; set; }

        [ViewModelToModel(nameof(Team))]
        public string Logo { get; set; }

        #endregion

        #region Commands

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

            if (!selectedFile.Result) {
                return;
            }

            try {
                var result = await TeamService.SetLogoAsync(Team, selectedFile.FileName);

                Team.Logo = result.Object.Logo;
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        public TaskCommand DeleteLogo { get; private set; }

        private async Task OnDeleteLogoExecute()
        {
            try {
                var result = await TeamService.DeleteLogoAsync(Team);

                Team.Logo = null;
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        #endregion

        #region Methods

        private void OnActiveTabChanged()
        {
            switch (ActiveTab) {
                case Tab.Projects:
                default:
                    NavigationService.Navigate<TeamProjectsPageViewModel>(this);
                    break;
                case Tab.Members:
                    NavigationService.Navigate<TeamMembersPageViewModel>(this);
                    break;
                case Tab.Stats:
                    break;
                case Tab.Settings:
                    NavigationService.Navigate<TeamSettingsPageViewModel>(this);
                    break;
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
