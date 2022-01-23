using System.Diagnostics;
using System.Threading.Tasks;

using Catel.IoC;
using Catel.MVVM;
using Catel.Services;

namespace Equality.ViewModels
{
    public class AuthorizationPageViewModel : ViewModelBase
    {
        public AuthorizationPageViewModel(/*IViewModelManager viewModelManager*/)
        {
            //System.Collections.Generic.IEnumerable<IViewModel> ViewModelsArray = viewModelManager.ActiveViewModels;
            //AuthorizationWindowViewModel currentWindowViewModel;
            //foreach (var item in ViewModelsArray)
            //{
            //    if(item is AuthorizationWindowViewModel vm)
            //    {
            //        currentWindowViewModel = vm;
            //        var test = vm.ActivePage;
            //        break;
            //    }
            //}
            //OpenForgotPassword = new Command(OnOpenForgotPasswordExecute);
        }

        public override string Title { get { return "View model title"; } }

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets

        //public Command OpenForgotPassword { get; private set; }

        //private void OnOpenForgotPasswordExecute()
        //{
        //    // TODO: Handle command logic here
        //}

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
