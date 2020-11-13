using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SMTP1
{
    internal static class Ex1
    {
        internal static void Entry(string fileName)
        {
            FileInfo file = ReadFile(fileName);
            Dictionary<char, int> symbolsCount = Common.ReadSymbolsCount(file);
            
            //1. a)
            double entropy = CalculateEntropy(symbolsCount);
            Common.PrintEntropy(entropy);
            
            //1. b)
            List<KeyValuePair<char, int>> symbolsSorted = SortSymbolsByCount(symbolsCount);
            PrintTopFive(symbolsSorted);
            
            //1. c)
            List<KeyValuePair<char, int>> topHalfGroup = GetTopHalfGroup(symbolsSorted);
            PrintTopHalfGroup(topHalfGroup);
        }

        private static FileInfo ReadFile(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                Console.WriteLine("File to read:");
                fileName = Console.ReadLine();
            }
            FileInfo file = new FileInfo(fileName);
            return file;
        }

        private static double CalculateEntropy(Dictionary<char,int> symbols)
        {
            double entropy = 0;
            int nOfSymbols = symbols.Values.Sum();
            foreach (var keyValuePair in symbols)
            {
                double probability = (double) keyValuePair.Value / nOfSymbols;
                entropy += probability * Math.Log2(1 / probability);
            }
            return entropy;
        }

        private static List<KeyValuePair<char, int>> SortSymbolsByCount(Dictionary<char,int> symbols)
        {
            List<KeyValuePair<char, int>> list = symbols.ToList();
            list.Sort((p1, p2) => p2.Value.CompareTo(p1.Value));
            return list;
        }

        private static void PrintTopFive(List<KeyValuePair<char,int>> symbolsSorted)
        {
            Console.WriteLine("----------");
            Console.WriteLine("Top Five:");
            int totalCount = 0;
            for (int i = 0; i < (symbolsSorted.Count > 5 ? 5 : symbolsSorted.Count); i++)
            {
                Console.WriteLine(
                    $"{i + 1}º: '{Convert.ToChar(symbolsSorted[i].Key)}' (Count: {symbolsSorted[i].Value})");
                totalCount += symbolsSorted[i].Value;
            }

            Console.WriteLine($"(Total Count: {totalCount})");
        }

        private static List<KeyValuePair<char, int>> GetTopHalfGroup(List<KeyValuePair<char,int>> symbolsSorted)
        {
            int totalCount = symbolsSorted.Select(x => x.Value).Sum();
            int currentCount = 0;
            int index = 0;
            foreach (var pair in symbolsSorted)
            {
                if (currentCount < Math.Ceiling((float)totalCount/2))
                {
                    currentCount += pair.Value;
                    index++;
                }
                else
                    break;
            }

            return symbolsSorted.GetRange(0, index);
        }

        private static void PrintTopHalfGroup(List<KeyValuePair<char,int>> topHalfGroup)
        {
            Console.WriteLine("----------");
            Console.WriteLine($"Top Half Group:");
            foreach (var t in topHalfGroup)
                Console.WriteLine($"'{Convert.ToChar(t.Key)}' (Count: {t.Value})");
            
            Console.WriteLine($"(Total Count: {topHalfGroup.Select(x => x.Value).Sum()})");
        }
    }
}