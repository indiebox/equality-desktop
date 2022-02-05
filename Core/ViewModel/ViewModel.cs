using System;
using System.Collections.Generic;

using Catel.Collections;
using Catel.Data;
using Catel.MVVM;

namespace Equality.Core.ViewModel
{
    public class ViewModel : ViewModelBase
    {
        /// <summary>
        /// The validation token that enables validation after a call <c>Dispose()</c> method.
        /// </summary>
        private IDisposable _validationToken;

        /// <summary>
        /// Gets value indicating that Api errors should be displayed.
        /// </summary>
        protected bool NeedToDisplayApiErrors { get; private set; }

        public ViewModel() : base()
        {
            DeferValidationUntilFirstSaveCall = false;
            ValidateUsingDataAnnotations = false;
            _validationToken = SuspendValidations();

            ExcludeFromValidationDecoratedProperties();
        }

        /// <summary>
        /// Exclude all properties decorated with <see cref="ExcludeFromValidationAttribute"/> from validation.
        /// </summary>
        private void ExcludeFromValidationDecoratedProperties()
        {
            var properties = PropertyDataManager.Default.GetCatelTypeInfo(GetType()).GetCatelProperties();

            foreach (var property in properties) {
                if (property.Value.GetPropertyInfo(GetType()).IsDecoratedWithAttribute(typeof(ExcludeFromValidationAttribute))) {
                    ExcludeFromValidation(property.Key);
                }
            }
        }

        protected override void OnValidatingFields(IValidationContext validationContext)
        {
            base.OnValidatedFields(validationContext);

            var list = new List<IFieldValidationResult>();
            if (NeedToDisplayApiErrors) {
                DisplayApiErrors(list);
            }

            foreach (IFieldValidationResult item in list) {
                validationContext.Add(item);
            }
        }

        /// <summary>
        /// Validates the current object for field and business rule errors.
        /// </summary>
        /// <param name="force">
        /// If set to true, a validation is forced. When the validation is not forced, it
        /// means that when the object is already validated, and no properties have been
        /// changed, no validation actually occurs since there is no reason for any values
        /// to have changed.
        /// </param>
        /// <remarks>
        /// To check whether this object contains any errors, use the <c>HasErrors</c> property.
        /// </remarks>
        public new void Validate(bool force = false)
        {
            base.Validate(force);

            // Internally this function calls EnsureValidationIsUpToDate(), which triggers Validate() method call second time.
            // So we trigger this call so that there is no unexpected behavior in the future.
            _ = HasErrors;
        }

        /// <summary>
        /// Display the Api errors.
        /// Override this method to enable display of the Api errors.
        /// </summary>
        /// <param name="validationResults">The Api errors, add additional errors to this list.</param>
        protected virtual void DisplayApiErrors(List<IFieldValidationResult> validationResults)
        {
        }

        /// <summary>
        /// Call method to display the Api errors.
        /// </summary>
        /// <remarks>
        /// Use <see cref="DisplayApiErrors(List{IFieldValidationResult})"/> to display the Api errors.
        /// </remarks>
        public void DisplayApiErrors()
        {
            NeedToDisplayApiErrors = true;

            Validate(true);

            NeedToDisplayApiErrors = false;
        }

        /// <summary>
        /// Enable display of the validation errors and perform validation.
        /// </summary>
        /// <remarks>If validation already enabled this method do nothing and just return <see langword="false"/>.</remarks>
        /// <returns>Returns <see langword="true"/> if there are errors in a validation.</returns>
        public bool EnableValidation()
        {
            if (_validationToken != null) {
                _validationToken.Dispose();
                _validationToken = null;

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
