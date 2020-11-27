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
                    $"{i + 1}º: '{Convert.ToChar(symbolsSorted[i].Key)}' (Count: {symbolsSorted[i].Value})");
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

        public static void PrintSourceAndSequenceEntropy(int fileSymbolsCountCount, int fileSum,
            int sequenceSymbolsCountCount, int sequenceSum, in double entropy, in double entropyMfo,
            in double entropySeq,
            in double entropyMfoSeq)
        {
            Console.WriteLine("-----File-----");
            PrintSymbolCount(fileSymbolsCountCount, fileSum);
            PrintEntropy(entropy);
            PrintEntropyMarkovFirst(entropyMfo);
            Console.WriteLine("-----New Sequence-----");
            PrintSymbolCount(sequenceSymbolsCountCount, sequenceSum);
            PrintEntropy(entropySeq);
            PrintEntropyMarkovFirst(entropyMfoSeq);
        }

        public static void PrintCompressionLengths(in long sourceLengthUncompressed, in long sourceLengthCompressed,
            in long sequenceLengthUncompressed, in long sequenceLengthCompressed)
        {
            Console.WriteLine($"Source Length Uncompressed: '{sourceLengthUncompressed}'");
            Console.WriteLine($"Source Length Compressed: '{sourceLengthCompressed}'");
            Console.WriteLine($"Sequence Length Uncompressed: '{sequenceLengthUncompressed}'");
            Console.WriteLine($"Sequence Length Compressed: '{sequenceLengthCompressed}'");
        }

        public static void PrintSymbolCount(in int symbolsCountCount, int sum)
        {
            Console.WriteLine($"Total Different Symbols: {symbolsCountCount}");
            Console.WriteLine($"Total Symbol Count: {sum}");
        }
    }
}