using System.Threading.Tasks;

using Catel.IoC;
using Catel.MVVM;
using Catel.Services;

namespace Equality.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        protected INavigationService NavigationService;

        public LoginPageViewModel(INavigationService service)
        {
            NavigationService = service;

            OpenForgotPassword = new Command(OnOpenForgotPasswordExecute);
            OpenRegisterWindow = new TaskCommand(OnOpenRegisterWindowExecute);
        }

        public override string Title => "Вход";

        public Command OpenForgotPassword { get; private set; }

        private void OnOpenForgotPasswordExecute()
        {
            NavigationService.Navigate<ForgotPasswordPageViewModel>();
        }

        public TaskCommand OpenRegisterWindow { get; private set; }

        private async Task OnOpenRegisterWindowExecute()
        {
            var uiVisualizerService = this.GetDependencyResolver().Resolve<IUIVisualizerService>();
            var vm = this.GetTypeFactory().CreateInstanceWithParametersAndAutoCompletion<RegisterWindowViewModel>();

            await uiVisualizerService.ShowAsync(vm);
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
