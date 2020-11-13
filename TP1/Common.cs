using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SMTP1
{
    internal static class Common
    {
        internal static FileInfo ReadFile(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
            {
                Console.WriteLine("File to read:");
                fileName = Console.ReadLine();
            }
            FileInfo file = new FileInfo(fileName);
            return file;
        }
        
        internal static Dictionary<char, int> ReadSymbolsCount(FileInfo fileName)
        {
            Dictionary<char, int> symbolsCount = new Dictionary<char, int>();
            using (FileStream fileStream = new FileStream(fileName.FullName, FileMode.Open, FileAccess.Read))
            {
                for (int i = 0; i < fileStream.Length; i++)
                {
                    char symbol = (char)fileStream.ReadByte();
                    if(symbol > 255)
                        throw new InvalidDataException($"Invalid symbol {symbol}");
                    
                    if (symbolsCount.TryGetValue(symbol, out int currentCount))
                        symbolsCount[symbol] = currentCount + 1;
                    else
                        symbolsCount.Add(symbol, 1);
                }
            }
            Console.WriteLine("----------");
            Console.WriteLine($"Total Different Symbols: {symbolsCount.Count}");
            Console.WriteLine($"Total Symbol Count: {symbolsCount.Values.Sum()}");
            return symbolsCount;
        }

        internal static void PrintEntropy(in double entropy)
        {
            Console.WriteLine("----------");
            Console.WriteLine($"The Entropy is {entropy}");
        }
    }
}