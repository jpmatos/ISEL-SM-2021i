using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Image = System.Drawing.Image;

namespace TP2
{
    static class Program
    {
        static void Main(string[] args)
        {
            string path = "/home/jpmatos/Documents/SM/Trab2/kodak/kodim01.png";
            //string path = "";
            if (String.IsNullOrEmpty(path))
            {
                Console.WriteLine("File to read:");
                path = Console.ReadLine();
            }
            
            FileInfo input = new FileInfo(path);
            FileInfo outputLossless = EncodeToJpeg(input, true);
            FileInfo outputLossy = EncodeToJpeg(input, false);
            
            double pstrLossless = CalculatePSNR(input, outputLossless);
            double pstrLossy = CalculatePSNR(input, outputLossy);
            Console.WriteLine($"Lossless JPEG Distortion: {pstrLossless}");
            Console.WriteLine($"Lossy JPEG Distortion: {pstrLossy}");
        }

        private static double CalculatePSNR(FileInfo image1, FileInfo image2)
        {
            Bitmap img1 = new Bitmap(image1.FullName);
            Bitmap img2 = new Bitmap(image2.FullName);
            double sumSq = 0;
            for (int i = 0; i < img1.Height; i++)
            {
                for (int j = 0; j < img1.Width; j++)
                {
                    Color p1 = img1.GetPixel(j, i);
                    Color p2 = img2.GetPixel(j, i);
                    int errR = Math.Abs(p2.R - p1.R);
                    int errG = Math.Abs(p2.G - p1.G);
                    int errB = Math.Abs(p2.B - p1.B);
                    sumSq += (double)(errR * errR) / 3 + (double)(errG * errG) / 3 + (double)(errB * errB) / 3;
                }
            }
            
            double mse = sumSq / (img1.Height * img1.Width);
            double psnr = 20 * Math.Log10(255d / Math.Sqrt(mse));
            
            return psnr;
        }

        private static FileInfo EncodeToJpeg(FileInfo input, bool quality)
        {
            Image myPng;
            ImageCodecInfo myImageCodecInfo;
            Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            // Create a Bitmap object based on a BMP file.
            myPng = Image.FromFile(input.FullName);

            // Get an ImageCodecInfo object that represents the JPEG codec.
            myImageCodecInfo = GetEncoderInfo("image/jpeg");

            // Create an Encoder object based on the GUID
            // for the Quality parameter category.
            if(quality)
                myEncoder = Encoder.Quality;
            else 
                myEncoder = Encoder.Compression;

            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one
            // EncoderParameter object in the array.
            myEncoderParameters = new EncoderParameters(1);

            string newfile =
                $"{input.DirectoryName}/result/{input.Name.Replace(input.Extension, "")}_{(quality ? "lossless": "loss")}.jpg";

            if (File.Exists(newfile))
            {
                new FileInfo(newfile).Delete();
            }

            // Save the bitmap as a JPEG file with quality level 100.
            myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            myPng.Save(newfile, myImageCodecInfo, myEncoderParameters);
            
            return new FileInfo(newfile);
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for(j = 0; j < encoders.Length; ++j)
            {
                if(encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
    }
}
