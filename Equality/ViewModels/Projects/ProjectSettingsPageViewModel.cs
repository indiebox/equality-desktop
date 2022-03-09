using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Data;
using Catel.MVVM;
using Catel.Services;

using Equality.Extensions;
using Equality.Http;
using Equality.Models;
using Equality.MVVM;
using Equality.Services;
using Equality.Validation;

namespace Equality.ViewModels
{
    public class ProjectSettingsPageViewModel : ViewModel
    {
        protected IOpenFileService OpenFileService;

        protected IProjectService ProjectService;

        public ProjectSettingsPageViewModel(IOpenFileService openFileService, IProjectService projectService)
        {
            OpenFileService = openFileService;
            ProjectService = projectService;

            UploadImage = new TaskCommand(OnUploadImageExecute);
            DeleteImage = new TaskCommand(OnDeleteImageExecute, () => !string.IsNullOrWhiteSpace(Image));
            UpdateSettings = new TaskCommand(OnUpdateSettingsExecuteAsync);
        }

        #region Properties

        [Model]
        public Project Project { get; set; }

        [ViewModelToModel(nameof(Project))]
        public string Image { get; set; }

        [ViewModelToModel(nameof(Project), Mode = ViewModelToModelMode.OneWay)]
        [Validatable]
        public string Name { get; set; }

        [ViewModelToModel(nameof(Project), Mode = ViewModelToModelMode.OneWay)]
        [Validatable]
        public string Description { get; set; }

        #endregion

        #region Commands

        public TaskCommand UpdateSettings { get; private set; }

        private async Task OnUpdateSettingsExecuteAsync()
        {
            if (FirstValidationHasErrors()) {
                return;
            }

            try {
                Project project = new()
                {
                    Id = Project.Id,
                    Name = Name,
                    Description = Description,
                };
                var result = await ProjectService.UpdateProjectAsync(project);

                Project.SyncWith(result.Object);
            } catch (UnprocessableEntityHttpException e) {
                HandleApiErrors(e.Errors);
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        public TaskCommand UploadImage { get; private set; }

        private async Task OnUploadImageExecute()
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
                var result = await ProjectService.SetImageAsync(Project, selectedFile.FileName);

                Project.SyncWith(result.Object);
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        public TaskCommand DeleteImage { get; private set; }

        private async Task OnDeleteImageExecute()
        {
            try {
                var result = await ProjectService.DeleteImageAsync(Project);

                Project.SyncWith(result.Object);
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
