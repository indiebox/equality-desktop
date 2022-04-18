using System.Collections.Generic;
using System.Threading.Tasks;

using Catel.Data;
using Catel.MVVM;

using Equality.Extensions;
using Equality.Validation;
using Equality.Models;
using Equality.Services;
using Equality.Data;
using Equality.ViewModels.Base;

namespace Equality.ViewModels
{
    public class CreateColumnControlViewModel : BaseCreateControlViewModel
    {
        IColumnService ColumnService;

        public CreateColumnControlViewModel(IColumnService columnService)
        {
            ColumnService = columnService;
        }

        #region Properties

        [Model]
        public Column Column { get; set; } = new();

        [ViewModelToModel(nameof(Column))]
        [Validatable]
        public string Name { get; set; }

        #endregion

        #region Commands

        protected override async Task OkAction(object param)
        {
            var response = await ColumnService.CreateColumnAsync(StateManager.SelectedBoard, Column);
            Column.SyncWith(response.Object);
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
    }
}
