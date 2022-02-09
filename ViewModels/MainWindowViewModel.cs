using System.Diagnostics;
using System.Threading.Tasks;

using Catel.Data;

using Catel.MVVM;

using Equality.Core.ViewModel;

namespace Equality.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        //protected INavigationService NavigationService;

        public MainWindowViewModel(/*INavigationService service*/)
        {
            //NavigationService = service;

            //NavigationService.Navigate<StartPageViewModel>();
        }

        public override string Title => "Equality";

        #region Properties

        public int ActiveTabIndex { get; set; }

        private void OnActiveTabIndexChanged()
        {
            Debug.WriteLine(ActiveTabIndex);
        }

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
