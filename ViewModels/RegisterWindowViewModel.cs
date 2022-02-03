using System.Threading.Tasks;

using Catel.Data;

using Catel.MVVM;

using CefSharp;

using Equality.Core.CefSharp;

namespace Equality.ViewModels
{
    public class RegisterWindowViewModel : ViewModelBase
    {
        public RegisterWindowViewModel()
        {
            Url = "http://equality/register";
        }

        public override string Title => "Регистрация";

        public string Url { get; set; }

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
