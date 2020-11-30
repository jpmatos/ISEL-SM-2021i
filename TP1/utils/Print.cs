using System;
using System.Collections.Generic;
using System.Linq;

namespace SMTP1
{
    internal static class Print
    {
        internal static void PrintEntropy(in double entropy)
        {
            Console.WriteLine($"The Entropy is {entropy}");
            Console.WriteLine("----------");
        }

        internal static void PrintEntropyMarkovFirst(in double entropy)
        {
            Console.WriteLine($"The Markov First Order Entropy is {entropy}");
            Console.WriteLine("----------");
        }

        internal static void PrintTopFive(List<KeyValuePair<char, int>> symbolsSorted)
        {
            Console.WriteLine("Top Five:");
            int totalCount = 0;
            for (int i = 0; i < (symbolsSorted.Count > 5 ? 5 : symbolsSorted.Count); i++)
            {
                Console.WriteLine(
                    $"{i + 1}º: '{symbolsSorted[i].Key}' ({Convert.ToByte(symbolsSorted[i].Key)}) (Count: {symbolsSorted[i].Value})");
                totalCount += symbolsSorted[i].Value;
            }

            Console.WriteLine($"(Total Count: {totalCount})");
            Console.WriteLine("----------");
        }

        internal static void PrintTopHalfGroup(List<KeyValuePair<char, int>> topHalfGroup)
        {
            Console.WriteLine($"Top Half Group:");
            foreach (var t in topHalfGroup)
                Console.WriteLine($"'{Convert.ToChar(t.Key)}' (Count: {t.Value})");

            Console.WriteLine($"(Total Count: {topHalfGroup.Select(x => x.Value).Sum()})");
            Console.WriteLine("----------");
        }

        public static void PrintCompressionLengths(in long sourceLengthUncompressed, in long sourceLengthCompressed,
            in long sequenceFoLengthUncompressed, in long sequenceFoLengthCompressed,
            in long sequenceSoLengthUncompressed, in long sequenceSoLengthCompressed)
        {
            Console.WriteLine($"Source Length Uncompressed: '{sourceLengthUncompressed}'");
            Console.WriteLine($"Source Length Compressed: '{sourceLengthCompressed}'");
            Console.WriteLine($"Sequence First Order Length Uncompressed: '{sequenceFoLengthUncompressed}'");
            Console.WriteLine($"Sequence First Order Length Compressed: '{sequenceFoLengthCompressed}'");
            Console.WriteLine($"Sequence Second Order Length Uncompressed: '{sequenceSoLengthUncompressed}'");
            Console.WriteLine($"Sequence Second Order Length Compressed: '{sequenceSoLengthCompressed}'");
        }

        public static void PrintSymbolCount(in int symbolsCountCount, int sum)
        {
            Console.WriteLine($"Total Different Symbols: {symbolsCountCount}");
            Console.WriteLine($"Total Symbol Count: {sum}");
        }

        public static void PrintSourceAndSequenceEntropy(
            in int symbolsCount, int symbolsCountSum, in double entropySource, in double entropyMfoSource, 
            in int symbolsCountSequenceFoCount, int symbolsCountSequenceFoCountSum, in double entropySequenceFo, in double entropyMfoSequenceFo, 
            in int symbolsCountSequenceSoCount, int symbolsCountSequenceSoCountSum, in double entropySequenceSo, in double entropyMfoSequenceSo)
        {
            Console.WriteLine("-----File-----");
            PrintSymbolCount(symbolsCount, symbolsCountSum);
            PrintEntropy(entropySource);
            PrintEntropyMarkovFirst(entropyMfoSource);
            Console.WriteLine("-----New First Order Sequence-----");
            PrintSymbolCount(symbolsCountSequenceFoCount, symbolsCountSequenceFoCountSum);
            PrintEntropy(entropySequenceFo);
            PrintEntropyMarkovFirst(entropyMfoSequenceFo);
            Console.WriteLine("-----New Second Order Sequence-----");
            PrintSymbolCount(symbolsCountSequenceSoCount, symbolsCountSequenceSoCountSum);
            PrintEntropy(entropySequenceSo);
            PrintEntropyMarkovFirst(entropyMfoSequenceSo);
        }

        public static void PrintFileName(string fileName)
        {
            Console.WriteLine($"File '{fileName}'");
        }

        public static void PrintCompressionResults(string method, in long lengthUncompressed, in long lengthCompressed)
        {
            Console.WriteLine($"{method} - Uncompressed: '{lengthUncompressed}'. Compressed: '{lengthCompressed}'. " +
                              $"Ratio: '{(float)lengthUncompressed/lengthCompressed:N2}:1' " +
                              $"Percentage Removed: '{100f - (float)lengthCompressed/lengthUncompressed*100:N2}%'");
        }

        public static void PrintPResult(in double p, in double entropy, in long lengthUncompressed, in long lengthCompressed)
        {
            Console.WriteLine(
                $"p='{p:N1}' MFO Entropy: '{entropy}'. Uncompressed: '{lengthUncompressed}'. Compressed: '{lengthCompressed}'");
        }
    }
}