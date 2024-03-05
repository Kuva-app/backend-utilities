using System;
using System.IO;

#nullable enable
namespace UtilitiesTest.VOs
{
    public class General
    {
        public const string CertThumbPrint = "af298732b679d6be2c2bb0144facf05b06aebe04";
        public const string CertThumbPrintMd5 = "ce5c472584689f184822f6855f360b64";
        public const string CertThumbPrintSha256 = "ed0f4157548604e81a672f06e878da8e5ca186b3c18adfbabf671f3ddb8f8797";
        public const string CertThumbPrintB64 = "YWYyOTg3MzJiNjc5ZDZiZTJjMmJiMDE0NGZhY2YwNWIwNmFlYmUwNA==";
        public const string SecurityKey = "4nx2iOq";
        public static readonly Guid ApiKey = new Guid("b8c3fa9e-d066-41dc-8230-f06918e4f7bd");
        public const string CertThumbPrintCompress = "KAAAAB+LCAAAAAAAAAoFwYEBACAEBMCVPomM48X+I3SXI3F9C83jGVtKSCzVyRocwrLZ0A/AfSrYKAAAAA==";
        public static readonly string? AppPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public const string FileNameZip = "ZipTest.zip";
        public const string FileNameLeandroUserMockJson = "LeandroUser.json";

        /// <summary>
        /// Load file content from file path
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>Return file content</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="OutOfMemoryException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        public static string? LoadFileContentFrom(string filePath, bool isMinify = true)
        {
            if (AppPath == null) return null;
            var path = Path.Combine(AppPath, filePath);
            if (!File.Exists(path)) return null;
            using StreamReader reader = new(path);
            string json = reader.ReadToEnd();
            return isMinify ? Utilities.General.Json.Utils.Minify(json) : json;
        }
    }
}
