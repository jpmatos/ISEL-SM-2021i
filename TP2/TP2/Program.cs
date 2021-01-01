using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using WebPWrapper;

namespace TP2
{
    static class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\jpmatos\\Files\\Kodim\\kodim01.png";
            //string path = "";
            if (String.IsNullOrEmpty(path))
            {
                Console.WriteLine("File to read:");
                path = Console.ReadLine();
            }
            
            FileInfo png = new FileInfo(path);
            
            //PNG to JPEG varying quality
            //PngToJpegVaryQuality(png);
            
            FileInfo pngToJpeg = EncodeToJpeg(png, $"ToJpeg", 100, true);
            FileInfo pngToJpegToWebpLossy = EncodeToWebp(pngToJpeg, "ToWebpLossy", false);
            double pngToJpeg_pngToJpegToWebpLossy_psnr = CalculatePsnr(pngToJpeg, pngToJpegToWebpLossy);
            double png_pngToJpegToWebpLossy_psnr = CalculatePsnr(png, pngToJpegToWebpLossy);
            
            Console.WriteLine($"PNG to JPEG to WebP. Distortion (PNGtoJPEG): {pngToJpeg_pngToJpegToWebpLossy_psnr}; " +
                              $"Distortion (PNG): {png_pngToJpegToWebpLossy_psnr}");

            // FileInfo pngToJpegLossless = EncodeToJpeg(png, "ToJpegLossless", true);
            // double pngToJpegLosslessPstr = CalculatePsnr(png, pngToJpegLossless);
            // Console.WriteLine($"PNG to Lossless JPEG Distortion: {pngToJpegLosslessPstr}");
        }

        private static void PngToJpegVaryQuality(FileInfo png)
        {
            for (int i = 0; i <= 4; i++)
            {
                FileInfo pngToJpegVar = EncodeToJpeg(png, $"ToJpeg{i * 25}", i * 25);
                double pngToJpegVarPstr = CalculatePsnr(png, pngToJpegVar);
                float sizeDifference = ((float)(png.Length - pngToJpegVar.Length) / png.Length * 100);
            
                Console.WriteLine($"PNG to JPEG{i * 25}. Distortion: {pngToJpegVarPstr}; Reduction: {sizeDifference:N2}");   
            }
        }

        private static FileInfo EncodeToWebp(FileInfo input, string append, bool lossless)
        {
            string newFile =
                $"{input.DirectoryName}\\{Path.GetFileNameWithoutExtension(input.Name)}_{append}.webp";
            
            using (var webPFileStream = new FileStream(newFile, FileMode.Create))
            {
                using (ImageFactory imageFactory = new ImageFactory(false))
                {
                    imageFactory.Load(input.OpenRead())
                        .Format(new WebPFormat())
                        .Quality(100)
                        .Save(webPFileStream);
                }
            }
            return new FileInfo(newFile);
        }

        private static FileInfo EncodeToJpeg(FileInfo input, string append, long quality, bool defaultQuality = false)
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
            if(!defaultQuality)
                myEncoder = Encoder.Quality;
            else
            {
                quality = 100;
                myEncoder = Encoder.Compression;
            }

            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one
            // EncoderParameter object in the array.
            myEncoderParameters = new EncoderParameters(1);

            string newFile =
                $"{input.DirectoryName}\\result\\{Path.GetFileNameWithoutExtension(input.Name)}_{append}.jpg";

            if (File.Exists(newFile))
            {
                new FileInfo(newFile).Delete();
            }

            // Save the bitmap as a JPEG file
            myEncoderParameter = new EncoderParameter(myEncoder, quality);
            myEncoderParameters.Param[0] = myEncoderParameter;
            myPng.Save(newFile, myImageCodecInfo, myEncoderParameters);
            
            return new FileInfo(newFile);
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

        private static double CalculatePsnr(FileInfo image1, FileInfo image2)
        {
            Bitmap img1;
            if (image1.Extension == ".webp")
            {
                byte[] img1Bytes = File.ReadAllBytes(image1.FullName);
                img1 = new Imazen.WebP.SimpleDecoder().DecodeFromBytes(img1Bytes, img1Bytes.Length);
            }
            else
                img1 = new Bitmap(image1.FullName);
            
            Bitmap img2;
            if (image2.Extension == ".webp")
            {
                byte[] img2Bytes = File.ReadAllBytes(image2.FullName);
                img2 = new Imazen.WebP.SimpleDecoder().DecodeFromBytes(img2Bytes, img2Bytes.Length);
            }
            else
                img2 = new Bitmap(image2.FullName);
            
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
    }
}
