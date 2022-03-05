namespace Equality.Validation
{
    public interface IValidatorRule<T>
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Check value passes validation rule.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>Returns <see langword="true"/> if there are no any errors, <see langword="false"/> otherwise.</returns>
        public bool Passes(T value);
    }
}
