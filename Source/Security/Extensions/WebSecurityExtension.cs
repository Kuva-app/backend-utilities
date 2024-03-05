using System;
using Utilities.General.Extensions;

namespace Utilities.Security.Extensions
{
    /// <summary>
    /// Web Security extension
    /// </summary>
    public static class WebSecurityExtension
    {
        /// <summary>
        /// Validates the password complexity.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public static bool ValidatePasswordComplexity(this string password, out PasswordComplexityErrorType errorMessage)
        {
            errorMessage = PasswordComplexityErrorType.Nothing;
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
            {
                errorMessage = PasswordComplexityErrorType.PasswordEmpty;
                return false;
            }
            if (password.Length < 8)
            {
                errorMessage = PasswordComplexityErrorType.PasswordMinimumCharsRequest;
                return false;
            }
            if (!password.HasLowerChar())
            {
                errorMessage = PasswordComplexityErrorType.PasswordLowerCharRequest;
                return false;
            }
            if (!password.HasUpperChar())
            {
                errorMessage = PasswordComplexityErrorType.PasswordUpperCharRequest;
                return false;
            }
            if (!password.HasNumber())
            {
                errorMessage = PasswordComplexityErrorType.PasswordNumericCharRequest;
                return false;
            }
            if (!password.HasSymbols())
            {
                errorMessage = PasswordComplexityErrorType.PasswordSpecialCharRequest;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Base64s the encode.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns></returns>
        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Base64s the decode.
        /// </summary>
        /// <param name="base64EncodedData">The base64 encoded data.</param>
        /// <returns></returns>
        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Determines whether [is local URL security] [the specified URL].
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>
        ///   <c>true</c> if [is local URL security] [the specified URL]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLocalUrlSecurity(string url)
        {
            return Uri.IsWellFormedUriString(url, UriKind.Absolute) && url.IsLocalUrl();
        }

        /// <summary>
        /// Determines whether [is local URL].
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>
        ///   <c>true</c> if [is local URL] [the specified URL]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLocalUrl(this string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            return url[0] == '/' && (url.Length == 1 ||
                    url[1] != '/' && url[1] != '\\') ||
                    url.Length > 1 && url[0] == '~' && url[1] == '/';
        }
    }
}
