using System;

namespace Equality.Validation
{
    /// <summary>
    /// Attribute that can be used to include properties to trigger 
    /// <c>ValidateFields</c> method in <c>ViewModel</c> on property changed.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ValidatableAttribute : Attribute
    {
    }
}
