namespace Equality.Validation
{
    public class MinStringLengthRule : IValidatorRule<string>
    {
        public MinStringLengthRule(int min)
        {
            Min = min;

            Message = $"Длина строки должна быть больше {Min} символов.";
        }

        public int Min { get; set; }

        public string Message { get; set; }

        public bool Passes(string value)
        {
            return value.Length >= Min;
        }
    }
}
