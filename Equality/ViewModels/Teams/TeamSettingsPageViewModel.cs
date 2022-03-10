using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Data;
using Catel.MVVM;
using Catel.Services;

using Equality.Http;
using Equality.Extensions;
using Equality.Helpers;
using Equality.Validation;
using Equality.MVVM;
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
            OpenFileService = openFileService;
            TeamService = teamService;

            UploadLogo = new TaskCommand(OnUploadLogoExecute);
            DeleteLogo = new TaskCommand(OnDeleteLogoExecute, () => !string.IsNullOrWhiteSpace(Logo));
            UpdateSettings = new TaskCommand(OnUpdateSettingsExecuteAsync);
        }

        #region Properties

        [Model]
        public Team Team { get; set; }

        [ViewModelToModel(nameof(Team))]
        public string Logo { get; set; }

        [ViewModelToModel(nameof(Team), Mode = ViewModelToModelMode.OneWay)]
        [Validatable]
        public string Name { get; set; }

        [ViewModelToModel(nameof(Team), Mode = ViewModelToModelMode.OneWay)]
        [Validatable]
        public string Description { get; set; }

        [ViewModelToModel(nameof(Team), Mode = ViewModelToModelMode.OneWay)]
        [Validatable]
        public string Url { get; set; }

        #endregion

        #region Commands

        public TaskCommand UpdateSettings { get; private set; }

        private async Task OnUpdateSettingsExecuteAsync()
        {
            if (FirstValidationHasErrors()) {
                return;
            }

            try {
                Team team = new()
                {
                    Id = Team.Id,
                    Name = Name,
                    Description = Description,
                    Url = Url,
                };
                var result = await TeamService.UpdateTeamAsync(team);

                Team.SyncWith(result.Object);
            } catch (UnprocessableEntityHttpException e) {
                HandleApiErrors(e.Errors);
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

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

        #region Validation

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            var validator = new Validator(validationResults);

            validator.ValidateField(nameof(Name), Name, new()
            {
                new NotEmptyStringRule(),
                new MaxStringLengthRule(255),
            });

            if (!string.IsNullOrEmpty(Description)) {
                validator.ValidateField(nameof(Description), Description, new()
                {
                    new MaxStringLengthRule(255),
                });
            }

            if (!string.IsNullOrEmpty(Url)) {
                validator.ValidateField(nameof(Url), Url, new()
                {
                    new MaxStringLengthRule(255),
                });
            }
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            Team = MvvmHelper.GetFirstInstanceOfViewModel<TeamPageViewModel>().Team;
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
