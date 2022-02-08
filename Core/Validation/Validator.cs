using System.Collections.Generic;

using Catel.Data;

namespace Equality.Core.Validation
{
    public class Validator
    {
        public Validator(List<IFieldValidationResult> validationResults)
        {
            FieldValidationResults = validationResults;
        }

        public List<IFieldValidationResult> FieldValidationResults { get; set; }

        /// <summary>
        /// Validates the given value throught all specified rules.
        /// </summary>
        /// <typeparam name="T">The type of value.</typeparam>
        /// <param name="propertyName">The property name.</param>
        /// <param name="value">The value to be validated.</param>
        /// <param name="rules">The validation rules.</param>
        /// <remarks>Validation stops after first error.</remarks>
        public void ValidateField<T>(string propertyName, T value, List<IValidatorRule<T>> rules)
        {
            foreach (var rule in rules) {
                if (!rule.Passes(value)) {
                    FieldValidationResults.Add(FieldValidationResult.CreateError(propertyName, rule.Message));

                    break;
                }
            }
        }

        /// <summary>
        /// Add error for specified property in validation results.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <param name="message">The error message.</param>
        public void AddError(string propertyName, string message)
        {
            FieldValidationResults.Add(FieldValidationResult.CreateError(propertyName, message));
        }
    }
}
