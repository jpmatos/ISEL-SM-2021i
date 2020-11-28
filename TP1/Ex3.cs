using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;

namespace SMTP1
{
    public static class Ex3
    {
        public static void Entry(string fileName)
        {
            //a.
            List<byte> source = Common.ReadFile(fileName);
            List<byte> sequenceFirstOrder = GenerateClaudeShannonFirstOrderSequence(source);
            List<byte> sequenceSecondOrder = GenerateClaudeShannonSecondOrderSequence(source);

            //b.
            //File
            Dictionary<char, int> symbolsCount = Common.ReadSymbolsCount(source);
            double entropy = Common.CalculateEntropy(symbolsCount);
            Dictionary<char, Dictionary<char, int>> symbolsCountMfo = Common.ReadMarkovFirstOrderCount(source);
            double entropyMfo = Common.CalculateMarkovFirstOrderEntropy(symbolsCount, symbolsCountMfo);
            
            //Sequence First Order
            Dictionary<char, int> symbolsCountSequenceFo = Common.ReadSymbolsCount(sequenceFirstOrder);
            double entropySequenceFo = Common.CalculateEntropy(symbolsCountSequenceFo);
            Dictionary<char, Dictionary<char, int>> symbolsCountMfoSequenceFo = Common.ReadMarkovFirstOrderCount(sequenceFirstOrder);
            double entropyMfoSequenceFo = Common.CalculateMarkovFirstOrderEntropy(symbolsCountSequenceFo, symbolsCountMfoSequenceFo);

            //Sequence Second Order
            Dictionary<char, int> symbolsCountSequenceSo = Common.ReadSymbolsCount(sequenceSecondOrder);
            double entropySequenceSo = Common.CalculateEntropy(symbolsCountSequenceSo);
            Dictionary<char, Dictionary<char, int>> symbolsCountMfoSequenceSo = Common.ReadMarkovFirstOrderCount(sequenceSecondOrder);
            double entropyMfoSequenceSo = Common.CalculateMarkovFirstOrderEntropy(symbolsCountSequenceSo, symbolsCountMfoSequenceSo);
            
            Print.PrintSourceAndSequenceEntropy(
                symbolsCount.Count, symbolsCount.Values.Sum(),  entropy, entropyMfo, 
                symbolsCountSequenceFo.Count, symbolsCountSequenceFo.Values.Sum(), entropySequenceFo, entropyMfoSequenceFo,
                symbolsCountSequenceSo.Count,symbolsCountSequenceSo.Values.Sum(), entropySequenceSo, entropyMfoSequenceSo);

            //c.
            long sourceLengthCompressed = GetCompressionLength(source, out long sourceLengthUncompressed);
            long sequenceFoLengthCompressed = GetCompressionLength(sequenceFirstOrder, out long sequenceFoLengthUncompressed);
            long sequenceSoLengthCompressed = GetCompressionLength(sequenceSecondOrder, out long sequenceSoLengthUncompressed);
            
            Print.PrintCompressionLengths(sourceLengthUncompressed, sourceLengthCompressed,
                sequenceFoLengthUncompressed, sequenceFoLengthCompressed,
                sequenceSoLengthUncompressed, sequenceSoLengthCompressed);
        }

        private static long GetCompressionLength(List<byte> source, out long lengthUncompressed)
        {
            using MemoryStream unZippedChunk = new MemoryStream(source.ToArray());
            lengthUncompressed = unZippedChunk.Length;

            using MemoryStream zippedChunk = new MemoryStream();
            using ZipOutputStream zipOutputStream = new ZipOutputStream(zippedChunk);
            zipOutputStream.SetLevel(9);

            ZipEntry entry = new ZipEntry("name");
            zipOutputStream.PutNextEntry(entry);

            unZippedChunk.CopyTo(zipOutputStream);
            return zipOutputStream.Length;
        }

        private static List<byte> GenerateClaudeShannonFirstOrderSequence(List<byte> book)
        {
            List<byte> sequence = new List<byte>();
            Random random = new Random();

            foreach (var _ in book)
            {
                int index = random.Next(0, book.Count);
                sequence.Add(book[index]);
            }

            return sequence;
        }

        private static List<byte> GenerateClaudeShannonSecondOrderSequence(List<byte> book)
        {
            List<byte> sequence = new List<byte>();

            Random random = new Random();
            int index = random.Next(0, book.Count);
            byte current = book[index];
            sequence.Add(current);

            foreach (var _ in book.Skip(1))
            {
                index = random.Next(0, book.Count);
                for (int j = index; j <= book.Count; j++)
                {
                    if (j == book.Count)
                        j = 0;
                    if (book[j] != current)
                        continue;

                    if (j == book.Count - 1)
                        index = 0;
                    else
                        index = j + 1;
                    current = book[index];
                    sequence.Add(current);
                    break;
                }
            }

            return sequence;
        }
    }
}