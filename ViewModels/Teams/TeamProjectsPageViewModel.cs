using System.Threading.Tasks;

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
        }

        #region Properties



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
