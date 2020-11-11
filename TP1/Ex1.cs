using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SMTP1
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            FileInfo fileName = ReadFile();
            Dictionary<int, int> symbols = ReadSymbols(fileName);
            
            //1. a)
            double entropy = CalculateEntropy(symbols);
            PrintEntropy(entropy);
            
            //1. b)
            List<KeyValuePair<int, int>> symbolsSorted = SortSymbolsByCount(symbols);
            PrintTopFive(symbolsSorted);
            
            //1. c)
            List<KeyValuePair<int, int>> topHalfGroup = GetTopHalfGroup(symbolsSorted);
            PrintTopHalfGroup(topHalfGroup);
        }

        private static FileInfo ReadFile()
        {
            string fileName = "";
            if (String.IsNullOrEmpty(fileName))
            {
                Console.WriteLine("File to read:");
                fileName = Console.ReadLine();
            }
            FileInfo file = new FileInfo(fileName);
            return file;
        }

        private static Dictionary<int, int> ReadSymbols(FileInfo fileName)
        {
            Dictionary<int, int> symbols = new Dictionary<int, int>();
            using (FileStream fileStream = new FileStream(fileName.FullName, FileMode.Open, FileAccess.Read))
            {
                for (int i = 0; i < fileStream.Length; i++)
                {
                    int symbol = fileStream.ReadByte();
                    if(symbol > 255)
                        throw new InvalidDataException($"Invalid symbol {symbol}");
                    
                    if (symbols.TryGetValue(symbol, out int currentCount))
                        symbols[symbol] = currentCount + 1;
                    else
                        symbols.Add(symbol, 1);
                }
            }
            Console.WriteLine("----------");
            Console.WriteLine($"Total Different Symbols: {symbols.Count}");
            Console.WriteLine($"Total Symbol Count: {symbols.Values.Sum()}");
            return symbols;
        }

        private static double CalculateEntropy(Dictionary<int,int> symbols)
        {
            double entropy = 0;
            int nOfSymbols = symbols.Values.Sum();
            foreach (KeyValuePair<int,int> keyValuePair in symbols)
            {
                double probability = ((double) keyValuePair.Value / nOfSymbols);
                entropy += probability * Math.Log2(1 / probability);
            }
            return entropy;
        }

        private static void PrintEntropy(in double entropy)
        {
            Console.WriteLine("----------");
            Console.WriteLine($"The Entropy is {entropy}");
        }

        private static List<KeyValuePair<int, int>> SortSymbolsByCount(Dictionary<int,int> symbols)
        {
            List<KeyValuePair<int, int>> list = symbols.ToList();
            list.Sort((p1, p2) => p2.Value.CompareTo(p1.Value));
            return list;
        }

        private static void PrintTopFive(List<KeyValuePair<int,int>> symbolsSorted)
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

        private static List<KeyValuePair<int, int>> GetTopHalfGroup(List<KeyValuePair<int,int>> symbolsSorted)
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

        private static void PrintTopHalfGroup(List<KeyValuePair<int,int>> topHalfGroup)
        {
            Console.WriteLine("----------");
            Console.WriteLine($"Top Half Group:");
            foreach (var t in topHalfGroup)
                Console.WriteLine($"'{Convert.ToChar(t.Key)}' (Count: {t.Value})");
            
            Console.WriteLine($"(Total Count: {topHalfGroup.Select(x => x.Value).Sum()})");
        }
    }
}