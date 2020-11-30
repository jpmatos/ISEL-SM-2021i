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
            
            //LZ4Codec
        }
    }
}