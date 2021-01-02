using System;
using System.IO;

namespace TP2
{
    static class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\Users\\jpmatos\\RiderProjects\\ISEL-SM-2021i\\kodak\\kodim01.png";
            //string path = "";
            if (String.IsNullOrEmpty(path))
            {
                Console.WriteLine("File to read:");
                path = Console.ReadLine();
            }
            
            FileInfo png = new FileInfo(path);
            
            //1.1 PNG to JPEG varying quality
            PngToJpegVaryQuality(png);
            Console.WriteLine();
            
            //1.1.1 (PNG to JPEG) to WebP (Loss & Max Quality)
            PngToJpegToWebpLossyMaxQuality(png);
            
            //1.1.2
            
        }

        private static void PngToJpegToWebpLossyMaxQuality(FileInfo png)
        {
            FileInfo pngToJpeg = ImageProcessor.EncodeToJpeg(png, $"ToJpeg", 100, true);
            FileInfo pngToJpegToWebpLossy = ImageProcessor.EncodeToWebp(pngToJpeg, "ToWebpLossy", false);
            double pngToJpeg_pngToJpegToWebpLossy_psnr = ImageProcessor.CalculatePsnr(pngToJpeg, pngToJpegToWebpLossy);
            double png_pngToJpegToWebpLossy_psnr = ImageProcessor.CalculatePsnr(png, pngToJpegToWebpLossy);

            Console.WriteLine($"1.1.1 PNG to JPEG to Webp (Loss & Max Quality)" +
                              $"\nPNGtoJPEG-PNGtoJPEGtoWebpLossy:" +
                              $"\nDistortion: {pngToJpeg_pngToJpegToWebpLossy_psnr:N2} Reduction: {ImageProcessor.SizeDifference(pngToJpeg.Length, pngToJpegToWebpLossy.Length):N2}%" +
                              $"\nPNG-PNGtoJPEGtoWebpLossy:" +
                              $"\nDistortion: {png_pngToJpegToWebpLossy_psnr:N2} Reduction: {ImageProcessor.SizeDifference(png.Length, pngToJpegToWebpLossy.Length):N2}%");

            FileInfo errorBetweenJpegAndWebpLossyInWebpLossless = ImageProcessor.CalculateErrorInWebpLossless(pngToJpeg, pngToJpegToWebpLossy);
            FileInfo pngToJpegToWebpLossless = ImageProcessor.EncodeToWebp(pngToJpeg, "ToWebpLossless", true);
            
            Console.WriteLine("\nWebpLossless-WebpLossy:" +
                              $"\nReduction: {ImageProcessor.SizeDifference(pngToJpegToWebpLossless.Length, pngToJpegToWebpLossy.Length):N2}%" +
                              $"\nWebpLossless-Error:" +
                              $"\nReduction: {ImageProcessor.SizeDifference(pngToJpegToWebpLossless.Length, errorBetweenJpegAndWebpLossyInWebpLossless.Length):N2}%");
        }

        private static void PngToJpegVaryQuality(FileInfo png)
        {
            Console.WriteLine("1.1 PNG to JPEG");
            for (int i = 0; i <= 4; i++)
            {
                FileInfo pngToJpegVar = ImageProcessor.EncodeToJpeg(png, $"ToJpeg{i * 25}", i * 25);
                double pngToJpegVarPstr = ImageProcessor.CalculatePsnr(png, pngToJpegVar);
                float sizeDifference = ImageProcessor.SizeDifference(png.Length, pngToJpegVar.Length); 
            
                Console.WriteLine($"PNG to JPEG{i * 25}. Distortion: {pngToJpegVarPstr:N2} Reduction: {sizeDifference:N2}%");   
            }
        }
    }
}
