using System.Globalization;
using System.Windows.Controls;

namespace Equality.Validation
{
    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "Это поле обязательно для заполнения.")
                : ValidationResult.ValidResult;
        }
    }
}
