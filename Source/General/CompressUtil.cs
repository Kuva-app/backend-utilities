#nullable enable
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Utilities.General
{
    /// <summary>
    /// CompressUtil
    /// </summary>
    public static class CompressUtil
    {
        /// <summary>
        /// Compactar String em Base64
        /// </summary>
        /// <param name="text">Conteudo a ser comprimido</param>
        /// <returns>
        /// String em Base64
        /// </returns>
        public static string CompressString(string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            var ms = new MemoryStream();
            using (var zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                zip.Write(buffer, 0, buffer.Length);
            }
            ms.Position = 0;
            var compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);
            byte[] gzBuffer = new byte[compressed.Length + 4];
            Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            return Convert.ToBase64String(gzBuffer);
        }

        /// <summary>
        /// Descompactar String Base64
        /// </summary>
        /// <param name="compressedText">String compactada em Base64</param>
        /// <returns>
        /// String descomprimida
        /// </returns>
        public static string DecompressString(string compressedText)
        {
            var gzBuffer = Convert.FromBase64String(compressedText);
            using var ms = new MemoryStream();
            var msgLength = BitConverter.ToInt32(gzBuffer, 0);
            ms.Write(gzBuffer, 4, gzBuffer.Length - 4);
            var buffer = new byte[msgLength];
            ms.Position = 0;
            using var zip = new GZipStream(ms, CompressionMode.Decompress);
            zip.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }

        /// <summary>
        /// Zips the specified zip file name.
        /// </summary>
        /// <param name="zipFileName">Name of the zip file.</param>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="fileFilter">The file filter.</param>
        public static void Zip(string zipFileName, string sourceDirectory, string? fileFilter = null)
        {
            Zip(zipFileName, sourceDirectory, true, fileFilter);
        }

        /// <summary>
        /// Zips the specified zip file name.
        /// </summary>
        /// <param name="zipFileName">Name of the zip file.</param>
        /// <param name="sourceDirectory">The source directory.</param>
        /// <param name="recurse">if set to <c>true</c> [recurse].</param>
        /// <param name="fileFilter">The file filter.</param>
        public static void Zip(string zipFileName, string sourceDirectory, bool recurse, string? fileFilter = null)
        {
            var fZip = new FastZip();
            fZip.CreateZip(zipFileName, sourceDirectory, recurse, fileFilter);
        }

        /// <summary>
        /// Compacta um arquivo a partir de seu binario (byte[])
        /// </summary>
        /// <param name="streamBufferToZip">byte[] a ser compactado</param>
        /// <param name="zipEntryName">Nome do arquivo compactado dentro do zip</param>
        /// <returns>
        /// byte[] arquivo zipado
        /// </returns>
        public static byte[] Zip(byte[] streamBufferToZip, string zipEntryName)
        {
            var buffer = new byte[4096];
            using var fileStreamOut = new MemoryStream();
            using (var zipOutStream = new ZipOutputStream(fileStreamOut))
            {
                using (var fileToZip = new MemoryStream(streamBufferToZip))
                {
                    var newEntry = new ZipEntry(zipEntryName) { DateTime = DateTime.Now };
                    //zipOutStream.UseZip64 = UseZip64.Off; //Compatibilidade se necessario
                    zipOutStream.SetLevel(9);
                    zipOutStream.PutNextEntry(newEntry);
                    StreamUtils.Copy(fileToZip, zipOutStream, buffer);
                    zipOutStream.CloseEntry();
                }
            }
            return fileStreamOut.ToArray();
        }

        /// <summary>
        /// Unzips the specified s file.
        /// </summary>
        /// <param name="sFile">The s file.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <param name="fileFilter">The filter.</param>
        public static void Unzip(string sFile, string targetDirectory, string? fileFilter = null)
        {
            var fZip = new FastZip();
            fZip.ExtractZip(sFile, targetDirectory, fileFilter);
        }

        /// <summary>
        /// Retorna uma lista de byte[], aonde cada item é um arquivo descompactado
        /// </summary>
        /// <param name="streamBufferToUnZip">byte[] do arquivo a ser descompactado</param>
        /// <returns>
        /// Lista de byte[] com os arquivos descompactados
        /// </returns>
        public static List<byte[]> UnZip(byte[] streamBufferToUnZip)
        {
            var retorno = new List<byte[]>();
            var buffer = new byte[4096];
            using var inputMemStream = new MemoryStream(streamBufferToUnZip);
            using var zipInputStream = new ZipInputStream(inputMemStream);
            while (zipInputStream.GetNextEntry() != null)
            {
                using var outputMemStream = new MemoryStream();
                StreamUtils.Copy(zipInputStream, outputMemStream, buffer);
                retorno.Add(outputMemStream.ToArray());
            }
            return retorno;
        }
    }
}
