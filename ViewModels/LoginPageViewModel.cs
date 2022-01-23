using System.Diagnostics;
using System.Threading.Tasks;
using Catel.MVVM;
using Catel.Services;

namespace Equality.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        protected INavigationService NavigationService;

        public LoginPageViewModel(INavigationService service)
        {
            OpenForgotPassword = new Command(OnOpenForgotPasswordExecute);
            NavigationService = service;
        }


        public Command OpenForgotPassword { get; private set; }

        private void OnOpenForgotPasswordExecute()
        {
            NavigationService.Navigate<ForgotPasswordPageViewModel>();
        }

        public override string Title => "Вход";

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets

        //public Command OpenForgotPasswordPage { get; set; }
        //private void OnOpenForgotPasswordPage()
        //{
        //    IDependencyResolver dependencyResolver = this.GetDependencyResolver();
        //    INavigationService navigationService = dependencyResolver.Resolve<INavigationService>();
        //    navigationService.Navigate<ForgotPasswordPageViewModel>();
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
