﻿using System.Collections.Generic;
using System.Threading.Tasks;

using Catel.Data;
using Catel.MVVM;
using Catel.Services;

using Equality.Validation;
using Equality.MVVM;

namespace Equality.ViewModels
{
    public class $safeitemname$ : ViewModel
    {
        #region DesignModeConstructor

        public $safeitemname$()
        {
            HandleDesignMode();
        }

        #endregion

        public $safeitemname$(INavigationService navigationService)
        {


            // TODO: Rename SendForm command and OnSendFormExecute method.
            SendForm = new TaskCommand(OnSendFormExecute, () => !HasErrors);
        }

        #region Properties

        [Validatable]
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
