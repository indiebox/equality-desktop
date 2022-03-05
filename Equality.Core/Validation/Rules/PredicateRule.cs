using System;

namespace Equality.Validation
{
    public class PredicateRule<T> : IValidatorRule<T>
    {
        protected Predicate<T> Predicate;

        public PredicateRule(Predicate<T> passesPredicate, string errorMessage)
        {
            Predicate = passesPredicate;
            Message = errorMessage;
        }

        public string Message { get; set; }

        public bool Passes(T value) => Predicate.Invoke(value);
    }
}
