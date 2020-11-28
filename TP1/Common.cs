using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SMTP1
{
    internal static class Common
    {
        internal static List<byte> ReadFile(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                Console.WriteLine("File to read:");
                fileName = Console.ReadLine();
            }
            FileInfo file = new FileInfo(fileName);
            Print.PrintFileName(file.Name);
            byte[] sourceArray = File.ReadAllBytes(file.FullName);
            return sourceArray.ToList();
        }
        
        internal static Dictionary<char, int> ReadSymbolsCount(List<byte> source)
        {
            Dictionary<char, int> symbolsCount = new Dictionary<char, int>();
                foreach (var t in source)
                {
                    char symbol = Convert.ToChar(t);
                    if(symbol > 255)
                        throw new InvalidDataException($"Invalid symbol {symbol}");
                    
                    if (symbolsCount.TryGetValue(symbol, out int currentCount))
                        symbolsCount[symbol] = currentCount + 1;
                    else
                        symbolsCount.Add(symbol, 1);
                }
            return symbolsCount;
        }

        internal static double CalculateEntropy(Dictionary<char,int> symbols)
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

        internal static double CalculateMarkovFirstOrderEntropy(Dictionary<char, int> symbolsCount,
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

        internal static Dictionary<char, Dictionary<char, int>> ReadMarkovFirstOrderCount(List<byte> source)
        {
            Dictionary<char, Dictionary<char, int>> symbolsCountMfo = new Dictionary<char, Dictionary<char, int>>();
                char lastSymbol = (char)0;
                for (int i = 0; i < source.Count; i++)
                {
                    char currSymbol = Convert.ToChar(source[i]);
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
            return symbolsCountMfo;
        }
    }
}