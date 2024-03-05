#nullable enable
using System;
using System.IO;
using Xunit;
using Utilities.General;

namespace UtilitiesTest
{
    public class CompressTest
    {
        [Fact]
        public void CompressStringTest()
        {
            var compressed = CompressUtil.CompressString(VOs.General.CertThumbPrint);
            var decompressed = CompressUtil.DecompressString(compressed);
            Assert.NotNull(compressed);
            Assert.Equal(VOs.General.CertThumbPrintCompress, compressed);
            Assert.Equal(VOs.General.CertThumbPrint, decompressed);
        }

        [Fact]
        public void ZipTest()
        {
            if (string.IsNullOrEmpty(VOs.General.AppPath) || string.IsNullOrWhiteSpace(VOs.General.AppPath)) return;
            var file = Path.Combine(VOs.General.AppPath, "Mock", VOs.General.FileNameZip);
            var sourceDirectory = Path.Combine(VOs.General.AppPath, "Mock");
            var destinationDirectory = VOs.General.AppPath;
            var fileExpected = Path.Combine(VOs.General.AppPath, VOs.General.FileNameLeandroUserMockJson);
            const string? fileFilter = @"\.json$";
            CompressUtil.Zip(file, sourceDirectory, fileFilter);
            System.Threading.Thread.Sleep(600);
            CompressUtil.Unzip(file, destinationDirectory, fileFilter);
            var fileZipExists = File.Exists(file);
            var fileUnZipExists = File.Exists(fileExpected);
            try
            {
                File.Delete(file);
                File.Delete(fileExpected);
            }
            catch (Exception)
            {
                // ignored
            }
            Assert.True(fileZipExists);
            Assert.True(fileUnZipExists);
        }
    }
}