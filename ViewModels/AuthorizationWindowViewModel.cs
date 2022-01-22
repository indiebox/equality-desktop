using System.Threading.Tasks;

using Catel.IoC;
using Catel.MVVM;
using Catel.MVVM.Views;

namespace Equality.ViewModels
{
    public class AuthorizationWindowViewModel : ViewModelBase
    {
        public string ActivePage { get; set; }
        public AuthorizationWindowViewModel(IUrlLocator urlLocator)
        {
            ActivePage = urlLocator.ResolveUrl(typeof(AuthorizationPageViewModel));
        }

        public override string Title { get { return "Вход"; } }

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
