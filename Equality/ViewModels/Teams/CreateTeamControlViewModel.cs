using System.Collections.Generic;
using System.Threading.Tasks;

using Catel.Data;
using Catel.MVVM;

using Equality.Http;
using Equality.Extensions;
using Equality.Validation;
using Equality.Models;
using Equality.Services;
using Equality.ViewModels.Base;

namespace Equality.ViewModels
{
    public class CreateTeamControlViewModel : BaseCreateControlViewModel
    {
        protected ITeamService TeamService;

        public CreateTeamControlViewModel(ITeamService teamService)
        {
            TeamService = teamService;
        }

        #region Properties

        [Model]
        public Team Team { get; set; } = new();

        [ViewModelToModel(nameof(Team))]
        [Validatable]
        public string Name { get; set; }

        #endregion

        #region Commands

        protected override async Task OkAction(object param)
        {
            var response = await TeamService.CreateAsync(Team, new()
            {
                Fields = new[]
                    {
                        new Field("teams", "id", "name", "description", "url", "logo")
                    }
            });
            Team.SyncWith(response.Object);
        }

        protected override bool OnOkCommandCanExecute(object param) => !HasErrors;

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
    }
}
