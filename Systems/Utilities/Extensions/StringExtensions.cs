using System;
using System.Linq;

namespace Administrator.Utilities.Extensions
{
    /// <summary> Helpful methods for working with strings. </summary>
    public static class StringExtensions
    {
        /// <summary> Convert the given file into an absolute filepath. </summary>
        /// <param name="absoluteDirectoryPath"> The absolute path of the current directory. </param>
        /// <param name="filename"> The filename. A relative file will be appended to the directory path. An absolute will be used instead. </param>
        /// <returns> The absolute filepath. </returns>
        public static String BuildAbsoluteFilepath(String absoluteDirectoryPath, String filename)
        {
            return filename[0] == '/' ? filename : absoluteDirectoryPath + '/' + filename;
        }


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
