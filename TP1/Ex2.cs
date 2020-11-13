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
            FileInfo file = Common.ReadFile(fileName);
            Dictionary<char, int> symbolsCount = Common.ReadSymbolsCount(file);
            Dictionary<char, Dictionary<char, int>> symbolsCountMfo = ReadMarkovFirstOrderCount(file);
            double entropy = CalculateMarkovFirstOrderEntropy(symbolsCount, symbolsCountMfo);
            Common.PrintEntropy(entropy);
        }

        private static double CalculateMarkovFirstOrderEntropy(Dictionary<char, int> symbolsCount,
            Dictionary<char, Dictionary<char, int>> symbolsCountMfo)
        {
            int totalSymbols = symbolsCount.Values.Sum();
            double entropy = 0;
            foreach (var currSymbolKeyValuePair in symbolsCountMfo)
            {
                int totalFirstOrderSymbols = currSymbolKeyValuePair.Value.Values.Sum();
                double h = 0;
                foreach (var currSymbolDict in currSymbolKeyValuePair.Value)
                {
                    double probability = (double)currSymbolDict.Value / totalFirstOrderSymbols;
                    h += probability * Math.Log2(1/probability);
                }

                entropy += ((double)symbolsCount[currSymbolKeyValuePair.Key] / totalSymbols) * h;
            }

            return entropy;
        }

        private static Dictionary<char, Dictionary<char, int>> ReadMarkovFirstOrderCount(FileInfo fileName)
        {
            Dictionary<char, Dictionary<char, int>> symbolsCountMfo = new Dictionary<char, Dictionary<char, int>>();
            using (FileStream fileStream = new FileStream(fileName.FullName, FileMode.Open, FileAccess.Read))
            {
                char lastSymbol = (char)0;
                for (int i = 0; i < fileStream.Length; i++)
                {
                    char currSymbol = Convert.ToChar(fileStream.ReadByte());
                    if(currSymbol > (char)255)
                        throw new InvalidDataException($"Invalid symbol {currSymbol}");

                    if (i == 0)
                    {
                        lastSymbol = currSymbol;
                        symbolsCountMfo.Add(currSymbol, new Dictionary<char, int>());
                        continue;
                    }
                    
                    if (symbolsCountMfo.TryGetValue(currSymbol, out Dictionary<char, int> currSymbolDict))
                        if (currSymbolDict.TryGetValue(lastSymbol, out _))
                            currSymbolDict[lastSymbol]++;
                        else
                            currSymbolDict.Add(lastSymbol, 1);
                    else
                    {
                        symbolsCountMfo.Add(currSymbol, new Dictionary<char, int>());
                        symbolsCountMfo[currSymbol].Add(lastSymbol, 1);
                    }

                    lastSymbol = currSymbol;
                }
            }
            // Console.WriteLine("----------");
            // Console.WriteLine($"Total Different Symbols: {symbols.Count}");
            // Console.WriteLine($"Total Symbol Count: {symbols.Values.Aggregate(0, (amount, dict) => amount += dict.Values.Sum()) + 1}");
            return symbolsCountMfo;
        }
    }
}