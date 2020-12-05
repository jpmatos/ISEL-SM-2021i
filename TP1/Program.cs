using System;

namespace SMTP1
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            string ex = "0";
            string fileName = "";
            if (ex == "0")
            {
                Console.WriteLine("Pick Exercise: (1, 2, 3, 4, 7)");
                ex = Console.ReadLine();
            }
            switch (ex)
            {
                case "1":
                    Ex1.Entry(fileName);
                    break;
                case "2":
                    Ex2.Entry(fileName);
                    break;
                case "3":
                    Ex3.Entry(fileName);
                    break;
                case "4":
                    Ex4.Entry();
                    break;
                case "7":
                    Ex7.Entry(fileName);
                    break;
            }
        }
    }
}