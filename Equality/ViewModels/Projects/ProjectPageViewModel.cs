using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

using Equality.Extensions;
using Equality.MVVM;
using Equality.Models;
using Equality.Services;
using Catel;
using Equality.Data;

namespace Equality.ViewModels
{
    public class ProjectPageViewModel : ViewModel
    {
        protected INavigationService NavigationService;

        public ProjectPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigationCompleted += OnNavigated;
        }

        public override string Title => "View model title";

        public enum Tab
        {
            Settings,
        }

        #region Properties

        public Tab ActiveTab { get; set; }

        [Model]
        Project Project { get; set; }

        [ViewModelToModel(nameof(Project))]
        public string Name { get; set; }

        #endregion

        #region Commands



        #endregion

        #region Methods

        private void OnNavigated(object sender, EventArgs e)
        {
            Project = NavigationContext.Values["project"] as Project;
        }

        private void OnActiveTabChanged()
        {
            switch (ActiveTab) {
                case Tab.Settings:
                default:
                    Argument.IsNotNull(nameof(Project), Project);

                    NavigationService.Navigate<ProjectSettingsPageViewModel>(this, new() { { "project", Project } });
                    break;
            }
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            OnActiveTabChanged();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
