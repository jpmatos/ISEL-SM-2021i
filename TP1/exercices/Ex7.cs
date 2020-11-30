using System;
using System.Collections.Generic;
using K4os.Compression.LZ4;

namespace SMTP1
{
    public static class Ex7
    {
        public static void Entry(string fileName)
        {
            List<byte> source = Common.ReadFile(fileName);
            Dictionary<char, int> symbolsCount = Common.ReadSymbolsCount(source);
            double entropy = Common.CalculateEntropy(symbolsCount);
            Print.PrintEntropy(entropy);

            long lengthCompressedZLib = Compression.GetZLibCompressionLength(source, out long lengthUncompressedZLib);
            long lengthCompressedLz4 = Compression.GetLz4CompressionLength(source, out long lengthUncompressedLz4);
            
            Console.WriteLine($"ZLib - Uncompressed: '{lengthUncompressedZLib}'. Compressed: '{lengthCompressedZLib}'. " +
                              $"Ratio: '{(float)lengthUncompressedZLib/lengthCompressedZLib:N2}:1' " +
                              $"Percentage Removed: '{100f - (float)lengthCompressedZLib/lengthUncompressedZLib*100:N2}%'");
            Console.WriteLine($"Lz4 - Uncompressed: '{lengthUncompressedLz4}'. Compressed: '{lengthCompressedLz4}'. " +
                              $"Ratio: '{((float)lengthUncompressedLz4/lengthCompressedLz4):N2}:1' " +
                              $"Percentage Removed: '{100 - (float)lengthCompressedLz4/lengthUncompressedLz4*100:N2}%'");
        }
    }
}