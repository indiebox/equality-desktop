using System.Threading.Tasks;

using Equality.Core.ViewModel;

namespace Equality.ViewModels
{
    public class CreateTeamDataWindowViewModel : ViewModel
    {
        public CreateTeamDataWindowViewModel()
        {
        }

        public override string Title => "Equality";

        #region Properties

        public string Name { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

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
