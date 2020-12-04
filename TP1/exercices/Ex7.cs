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

            int divisions = 10;
            for (float i = 1; i <= divisions; i++)
            {
                int range = (int) Math.Truncate((source.Count) * (i / divisions));
                List<byte> subSource = source.GetRange(0, range);
                long lengthCompressedZLib = Compression.GetZLibCompressionLength(subSource, out long lengthUncompressedZLib);
                Print.PrintCompressionResults($"ZLib {i*divisions}%", lengthUncompressedZLib, lengthCompressedZLib);

                if ((int)i == divisions)
                {
                    Dictionary<char, int> symbolsCountCompressed = Common.ReadSymbolsCount(subSource);
                    double entropyCompressed = Common.CalculateEntropy(symbolsCountCompressed);
                    Print.PrintEntropy(entropyCompressed);
                }
            }
            
            for (float i = 1; i <= divisions; i++)
            {
                int range = (int) Math.Truncate((source.Count) * (i / divisions));
                List<byte> subSource = source.GetRange(0, range);
                long lengthCompressedLz4 = Compression.GetLz4CompressionLength(subSource, out long lengthUncompressedLz4);
                Print.PrintCompressionResults("Lz4", lengthUncompressedLz4, lengthCompressedLz4);

                if ((int)i == divisions)
                {
                    Dictionary<char, int> symbolsCountCompressed = Common.ReadSymbolsCount(subSource);
                    double entropyCompressed = Common.CalculateEntropy(symbolsCountCompressed);
                    Print.PrintEntropy(entropyCompressed);
                }
            }
        }
    }
}