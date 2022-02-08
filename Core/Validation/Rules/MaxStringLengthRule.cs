namespace Equality.Core.Validation
{
    public class MaxStringLengthRule : IValidatorRule<string>
    {
        public MaxStringLengthRule(int max)
        {
            Max = max;

            Message = $"Длина строки должна быть меньше {Max + 1} символов.";
        }

        public int Max { get; set; }

        public string Message { get; set; }

        public bool Passes(string value)
        {
            return value.Length <= Max;
        }
    }
}
