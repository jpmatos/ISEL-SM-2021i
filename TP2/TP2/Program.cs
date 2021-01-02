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
            
            //1.1 PNGto JPEG varying quality
            PngToJpegVaryQuality(png);
            Console.WriteLine();
            
            //1.1.1 (PNG to JPEG) to WebP (Loss & Max Quality)
            PngToJpegToWebpLossyMaxQuality(png);
            Console.WriteLine();
            
            //1.1.2 Error PNG and (PNG to JPEG) compare with (PNG to WebP) (Lossless)
            ErrorPngAndPngToJpegCompareWithPngToWebpLossless(png);
        }

        private static void ErrorPngAndPngToJpegCompareWithPngToWebpLossless(FileInfo png)
        {
            FileInfo pngToJpeg = ImageProcessor.EncodeToJpeg(png, $"ToJpeg", 100, true);
            FileInfo errorPngAndPngToJpeg = ImageProcessor.CalculateErrorInWebpLossless(png, pngToJpeg);
            FileInfo pngToWebpLossless = ImageProcessor.EncodeToWebp(png, "ToWebpLossless", true);
            
            Console.WriteLine("1.1.2 Error PNG-JPEGtoPNG compared with PNGtoWebP (Lossless)" +
                              "\nErrorPngAndPngToJpeg-PngToWebpLossless:" +
                              $"\n\tReduction: {ImageProcessor.SizeDifference(errorPngAndPngToJpeg.Length, pngToWebpLossless.Length):N2}%");
        }

        private static void PngToJpegToWebpLossyMaxQuality(FileInfo png)
        {
            FileInfo pngToJpeg = ImageProcessor.EncodeToJpeg(png, $"ToJpeg", 100, true);
            FileInfo pngToJpegToWebpLossy = ImageProcessor.EncodeToWebp(pngToJpeg, "ToWebpLossy", false);
            double psnrPngToJpegAndPngToJpegToWebpLossy = ImageProcessor.CalculatePsnr(pngToJpeg, pngToJpegToWebpLossy);
            double psnrPngAndPngToJpegToWebpLossy = ImageProcessor.CalculatePsnr(png, pngToJpegToWebpLossy);

            Console.WriteLine($"1.1.1a PNG to JPEG to WebP (Lossy)" +
                              $"\nPNGtoJPEG-PNGtoJPEGtoWebpLossy:" +
                              $"\n\tDistortion: {psnrPngToJpegAndPngToJpegToWebpLossy:N2}dB Reduction: {ImageProcessor.SizeDifference(pngToJpeg.Length, pngToJpegToWebpLossy.Length):N2}%" +
                              $"\nPNG-PNGtoJPEGtoWebpLossy:" +
                              $"\n\tDistortion: {psnrPngAndPngToJpegToWebpLossy:N2}dB Reduction: {ImageProcessor.SizeDifference(png.Length, pngToJpegToWebpLossy.Length):N2}%");

            FileInfo errorPngToJpegAndPngToJpegToWebpLossy = ImageProcessor.CalculateErrorInWebpLossless(pngToJpeg, pngToJpegToWebpLossy);
            FileInfo pngToJpegToWebpLossless = ImageProcessor.EncodeToWebp(pngToJpeg, "ToWebpLossless", true);
            
            Console.WriteLine("\n1.1.1b PNG to WebP Lossless comparisons" +
                              "\nPngToWebpLossless-PngToJpegToWebpLossy:" +
                              $"\n\tReduction: {ImageProcessor.SizeDifference(pngToJpegToWebpLossless.Length, pngToJpegToWebpLossy.Length):N2}%" +
                              $"\nPngToWebpLossless-ErrorPngToJpgAndPngToJpegtoWebpLossy:" +
                              $"\n\tReduction: {ImageProcessor.SizeDifference(pngToJpegToWebpLossless.Length, errorPngToJpegAndPngToJpegToWebpLossy.Length):N2}%");
        }

        private static void PngToJpegVaryQuality(FileInfo png)
        {
            Console.WriteLine("1.1 PNG to JPEG Varying");
            for (int i = 0; i <= 4; i++)
            {
                FileInfo pngToJpegVar = ImageProcessor.EncodeToJpeg(png, $"ToJpeg{i * 25}", i * 25);
                double pngToJpegVarPstr = ImageProcessor.CalculatePsnr(png, pngToJpegVar);
                float sizeDifference = ImageProcessor.SizeDifference(png.Length, pngToJpegVar.Length); 
            
                Console.WriteLine($"PNG to JPEG{i * 25}. Distortion: {pngToJpegVarPstr:N2}dB Reduction: {sizeDifference:N2}%");   
            }
        }
    }
}
