using Catel.Services;

namespace Equality.ViewModels.Design
{
    public class DesignForgotPasswordPageViewModel : ForgotPasswordPageViewModel
    {
        public DesignForgotPasswordPageViewModel(INavigationService service) : base(service)
        {
            Title = "Восстановление пароля";
        }
    }
}
