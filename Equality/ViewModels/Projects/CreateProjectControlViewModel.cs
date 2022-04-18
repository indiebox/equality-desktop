using System.Collections.Generic;
using System.Threading.Tasks;

using Catel;
using Catel.Data;
using Catel.MVVM;

using Equality.Data;
using Equality.Extensions;
using Equality.Http;
using Equality.Models;
using Equality.Services;
using Equality.Validation;
using Equality.ViewModels.Base;

namespace Equality.ViewModels
{
    public class CreateProjectControlViewModel : BaseCreateControlViewModel
    {
        protected Team Team = StateManager.SelectedTeam;

        protected IProjectService ProjectService;

        public CreateProjectControlViewModel(IProjectService projectService)
        {
            ProjectService = projectService;
        }

        public CreateProjectControlViewModel(Team team, IProjectService projectService) : this(projectService)
        {
            Argument.IsNotNull(nameof(team), team);
            Team = team;
        }

        #region Properties

        [Model]
        public Project Project { get; set; } = new();

        [ViewModelToModel(nameof(Project))]
        [Validatable]
        public string Name { get; set; }

        #endregion

        #region Commands

        protected override async Task OkAction(object param)
        {
            var response = await ProjectService.CreateProjectAsync(Team, Project, new()
            {
                Fields = new[]
                {
                    new Field("projects", "id", "name", "description", "image")
                }
            });
            Project.SyncWith(response.Object);
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
