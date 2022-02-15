using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Data;
using Catel.Fody;
using Catel.MVVM;

using Equality.Core.Validation;
using Equality.Core.ViewModel;
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

            ApiFieldsMap = new()
            {
                { nameof(Team.Name), "name" },
                { nameof(Team.Description), "description" },
                { nameof(Team.Url), "url" },
            };
        }

        public override string Title => "Equality";

        #region Properties

        [Model]
        [Expose("Name")]
        [Expose("Description")]
        [Expose("Url")]
        public Team Team { get; set; } = new();

        #endregion

        #region Commands

        public TaskCommand CreateTeam { get; private set; }

        private async Task OnCreateTeamExecute()
        {
            if (FirstValidationHasErrors()) {
                return;
            }

            try {
                await TeamService.CreateAsync(Team);

                // Success
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion

        #region Validation

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            var validator = new Validator(validationResults);

            validator.ValidateField(nameof(Team.Name), Team.Name, new()
            {
                // TODO: add validation rules
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
