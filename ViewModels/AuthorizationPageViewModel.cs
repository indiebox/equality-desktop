using System.Threading.Tasks;

using Catel.IoC;
using Catel.MVVM;
using Catel.Services;

namespace Equality.ViewModels
{
    public class AuthorizationPageViewModel : ViewModelBase
    {
        public AuthorizationPageViewModel(/* dependency injection here */)
        {
        }

        public override string Title { get { return "View model title"; } }

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets

        public Command OpenForgotPasswordPage { get; set; }
        private void OnOpenForgotPasswordPage()
        {
            IDependencyResolver dependencyResolver = this.GetDependencyResolver();
            INavigationService navigationService = dependencyResolver.Resolve<INavigationService>();
            navigationService.Navigate<ForgotPasswordPageViewModel>();
        }

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
