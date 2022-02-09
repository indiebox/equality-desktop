using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Catel.Data;
using Catel.IoC;
using Catel.MVVM;

using Equality.Core.ViewModel;

namespace Equality.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        //protected INavigationService NavigationService;

        protected IViewModelFactory ViewModelFactory;

        public MainWindowViewModel(IViewModelFactory viewModelFactory/*INavigationService service*/)
        {
            ViewModelFactory = viewModelFactory;

            ViewModelTabs.Add(0, ViewModelFactory.CreateViewModel<StartPageViewModel>(null));
            ViewModelTab = ViewModelTabs[ActiveTabIndex];
        }

        public override string Title => "Equality";

        #region Properties

        public Dictionary<int, IViewModel> ViewModelTabs { get; set; } = new();

        public IViewModel ViewModelTab { get; set; }

        public int ActiveTabIndex { get; set; }

        private void OnActiveTabIndexChanged()
        {
            if (!ViewModelTabs.ContainsKey(ActiveTabIndex)) {
                ViewModelTab = null;

                return;

                //IViewModel vm;

                //switch (ActiveTabIndex) {
                //    case 1:
                //        vm = ViewModelFactory.CreateViewModel<StartPageViewModel>(null);
                //        break;
                //    default:
                //        break;
                //}
            }

            ViewModelTab = ViewModelTabs[ActiveTabIndex];
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
