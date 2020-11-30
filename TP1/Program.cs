using System;

namespace SMTP1
{
    public static class Program
    {
        //For debugging purposes
        private static readonly string fileName = "/home/jpmatos/Documents/SM/Trab1/corpusSM2021/pg3333.txt";
        //private static readonly string fileName = "/home/jpmatos/Documents/SM/Trab1/corpusSM2021/34767-0.txt";
        //private static readonly string fileName = "/home/jpmatos/Documents/SM/Trab1/testfile.txt";
        private static int ex = 4;
        
        public static void Main(string[] args)
        {
            if (ex == 0)
            {
                Console.WriteLine("Pick Exercise: (1-7)");
                ex = Console.Read();
            }
            switch (ex)
            {
                case 1:
                    Ex1.Entry(fileName);
                    break;
                case 2:
                    Ex2.Entry(fileName);
                    break;
                case 3:
                    Ex3.Entry(fileName);
                    break;
                case 4:
                    Ex4.Entry();
                    break;
                case 7:
                    Ex7.Entry(fileName);
                    break;
            }
        }
    }
}