#nullable enable
using Utilities.General;
using System;
using System.Text.Json;

namespace Utilities.General.Json
{
    public static class Utils
    {
        /// <summary>
        /// Serialize object to json.
        /// </summary>
        /// <param name="value">The object value</param>
        /// <typeparam name="T">The type of object</typeparam>
        /// <returns>Return string of json, null if exception</returns>
        public static string? Serialize<T>(T value, bool isMinify = true)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            try
            {
                var jsonText = JsonSerializer.Serialize(value, options);
                return isMinify ? Minify(jsonText) : jsonText;
            }
            catch (NotSupportedException)
            {
                return null;
            }
        }
        /// <summary>
        /// Deserialize object to json
        /// </summary>
        /// <param name="value">The value</param>
        /// <typeparam name="T">The type</typeparam>
        /// <returns>Return T object <see cref="T"/></returns>
        public static T? Deserialize<T>(string value)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                return default;
            try
            {
                var obj = JsonSerializer.Deserialize<T>(value);
                return obj;
            }
            catch (NotSupportedException)
            {
                return default;
            }
        }

        /// <summary>
        /// Minify json string
        /// </summary>
        /// <param name="json">The json string</param>
        public static string Minify(string json)
        {
            return json.Replace("\n", "").Replace("\r", "").Replace("\t", "");
        }
    }
}
