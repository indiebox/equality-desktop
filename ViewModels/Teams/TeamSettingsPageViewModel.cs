using System.Threading.Tasks;

using Equality.Core.Helpers;
using Equality.Core.ViewModel;
using Equality.Models;

namespace Equality.ViewModels
{
    public class TeamSettingsPageViewModel : ViewModel
    {
        protected Team Team;

        public TeamSettingsPageViewModel()
        {
            Team = MvvmHelper.GetFirstInstanceOfViewModel<TeamPageViewModel>().Team;
        }

        #region Properties



        #endregion

        #region Commands



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
