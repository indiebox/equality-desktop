using System.Threading.Tasks;

using Equality.MVVM;

namespace Equality.ViewModels
{
    public class LeaveTeamDialogViewModel : ViewModel
    {
        #region DesignModeConstructor

        public LeaveTeamDialogViewModel()
        {
            HandleDesignMode();
        }

        #endregion

        public LeaveTeamDialogViewModel(bool isLastMember)
        {
            IsLastMember = isLastMember;
        }

        #region Properties

        public bool IsLastMember { get; set; }

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
