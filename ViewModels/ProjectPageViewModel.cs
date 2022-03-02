using System.Threading.Tasks;

using Equality.Core.ViewModel;

namespace Equality.ViewModels
{
    public class ProjectPageViewModel : ViewModel
    {
        public ProjectPageViewModel()
        {
        }

        public override string Title => "View model title";

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
