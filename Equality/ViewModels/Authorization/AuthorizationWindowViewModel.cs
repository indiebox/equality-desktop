using System.Threading.Tasks;

using Catel.Services;

using Equality.MVVM;

using MaterialDesignThemes.Wpf;

namespace Equality.ViewModels
{
    public class AuthorizationWindowViewModel : ViewModel
    {

        protected INavigationService NavigationService;

        public AuthorizationWindowViewModel(INavigationService navigationService)
        {
            ITheme theme = _paletteHelper.GetTheme();
            IBaseTheme baseTheme;
            string currentThemeString = Properties.Settings.Default.current_theme;

            switch (currentThemeString) {
                case "Light":
                    Properties.Settings.Default.current_theme = "Light";

                    baseTheme = new MaterialDesignLightTheme();
                    theme.SetBaseTheme(baseTheme);
                    _paletteHelper.SetTheme(theme);

                    break;
                case "Dark":
                    Properties.Settings.Default.current_theme = "Dark";

                    baseTheme = new MaterialDesignDarkTheme();
                    theme.SetBaseTheme(baseTheme);
                    _paletteHelper.SetTheme(theme);

                    break;
                case "Sync":
                    Properties.Settings.Default.current_theme = "Sync";
                    break;
            }



            NavigationService = navigationService;
        }
        private readonly PaletteHelper _paletteHelper = new PaletteHelper();

        public override string Title => "Equality";

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            NavigationService.Navigate<LoginPageViewModel>();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
