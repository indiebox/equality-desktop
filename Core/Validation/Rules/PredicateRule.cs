using System;

namespace Equality.Core.Validation
{
    public class PredicateRule<T> : IValidatorRule<T>
    {
        protected Predicate<T> Predicate;

        public PredicateRule(Predicate<T> predicate, string message)
        {
            Predicate = predicate;
            Message = message;
        }

        public string Message { get; set; }

        public bool Passes(T value) => Predicate.Invoke(value);
    }
}
