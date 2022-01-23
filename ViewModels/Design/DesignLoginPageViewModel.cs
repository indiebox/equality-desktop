using Catel.Services;

namespace Equality.ViewModels.Design
{
    public class DesignLoginPageViewModel : LoginPageViewModel
    {
        public DesignLoginPageViewModel(INavigationService service) : base(service)
        {
            Title = "Вход";
        }
    }
}
