using System;
using System.Linq;

namespace Administrator.Utilities.Extensions
{
    /// <summary> Helpful methods for working with strings. </summary>
    public static class StringExtensions
    {
        /// <summary> Returns whether the input only contains digits and letters (no special characters). </summary>
        public static Boolean IsAlphaNumeric(this String input)
        {
            return input.All(Char.IsLetterOrDigit);
        }

        /// <summary> Returns whether the input only contains letters. </summary>
        public static Boolean IsAlphaOnly(this String input)
        {
            return input.All(Char.IsLetter);
        }

        /// <summary> Returns whether the input only contains digits. </summary>
        public static Boolean IsNumericOnly(this String input)
        {
            return input.All(Char.IsDigit);
        }
    }
}
