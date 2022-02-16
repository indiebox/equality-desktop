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
    public class CreateTeamDataWindowViewModel : ViewModel
    {
        protected Dictionary<string, Team> TeamStorage;

        protected ITeamService TeamService;

        public CreateTeamDataWindowViewModel(Dictionary<string, Team> teamStorage, ITeamService teamService)
        {
            TeamStorage = teamStorage;
            TeamService = teamService;

            CreateTeam = new TaskCommand(OnCreateTeamExecute, () => !HasErrors);
            CloseWindow = new Command(OnCloseWindowExecute);

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
                var response = await TeamService.CreateAsync(Team);

                TeamStorage.Add("Team", TeamService.Deserialize(response.Content["data"].ToString()));

                CloseWindow.Execute();
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        public Command CloseWindow { get; private set; }

        private void OnCloseWindowExecute()
        {
            this.SaveAndCloseViewModelAsync();
        }

        #endregion

        #region Validation

        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            var validator = new Validator(validationResults);

            validator.ValidateField(nameof(Team.Name), Team.Name, new()
            {
                new NotEmptyStringRule(),
                new MaxStringLengthRule(255),
            });

            if (!string.IsNullOrEmpty(Team.Description)) {
                validator.ValidateField(nameof(Team.Description), Team.Description, new()
                {
                    new MaxStringLengthRule(255),
                });
            }

            if (!string.IsNullOrEmpty(Team.Url)) {
                validator.ValidateField(nameof(Team.Url), Team.Url, new()
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
