using System;
using System.Threading.Tasks;

using Catel.MVVM;

using Equality.Data;
using Equality.Models;
using Equality.MVVM;

namespace Equality.ViewModels
{
    public class ProjectSettingsPageViewModel : ViewModel
    {
        public ProjectSettingsPageViewModel()
        {
            NavigationCompleted += OnNavigated;
        }

        public override string Title => "View model title";

        #region Properties
        [Model]
        public Project Project { get; set; }

        [ViewModelToModel(nameof(Project))]
        public string Name { get; set; }

        [ViewModelToModel(nameof(Project))]
        public string Description { get; set; }

        #endregion

        #region Commands



        #endregion

        #region Methods

        private void OnNavigated(object sender, EventArgs e)
        {
            Project = NavigationContext.Values["project"] as Project;
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
