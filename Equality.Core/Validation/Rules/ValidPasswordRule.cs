using System.Linq;

namespace Equality.Validation
{
    public class ValidPasswordRule : IValidatorRule<string>
    {
        public string Message { get; set; }

        public bool Passes(string value)
        {
            if (!value.Any(char.IsNumber)) {
                Message = "Пароль должен содержать хотя бы одну цифру.";

                return false;
            }

            if (!value.Any(char.IsLower) || !value.Any(char.IsUpper)) {
                Message = "Пароль должен содержать хотя бы одну большую и маленькую буквы.";

                return false;
            }

            return true;
        }
    }
}
