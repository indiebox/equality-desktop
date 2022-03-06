namespace Equality.Validation
{
    public class ValidEmailRule : IValidatorRule<string>
    {
        public ValidEmailRule(bool lightCheck = true)
        {
            LightCheck = lightCheck;
        }

        public bool LightCheck { get; set; }

        public string Message { get; set; } = "Поле должно быть валидным Email-адресом.";

        public bool Passes(string value)
        {
            value = value.Trim();

            if (value.EndsWith(".")) {
                return false;
            }

            if (LightCheck) {
                return value.Trim('@').Contains('@');
            }

            try {
                var email = new System.Net.Mail.MailAddress(value);

                return email.Address == value;
            } catch {
                return false;
            }
        }
    }
}
