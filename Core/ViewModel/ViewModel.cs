using System;

using Catel.Collections;
using Catel.MVVM;

namespace Equality.Core.ViewModel
{
    public class ViewModel : ViewModelBase
    {
        /// <summary>
        /// The validation token that enables validation after a call <c>Dispose()</c> method.
        /// </summary>
        private IDisposable ValidationToken;

        public ViewModel() : base()
        {
            DeferValidationUntilFirstSaveCall = false;
            ValidateUsingDataAnnotations = false;
            ValidationToken = SuspendValidations();
        }

        /// <summary>
        /// Enable the display of validation errors and perform validation.
        /// </summary>
        /// <remarks>If validation already enabled this method do nothing and just return <see langword="false"/>.</remarks>
        /// <returns>Returns <see langword="true"/> if there are errors in a validation.</returns>
        public bool EnableValidation()
        {
            if (ValidationToken != null) {
                ValidationToken.Dispose();
                ValidationToken = null;

                return HasErrors;
            }

            return false;
        }

        /// <summary>
        /// Exclude property from a validation.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <remarks>By default, all properties are considered validatable. 
        /// <code></code>
        /// It means, if any property changed, <c>ValidateFields</c> and <c>ValidateBusinessRules</c> in ViewModel would be fired.
        /// <code></code>
        /// It is recomended to use this method for optimization of Validate* method calls if needed.
        /// </remarks>
        public void ExcludeFromValidation(string propertyName)
        {
            PropertiesNotCausingValidation[GetType()].Add(propertyName);
        }

        /// <summary>
        /// Exclude properties from a validation.
        /// </summary>
        /// <param name="propertyName">Array of the property names.</param>
        /// <remarks>By default, all properties are considered validatable. 
        /// <code></code>
        /// It means, if any property changed, <c>ValidateFields</c> and <c>ValidateBusinessRules</c> in ViewModel would be fired.
        /// <code></code>
        /// It is recomended to use this method for optimization of Validate* method calls if needed.
        /// </remarks>
        public void ExcludeFromValidation(string[] propertyName)
        {
            PropertiesNotCausingValidation[GetType()].AddRange(propertyName);
        }
    }
}
