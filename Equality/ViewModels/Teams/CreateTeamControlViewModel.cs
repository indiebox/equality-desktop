using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Data;
using Catel.MVVM;

using Equality.Http;
using Equality.Extensions;
using Equality.Validation;
using Equality.MVVM;
using Equality.Models;
using Equality.Services;

namespace Equality.ViewModels
{
    public class CreateTeamControlViewModel : ViewModel
    {
        protected ITeamService TeamService;

        public CreateTeamControlViewModel(ITeamService teamService)
        {
            TeamService = teamService;

            CreateTeam = new TaskCommand(OnCreateTeamExecute, () => !HasErrors);
            Cancel = new TaskCommand(OnCancelExecute);
        }

        #region Properties

        [Model]
        public Team Team { get; set; } = new();

        [ViewModelToModel(nameof(Team))]
        public string Name { get; set; }

        #endregion

        #region Commands

        public TaskCommand CreateTeam { get; private set; }

        private async Task OnCreateTeamExecute()
        {
            if (FirstValidationHasErrors()) {
                return;
            }

            try {
                var response = await TeamService.CreateAsync(Team);
                Team.SyncWith(response.Object);

                await SaveViewModelAsync();
                await CloseViewModelAsync(true);
            } catch (UnprocessableEntityHttpException e) {
                HandleApiErrors(e.Errors);
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        public TaskCommand Cancel { get; private set; }

        private async Task OnCancelExecute()
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

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
