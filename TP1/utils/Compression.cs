using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using K4os.Compression.LZ4;

namespace SMTP1
{
    public static class Compression
    {
        internal static long GetZLibCompressionLength(List<byte> source, out long lengthUncompressed)
        {
            using MemoryStream unZippedChunk = new MemoryStream(source.ToArray());
            lengthUncompressed = unZippedChunk.Length;

            using MemoryStream zippedChunk = new MemoryStream();
            using ZipOutputStream zipOutputStream = new ZipOutputStream(zippedChunk);
            zipOutputStream.SetLevel(9);

            ZipEntry entry = new ZipEntry("name");
            zipOutputStream.PutNextEntry(entry);

            unZippedChunk.CopyTo(zipOutputStream);
            return zipOutputStream.Length;
        }

        public static long GetLz4CompressionLength(List<byte> sourceList, out long lengthUncompressed)
        {
            byte[] source = sourceList.ToArray();
            byte[] target = new byte[LZ4Codec.MaximumOutputSize(source.Length)];
            lengthUncompressed = source.Length;
            
            return LZ4Codec.Encode(source, 0, source.Length, target, 0, target.Length, LZ4Level.L12_MAX);
        }
    }
}