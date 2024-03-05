using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace Utilities.General.Extensions
{
    /// <summary>
    /// String Extension
    /// </summary>
    public static class StringExtension
    {

        /// <summary>
        /// Converts the json object.
        /// </summary>
        /// <param name="jsonResponse">The json response.</param>
        /// <returns></returns>
        [return: MaybeNull]
        public static T ConvertJsonObject<T>(this string jsonResponse)
        {
            if (string.IsNullOrEmpty(jsonResponse))
                return default;
            T value = default;
            try
            {
                value = JsonSerializer.Deserialize<T>(jsonResponse);
            }
            catch
            {
                // ignored
            }
            return value;
        }

        /// <summary>
        /// Get Unicode string from text
        /// </summary>
        /// <param name="text">The text</param>
        /// <returns>
        /// return string without non unicode characters
        /// </returns>
        public static string GetUnicodeString(this string text)
        {
            var newText = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            var count = arrayText.Length;
            for (var i = 0; i < count; i++)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(arrayText[i]) != System.Globalization.UnicodeCategory.NonSpacingMark)
                    newText.Append(arrayText[i]);
            }
            return newText.ToString();
        }

        /// <summary>
        /// Lefts the specified size.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static string Left(this string value, int size)
        {
            return value.Length > size ? value.Substring(0, size) : value;
        }

        /// <summary>
        /// Rights the specified size.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        public static string Right(this string value, int size)
        {
            return value.Length > size ? value.Substring(value.Length - size, size) : value;
        }

        /// <summary>
        /// Determines whether [has lower character].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if [has lower character] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasLowerChar(this string value)
        {
            return new Regex(@"[a-z]+").IsMatch(value);
        }

        /// <summary>
        /// Determines whether [has upper character].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if [has upper character] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasUpperChar(this string value)
        {
            return new Regex(@"[A-Z]+").IsMatch(value);
        }

        /// <summary>
        /// Determines whether this instance has number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the specified value has number; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasNumber(this string value)
        {
            return new Regex(@"[0-9]+").IsMatch(value);
        }

        /// <summary>
        /// Determines whether this instance has symbols.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the specified value has symbols; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasSymbols(this string value)
        {
            return new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]").IsMatch(value);
        }
    }
}
