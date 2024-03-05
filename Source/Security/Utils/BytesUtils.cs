using System;
using System.Text;

namespace Utilities.Security.Utils
{
    public static class BytesUtils
    {
        /// <summary>
        /// Gets the bytes from.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] GetBytesFrom<T>(T value)
        {
            try
            {
                var json = General.Json.Utils.Serialize(value);
                return json != null ? Encoding.UTF8.GetBytes(json) : new byte[] { };
            }
            catch (Exception e)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(e);
#endif
                return null;
            }
        }

        /// <summary>
        /// Gets the object from bytes.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public static T GetObjectFromBytes<T>(byte[] buffer)
        {
            try
            {
                var json = Encoding.UTF8.GetString(buffer);
                return General.Json.Utils.Deserialize<T>(json);
            }
            catch (Exception e)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(e);
#endif
                return default;
            }
        }
    }
}