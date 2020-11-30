using System;
using System.Collections.Generic;
using K4os.Compression.LZ4;

namespace SMTP1
{
    public static class Ex7
    {
        public static void Entry(string fileName)
        {
            //7.
            List<byte> source = Common.ReadFile(fileName);
            Dictionary<char, int> symbolsCount = Common.ReadSymbolsCount(source);
            double entropy = Common.CalculateEntropy(symbolsCount);
            Print.PrintEntropy(entropy);

            long lengthCompressedZLib = Compression.GetZLibCompressionLength(source, out long lengthUncompressedZLib);
            long lengthCompressedLz4 = Compression.GetLz4CompressionLength(source, out long lengthUncompressedLz4);

            Print.PrintCompressionResults("ZLib", lengthUncompressedZLib, lengthCompressedZLib);
            Print.PrintCompressionResults("Lz4", lengthUncompressedLz4, lengthCompressedLz4);
        }
    }
}