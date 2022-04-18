using System.Collections.Generic;
using System.Threading.Tasks;

using Catel;
using Catel.Data;
using Catel.MVVM;

using Equality.Extensions;
using Equality.Models;
using Equality.Services;
using Equality.Validation;
using Equality.ViewModels.Base;

namespace Equality.ViewModels
{
    public class CreateCardControlViewModel : BaseCreateControlViewModel
    {
        protected Column Column;

        protected ICardService CardService;

        public CreateCardControlViewModel(Column column, ICardService cardService)
        {
            Argument.IsNotNull(nameof(column), column);
            Argument.IsNotNull(nameof(cardService), cardService);

            Column = column;
            CardService = cardService;
        }

        #region Properties

        [Model]
        public Card Card { get; set; } = new();

        [ViewModelToModel(nameof(Card))]
        [Validatable]
        public string Name { get; set; }

        #endregion

        #region Commands

        protected override async Task OkAction(object param)
        {
            var response = await CardService.CreateCardAsync(Column, Card);
            Card.SyncWith(response.Object);
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
