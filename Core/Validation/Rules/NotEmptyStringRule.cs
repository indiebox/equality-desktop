namespace Equality.Core.Validation
{
    public class NotEmptyStringRule : IValidatorRule<string>
    {
        public NotEmptyStringRule(bool ignoreWhiteSpaces = false)
        {
            IgnoreWhiteSpaces = ignoreWhiteSpaces;
        }

        public bool IgnoreWhiteSpaces { get; set; }

        public string Message { get; set; } = "Поле обязательно для заполнения.";

        public bool Passes(string value)
        {
            return IgnoreWhiteSpaces
                ? !string.IsNullOrEmpty(value)
                : !string.IsNullOrWhiteSpace(value);
        }
    }
}
