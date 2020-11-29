using System;
using System.Collections.Generic;

namespace SMTP1
{
    public static class Ex4
    {
        public static void Entry()
        {
            Random random = new Random();
            for (int i = 0; i <= 10; i++)
            {
                double p = 0.1 * i;

                double entropy = 0;
                for (int j = 0; j < 255; j++)
                {
                    if (p == 0)
                        entropy += (1d / 255) * 
                                   (((1d - p) / 2) * Math.Log2(1 / ((1d - p) / 2)) +
                                    ((1d - p) / 2) * Math.Log2(1 / ((1d - p) / 2)));
                    else if (p == 1)
                        entropy += (1d / 255) * 
                                   ((p / 3) * Math.Log2(1 / (p / 3)) + 
                                    (p / 3) * Math.Log2(1 / (p / 3)) +
                                    (p / 3) * Math.Log2(1 / (p / 3)));
                    else
                        entropy += (1d / 255) * 
                                   ((p / 3) * Math.Log2(1 / (p / 3)) + 
                                    (p / 3) * Math.Log2(1 / (p / 3)) +
                                    (p / 3) * Math.Log2(1 / (p / 3)) +
                                    ((1d - p) / 2) * Math.Log2(1 / ((1d - p) / 2)) +
                                    ((1d - p) / 2) * Math.Log2(1 / ((1d - p) / 2)));
                }
            
                //intervals
                double first = 0d;
                double second = p / 3;
                double third = (p / 3) * 2;
                double fourth = (p / 3) * 3;
                double fifth = (p / 3) * 3 + (1d - p) / 2;
                double sixth = 1d;
                
                List<byte> sequence = new List<byte>();
                byte current = Convert.ToByte(random.Next(0, 255));
                sequence.Add(current);

                for (long j = 1; j < 300000; j++)
                {
                    double chance = random.NextDouble();
                    //Move back 1
                    if (first <= chance && chance < second)
                    {
                        if (current == 0)
                            current = 255;
                        else
                            current--;
                    }
                    //Stay
                    if (second <= chance && chance < third)
                    {
                        //nothing
                    }
                    //Move forward 1
                    if (third <= chance && chance < fourth)
                    {
                        if (current == 255)
                            current = 0;
                        else
                            current++;
                    }
                    //Move back 2
                    if (fourth <= chance && chance < fifth)
                    {
                        if (current == 0)
                            current = 254;
                        else if (current == 1)
                            current = 255;
                        else
                            current -= 2;
                    }
                    //Move forward 2
                    if (fifth <= chance && chance < sixth)
                    {
                        if (current == 255)
                            current = 1;
                        else if (current == 254)
                            current = 0;
                        else
                            current += 2;
                    }
                    sequence.Add(current);
                }

                long lengthCompressed = Common.GetCompressionLength(sequence, out long lengthUncompressed);
                
                Console.WriteLine($"p='{p:N1}' MFO Entropy: '{entropy}'. Uncompressed: '{lengthUncompressed}'. Compressed: '{lengthCompressed}'");
            }
        }
    }
}