using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

using Catel.Data;
using Catel.MVVM;

using Equality.Extensions;
using Equality.Helpers;
using Equality.Http;
using Equality.Models;
using Equality.MVVM;
using Equality.Services;
using Equality.Validation;

namespace Equality.ViewModels
{
    public class CreateProjectControlViewModel : ViewModel
    {
        protected Team Team;

        protected IProjectService ProjectService;

        public CreateProjectControlViewModel(IProjectService projectService)
        {
            ProjectService = projectService;

            CreateProject = new TaskCommand<KeyEventArgs>(OnCreateProjectExecute);
            CloseWindow = new TaskCommand(OnCloseWindowExecute);
        }

        #region Properties

        [Model]
        public Project Project { get; set; } = new();

        [ViewModelToModel(nameof(Project))]
        [Validatable]
        public string Name { get; set; }

        #endregion

        #region Commands

        public TaskCommand<KeyEventArgs> CreateProject { get; private set; }

        private async Task OnCreateProjectExecute(KeyEventArgs args)
        {
            if (args != null) {
                if (args.Key == Key.Escape) {
                    CloseWindow.Execute();
                }

                if (args.Key != Key.Enter) {
                    return;
                }
            }

            if (FirstValidationHasErrors() || HasErrors) {
                return;
            }

            try {
                var response = await ProjectService.CreateProjectAsync(Team, Project);
                Project.SyncWith(response.Object);

                await SaveViewModelAsync();
                await CloseViewModelAsync(true);
            } catch (UnprocessableEntityHttpException e) {
                HandleApiErrors(e.Errors);
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        public TaskCommand CloseWindow { get; private set; }

        private async Task OnCloseWindowExecute()
        {
            await CancelViewModelAsync();
            await CloseViewModelAsync(false);
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
