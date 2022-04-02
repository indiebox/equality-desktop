using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Catel;
using Catel.Data;
using Catel.MVVM;

using Equality.Data;
using Equality.Extensions;
using Equality.Http;
using Equality.Models;
using Equality.MVVM;
using Equality.Services;
using Equality.Validation;

namespace Equality.ViewModels
{
    public class CreateCardControlViewModel : ViewModel
    {
        protected Column Column;

        protected ICardService CardService;

        public CreateCardControlViewModel(Column column, ICardService cardService)
        {
            Argument.IsNotNull(nameof(column), column);
            Argument.IsNotNull(nameof(cardService), cardService);

            Column = column;
            CardService = cardService;

            CreateCard = new(OnCreateCardExecute, () => !HasErrors);
            CloseWindow = new(OnCloseWindowExecute);
        }

        #region Properties

        [Model]
        public Card Card { get; set; } = new();

        [ViewModelToModel(nameof(Card))]
        [Validatable]
        public string Name { get; set; }

        #endregion

        #region Commands

        public TaskCommand CreateCard { get; private set; }

        private async Task OnCreateCardExecute()
        {
            if (FirstValidationHasErrors()) {
                return;
            }

            try {
                var response = await CardService.CreateCardAsync(Column, Card);
                Card.SyncWith(response.Object);

                await SaveViewModelAsync();
                await CloseViewModelAsync(true);
            } catch (UnprocessableEntityHttpException e) {
                HandleApiErrors(e.Errors);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
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

            // TODO: subcribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
