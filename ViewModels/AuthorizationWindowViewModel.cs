using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

namespace Equality.ViewModels
{
    public class AuthorizationWindowViewModel : ViewModelBase
    {
        protected INavigationService NavigationService;

        public AuthorizationWindowViewModel(INavigationService service)
        {
            NavigationService = service;

            NavigationService.Navigate<LoginPageViewModel>();
        }

        public override string Title => "Авторизация";

        // TODO: Register models with the vmpropmodel codesnippet
        // TODO: Register view model properties with the vmprop or vmpropviewmodeltomodel codesnippets
        // TODO: Register commands with the vmcommand or vmcommandwithcanexecute codesnippets

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
