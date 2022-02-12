using System.Collections.Generic;
using System.Threading.Tasks;

using Catel.Data;

using Equality.Core.Validation;
using Equality.Core.ViewModel;

namespace $rootnamespace$
{
    public class $safeitemname$ : ViewModel
    {
        public $safeitemname$()
        {
            

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
