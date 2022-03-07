﻿using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel;
using Catel.MVVM;
using Catel.Services;

using Equality.MVVM;
using Equality.Models;
using Equality.Services;
using Equality.Data;
using Notification.Wpf;

namespace Equality.ViewModels
{
    public class ApplicationWindowViewModel : ViewModel
    {
        protected IUIVisualizerService UIVisualizerService;

        protected INavigationService NavigationService;

        protected IUserService UserService;

        public ApplicationWindowViewModel(IUIVisualizerService uIVisualizerService, INavigationService navigationService, IUserService userService)
        {
            UIVisualizerService = uIVisualizerService;
            NavigationService = navigationService;
            UserService = userService;

            Logout = new TaskCommand(OnLogoutExecute);
        }

        public override string Title => "Equality";

        public enum Tab
        {
            Main,
            Projects,
            Team,
            Project,
            TempProject,
        }

        #region Properties

        public Tab ActiveTab { get; set; }

        public Team SelectedTeam { get; set; }

        #endregion

        #region Commands

        public TaskCommand Logout { get; private set; }

        private async Task OnLogoutExecute()
        {
            try {
                await UserService.LogoutAsync();

                StateManager.ApiToken = null;
                StateManager.CurrentUser = null;

                Properties.Settings.Default.api_token = null;
                Properties.Settings.Default.Save();

                await UIVisualizerService.ShowAsync<AuthorizationWindowViewModel>();
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion

        #region Methods

        private void OnActiveTabChanged()
        {
            switch (ActiveTab) {
                case Tab.Main:
                default:
                    NavigationService.Navigate<StartPageViewModel>();
                    break;
                case Tab.Projects:
                    NavigationService.Navigate<ProjectsPageViewModel>();
                    break;
                case Tab.TempProject:
                    NavigationService.Navigate<ProjectPageViewModel>();
                    break;
                case Tab.Team:
                    Argument.IsNotNull(nameof(SelectedTeam), SelectedTeam);

                    NavigationService.Navigate<TeamPageViewModel>(new() { { "team", SelectedTeam } });
                    break;
            }
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            OnActiveTabChanged();

            var notif = new NotificationManager();

            var content = new NotificationContent();
            content.Title = "Test 1";
            content.Message = "Msg";
            content.Type = NotificationType.Error;
            content.Icon = new MaterialDesignThemes.Wpf.PackIcon
            { Kind = MaterialDesignThemes.Wpf.PackIconKind.Error, Height = 25, Width = 25 };
            notif.Show(content, "NotificationsContainer", System.TimeSpan.MaxValue);

            content = new NotificationContent();
            content.Title = "Test 2";
            content.Message = "Msg";
            content.Type = NotificationType.Success;
            content.Icon = new MaterialDesignThemes.Wpf.PackIcon
            { Kind = MaterialDesignThemes.Wpf.PackIconKind.Check, Height = 25, Width = 25 };
            notif.Show(content, "NotificationsContainer", System.TimeSpan.MaxValue);

            content = new NotificationContent();
            content.Title = "Test 3";
            content.Message = "Msg";
            content.Type = NotificationType.Information;
            content.Icon = new MaterialDesignThemes.Wpf.PackIcon
            { Kind = MaterialDesignThemes.Wpf.PackIconKind.Information, Height = 25, Width = 25 };
            notif.Show(content, "NotificationsContainer", System.TimeSpan.MaxValue);

            content = new NotificationContent();
            content.Title = "Test 3";
            content.Message = "Msg";
            content.Type = NotificationType.Warning;
            content.Icon = new MaterialDesignThemes.Wpf.PackIcon
            { Kind = MaterialDesignThemes.Wpf.PackIconKind.Warning, Height = 25, Width = 25 };
            notif.Show(content, "NotificationsContainer", System.TimeSpan.MaxValue);
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
