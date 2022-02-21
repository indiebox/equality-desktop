using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

using Equality.Core.Extensions;
using Equality.Core.Helpers;
using Equality.Core.ViewModel;
using Equality.Models;
using Equality.Services;

namespace Equality.ViewModels
{
    public class TeamSettingsPageViewModel : ViewModel
    {
        protected IOpenFileService OpenFileService;

        protected ITeamService TeamService;

        public TeamSettingsPageViewModel(IOpenFileService openFileService, ITeamService teamService)
        {
            Team = MvvmHelper.GetFirstInstanceOfViewModel<TeamPageViewModel>().Team;

            OpenFileService = openFileService;
            TeamService = teamService;

            UploadLogo = new TaskCommand(OnUploadLogoExecute);
            DeleteLogo = new TaskCommand(OnDeleteLogoExecute, () => !string.IsNullOrWhiteSpace(Logo));
        }

        #region Properties

        [Model]
        public Team Team { get; set; }

        [ViewModelToModel(nameof(Team))]
        public string Logo { get; set; }

        #endregion

        #region Commands

        public TaskCommand UploadLogo { get; private set; }

        private async Task OnUploadLogoExecute()
        {
            DetermineOpenFileContext file = new()
            {
                Title = "Выберите изображение",
                CheckFileExists = true,
                CheckPathExists = true,
                Filter = "Image|*.jpg;*.jpeg;*.png"
            };
            var selectedFile = await OpenFileService.DetermineFileAsync(file);

            if (!selectedFile.Result) {
                return;
            }

            try {
                var result = await TeamService.SetLogoAsync(Team, selectedFile.FileName);

                Team.SyncWith(result.Object);
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        public TaskCommand DeleteLogo { get; private set; }

        private async Task OnDeleteLogoExecute()
        {
            try {
                var result = await TeamService.DeleteLogoAsync(Team);

                Team.SyncWith(result.Object);
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion

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
