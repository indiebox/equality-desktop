using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Catel.Collections;

using Equality.Core.Helpers;
using Equality.Core.ViewModel;
using Equality.Models;

namespace Equality.ViewModels
{
    public class TeamProjectsPageViewModel : ViewModel
    {
        protected Team Team;

        public TeamProjectsPageViewModel()
        {
            Projects.AddRange(new Project[] {
                new Project { Name = "Project I"},
                new Project { Name = "Project II"},
                new Project { Name = "Project III"},
                new Project { Name = "Project I"},
                new Project { Name = "Project II"},
                new Project { Name = "Project III"},
                new Project { Name = "Project I"},
                new Project { Name = "Project II"},
                new Project { Name = "Project III"},
                new Project { Name = "Project I"},
                new Project { Name = "Project II"},
                new Project { Name = "Project III"},
            });
        }

        #region Properties

        public ObservableCollection<Project> Projects { get; set; } = new();


        #endregion

        #region Commands



        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            Team = MvvmHelper.GetFirstInstanceOfViewModel<TeamPageViewModel>().Team;
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
