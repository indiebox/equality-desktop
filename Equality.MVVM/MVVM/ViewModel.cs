using System;
using System.Collections.Generic;
using System.Text;

using Catel;
using Catel.Data;
using Catel.MVVM;

using Equality.Validation;

namespace Equality.MVVM
{
    public class ViewModel : ViewModelBase
    {
        /// <summary>
        /// The validation token that enables validation after a call <c>Dispose()</c> method.
        /// </summary>
        private IDisposable _validationToken;

        public ViewModel() : base()
        {
            DeferValidationUntilFirstSaveCall = false;
            _validationToken = SuspendValidations();

            ApiErrors = new();
            CatelProperties = PropertyDataManager.Default.GetCatelTypeInfo(GetType()).GetCatelProperties();

            AutomaticallyHideApiErrorsOnPropertyChanged = true;
            AutomaticallyMapApiFields = true;

            ExcludeNonDecoratedPropertiesFromValidation();
        }

        /// <summary>
        /// The Api errors for fields.
        /// </summary>
        /// <remarks>Key is the field name, Value is the first error message.</remarks>
        [ExcludeFromValidation]
        public Dictionary<string, string> ApiErrors { get; private set; }

        /// <summary>
        /// Should Api errors automatically hiding on associated property changed.
        /// </summary>
        [ExcludeFromValidation]
        protected bool AutomaticallyHideApiErrorsOnPropertyChanged { get; set; }

        /// <summary>
        /// Should Api errors trying to automatically map with CamelCased property on VM.
        /// </summary>
        [ExcludeFromValidation]
        protected bool AutomaticallyMapApiFields { get; set; }

        /// <summary>
        /// Gets the list of all properties of the view model.
        /// </summary>
        protected IDictionary<string, PropertyData> CatelProperties { get; private set; }

        /// <summary>
        /// Enable display of the validation errors and perform first validation.
        /// </summary>
        /// <remarks>If validation already enabled this method do nothing and just return <see langword="false"/>.</remarks>
        /// <returns>Returns <see langword="true"/> if there are errors in a validation.</returns>
        public bool FirstValidationHasErrors()
        {
            if (_validationToken != null) {
                _validationToken.Dispose();
                _validationToken = null;

                return HasErrors;
            }

            return false;
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

        protected override void OnPropertyChanged(AdvancedPropertyChangedEventArgs e)
        {
            if (IsSaving || IsCanceling || IsClosing || IsClosed || string.IsNullOrEmpty(e.PropertyName)) {
                return;
            }

            // Remove the Api errors when the associated and assumed property has changed.
            // Also we use check e.OldValue != e.NewValue && e.IsOldValueMeaningful,
            // so that we can see that the value has really changed by user.
            if (AutomaticallyHideApiErrorsOnPropertyChanged
                && e.OldValue != e.NewValue
                && e.IsOldValueMeaningful) {

                if (ApiErrors.ContainsKey(e.PropertyName)) {
                    ApiErrors.Remove(e.PropertyName);
                } else {
                    ApiErrors.Remove(PropertyToApiFieldName(e.PropertyName));
                }
            }

            base.OnPropertyChanged(e);
        }

        protected override void OnValidatingFields(IValidationContext validationContext)
        {
            base.OnValidatingFields(validationContext);

            if (ApiErrors.Count != 0) {
                var list = new List<IFieldValidationResult>();
                DisplayApiErrors(list);

                foreach (var item in list) {
                    validationContext.Add(item);
                }
            }
        }

        /// <summary>
        /// Display the Api errors.
        /// Override this method to implement custom logic for the Api errors.
        /// </summary>
        /// <param name="validationResults">The Api errors, add additional errors to this list.</param>
        /// <remarks>
        /// You can use <c>base.DisplayApiErrors(validationResults)</c> from overrided method to preserve the base logic.
        /// </remarks>
        protected virtual void DisplayApiErrors(List<IFieldValidationResult> validationResults)
        {
            var errors = new Dictionary<string, string>(ApiErrors);
            if (errors.Count == 0) {
                return;
            }

            foreach (var error in errors) {
                if (PropertyExists(error.Key)) {
                    validationResults.Add(FieldValidationResult.CreateError(error.Key, error.Value));
                } else if (AutomaticallyMapApiFields) {
                    // Automatically map api fields with assumed properties of VM.
                    string assumedPropertyName = ApiFieldToPropertyName(error.Key);

                    if (PropertyExists(assumedPropertyName)) {
                        validationResults.Add(FieldValidationResult.CreateError(assumedPropertyName, error.Value));
                    }
                }
            }
        }

        /// <summary>
        /// Invoke the specified <paramref name="action"/> in Design mode.
        /// </summary>
        /// <param name="action">Callback.</param>
        /// <param name="throwsExceptionNotDesignMode">Throws exception if application not in Design mode.</param>
        /// <returns>Returns <see langword="true"/> if application in Design mode.</returns>
        /// 
        /// <exception cref="InvalidOperationException">If <paramref name="throwsExceptionNotDesignMode"/> is <see langword="true"/> and application not in Design mode.</exception>
        /// 
        /// <example>
        /// This method convenient to use for generating fake data during the Design mode.
        /// <code>
        /// class SomeViewModel : ViewModel {
        ///     // If this constructor used not in Design mode, this means that the applicationfailed to resolve 
        ///     // ISomeService from ServiceLocator, so thats why this method throws the exception.
        ///     public SomeViewModel() {
        ///         HandleDesignMode(() => {
        ///             Name = "Fake data";
        ///         });
        ///     }
        ///     
        ///     public SomeViewModel(ISomeService service) {
        ///         Name = "Real data";
        ///     }
        ///     
        ///     public string Name { get; set; }
        /// }
        /// </code>
        /// 
        /// If your VM has empty constructor and it should be empty not in Design mode, you can pass 2rd parameter as false:
        /// <code>
        /// class SomeViewModel : ViewModel {
        ///     // If this constructor used not in Design mode, this means that the applicationfailed to resolve 
        ///     // ISomeService from ServiceLocator, so thats why this method throws the exception.
        ///     public SomeViewModel() {
        ///         if (HandleDesignMode(() => {
        ///             Name = "Fake data";
        ///         }, false)) return;
        ///         
        ///         Name = "Real data";
        ///     }
        ///     
        ///     public string Name { get; set; }
        /// }
        /// </code>
        /// 
        /// If you dont need to generate any fake data, you should just set 'IsDesignTimeCreatable' to 'false' in xaml.
        /// </example>
        protected bool HandleDesignMode(Action action = null, bool throwsExceptionNotDesignMode = true)
        {
            if (CatelEnvironment.IsInDesignMode) {
                action?.Invoke();
            } else if (throwsExceptionNotDesignMode) {
                throw new InvalidOperationException("This method is available only in Design Mode. " +
                    "Most likely, the VM was created using the wrong constructor. " +
                    "Perhaps this is due to the inability to resolve some dependencies.");
            }

            return CatelEnvironment.IsInDesignMode;
        }

        /// <summary>
        /// Handle Api errors and call <c>DisplayApiErrors()</c>.
        /// </summary>
        /// <param name="errors">The Api errors.</param>
        /// <param name="aliases">The list of aliases of errors. First key is the name of api field, second is alias.</param>
        /// <remarks>
        /// If <paramref name="aliases"/> specified, errors will be added to <see cref="ApiErrors"/> with alias.
        /// Use <see cref="DisplayApiErrors(List{IFieldValidationResult})"/> to display the Api errors.
        /// </remarks>
        protected void HandleApiErrors(Dictionary<string, string[]> errors, Dictionary<string, string> aliases = null)
        {
            ApiErrors.Clear();

            foreach (var error in errors) {
                string key = error.Key;

                if (aliases?.ContainsKey(key) ?? false) {
                    key = aliases[error.Key];
                }

                ApiErrors.Add(key, error.Value[0]);
            }

            Validate(true);
        }

        /// <summary>
        /// Check if property exists in <c>ViewModel</c>.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <returns>Returns true if property exists.</returns>
        protected bool PropertyExists(string propertyName) => CatelProperties.ContainsKey(propertyName);

        /// <summary>
        /// Converts Api field to assumed property name.
        /// </summary>
        /// <param name="apiFieldKey">The Api field key.</param>
        /// <returns>The assumed property name for Api field.</returns>
        private protected string ApiFieldToPropertyName(string apiFieldKey)
        {
            Argument.IsNotNullOrEmpty(nameof(apiFieldKey), apiFieldKey);

            var result = new StringBuilder();
            string[] peaces = apiFieldKey.Split("_");

            foreach (string peace in peaces) {
                result.Append(string.Concat(peace[0].ToString().ToUpper(), peace.AsSpan(1)));
            }

            return result.ToString();
        }

        /// <summary>
        /// Converts property name to assumed Api field name.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <returns>The assumed Api field name for property.</returns>
        private protected string PropertyToApiFieldName(string propertyName)
        {
            Argument.IsNotNullOrEmpty(nameof(propertyName), propertyName);

            var result = new StringBuilder();
            result.Append(char.ToLowerInvariant(propertyName[0]));

            for (int i = 1; i < propertyName.Length; ++i) {
                char c = propertyName[i];

                if (char.IsUpper(c)) {
                    result.Append('_');
                    result.Append(char.ToLowerInvariant(c));
                } else {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Exclude all properties non-decorated with <see cref="ValidatableAttribute"/> from validation.
        /// </summary>
        private void ExcludeNonDecoratedPropertiesFromValidation()
        {
            var type = GetType();

            foreach (var property in CatelProperties) {
                if (!property.Value.GetPropertyInfo(type).IsDecoratedWithAttribute(typeof(ValidatableAttribute))) {
                    PropertiesNotCausingValidation[type].Add(property.Key);
                }
            }
        }
    }
}
