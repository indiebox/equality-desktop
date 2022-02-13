using System.Threading.Tasks;

using Equality.Core.ViewModel;

namespace Equality.ViewModels
{
    public class RegisterWindowViewModel : ViewModel
    {
        public RegisterWindowViewModel()
        {
            Url = "http://equality/register";
        }

        public override string Title => "Регистрация";

        public string Url { get; set; }

        public bool IsBrowserLoaded { get; set; }

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
