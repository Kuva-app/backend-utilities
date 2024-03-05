using System.ComponentModel;

namespace Utilities.Security
{
    /// <summary>
    /// Password complexity error type enum
    /// </summary>
    public enum PasswordComplexityErrorType
    {
        /// <summary>
        /// The nothing
        /// </summary>
        Nothing = 0,
        /// <summary>
        /// The password empty
        /// </summary>
        [Description("Password should not be empty.")]
        PasswordEmpty = 1,
        /// <summary>
        /// The password minimum chars request
        /// </summary>
        [Description("The password must not be less than 8 characters.")]
        PasswordMinimumCharsRequest = 2,
        /// <summary>
        /// The password lower character request
        /// </summary>
        [Description("The password must contain at least one lowercase letter.")]
        PasswordLowerCharRequest = 3,
        /// <summary>
        /// The password upper character request
        /// </summary>
        [Description("The password must contain at least one uppercase letter.")]
        PasswordUpperCharRequest = 4,
        /// <summary>
        /// The password numeric character request
        /// </summary>
        [Description("The password must contain at least one numeric character.")]
        PasswordNumericCharRequest = 5,
        /// <summary>
        /// The password special character request
        /// </summary>
        [Description("The password must contain at least one special character, for example: '!@#$%^&*()_+=[{]};:<>|./?,-'.")]
        PasswordSpecialCharRequest = 6
    }

}
