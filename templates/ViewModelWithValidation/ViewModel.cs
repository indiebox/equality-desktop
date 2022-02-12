using System.Collections.Generic;
using System.Threading.Tasks;

using Catel.Data;
using Catel.MVVM;

using Equality.Core.Validation;
using Equality.Core.ViewModel;

namespace $rootnamespace$
{
    public class $safeitemname$ : ViewModel
    {
        public $safeitemname$()
        {
            
            
            // TODO: Rename SendForm command and OnSendFormExecute method.
            SendForm = new TaskCommand(OnSendFormExecute, () => !HasErrors);

            ApiFieldsMap = new ()
            {
                { nameof(Example), "example" },
            };
        }

        public override string Title => "View model title";

        #region Properties
        
        public string Example { get; set; }

        #endregion

        #region Commands
        
        public TaskCommand SendForm { get; private set; }

        private async Task OnSendFormExecute()
        {
            if (FirstValidationHasErrors()) {
                return;
            }

            // TODO: Handle command logic here
        }
        
        #endregion
        
        #region Validation
        
        protected override void ValidateFields(List<IFieldValidationResult> validationResults)
        {
            var validator = new Validator(validationResults);
        
            validator.ValidateField(nameof(Example), Example, new()
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
