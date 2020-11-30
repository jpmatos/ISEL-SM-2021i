using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SMTP1
{
    internal static class Ex2
    {
        internal static void Entry(string fileName)
        {
            //2.
            List<byte> source = Common.ReadFile(fileName);
            Dictionary<char, int> symbolsCount = Common.ReadSymbolsCount(source);
            Dictionary<char, Dictionary<char, int>> symbolsCountMfo = Common.ReadMarkovFirstOrderCount(source);
            double entropy = Common.CalculateMarkovFirstOrderEntropy(symbolsCount, symbolsCountMfo);
            Print.PrintSymbolCount(symbolsCount.Count, symbolsCount.Values.Sum());
            Print.PrintEntropyMarkovFirst(entropy);
        }
    }
}