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
            List<byte> sequence = GenerateClaudeShannonSequence(source);

            //b.
            Dictionary<char, int> symbolsCount = Common.ReadSymbolsCount(source);
            double entropy = Common.CalculateEntropy(symbolsCount);
            Dictionary<char, Dictionary<char, int>> symbolsCountMfo = Common.ReadMarkovFirstOrderCount(source);
            double entropyMfo = Common.CalculateMarkovFirstOrderEntropy(symbolsCount, symbolsCountMfo);

            Dictionary<char, int> symbolsCountSeq = Common.ReadSymbolsCount(sequence);
            double entropySeq = Common.CalculateEntropy(symbolsCountSeq);
            Dictionary<char, Dictionary<char, int>> symbolsCountMfoSeq = Common.ReadMarkovFirstOrderCount(sequence);
            double entropyMfoSeq = Common.CalculateMarkovFirstOrderEntropy(symbolsCountSeq, symbolsCountMfoSeq);
            Print.PrintSourceAndSequenceEntropy(symbolsCount.Count, symbolsCount.Values.Sum(), symbolsCountSeq.Count,
                symbolsCountSeq.Values.Sum(),
                entropy, entropyMfo, entropySeq, entropyMfoSeq);

            //c.
            long sourceLengthCompressed = GetCompressionLength(source, out long sourceLengthUncompressed);
            long sequenceLengthCompressed = GetCompressionLength(sequence, out long sequenceLengthUncompressed);
            Print.PrintCompressionLengths(sourceLengthUncompressed, sourceLengthCompressed, sequenceLengthUncompressed,
                sequenceLengthCompressed);
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

        private static List<byte> GenerateClaudeShannonSequence(List<byte> book)
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