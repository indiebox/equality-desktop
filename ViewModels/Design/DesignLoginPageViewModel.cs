namespace Equality.ViewModels.Design
{
    public class DesignLoginPageViewModel : LoginPageViewModel
    {
        public DesignLoginPageViewModel() : base(null, null, null)
        {
            CredentialsErrorMessage = "Ошибка данных";
        }
    }
}

