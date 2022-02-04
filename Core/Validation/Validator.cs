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

        public void ValidateField<T>(string propertyName, T value, List<IValidatorRule<T>> rules)
            where T : class
        {
            foreach (var rule in rules) {
                if (!rule.Passes(value)) {
                    FieldValidationResults.Add(FieldValidationResult.CreateError(propertyName, rule.Message));

                    break;
                }
            }
        }
    }
}
