using System.Threading.Tasks;

using Catel.MVVM;

namespace Equality.ViewModels
{
    public class AuthorizationWindowViewModel : ViewModelBase
    {
        public string ActievePage { get; set; }
        public AuthorizationWindowViewModel(/* dependency injection here */)
        {
            ActievePage = "/Views/AuthorizationPage.xaml";
        }

        // public override string Title { get { return "SUA"; } }

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
