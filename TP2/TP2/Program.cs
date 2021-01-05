using System;
using System.IO;

namespace TP2
{
    static class Program
    {
        private static void Main(string[] args)
        {
            //Read PNG
            FileInfo png = ReadPng(args.Length > 0 ? args[0] : "");
            
            //Set Console.Write to text file
            SetConsoleWriteToFile(png);

            //1.1 PNG to JPEG varying quality
            PngToJpegVaryQuality(png);
            Console.WriteLine();
            
            //1.1.1 (PNG to JPEG) to WebP (Loss & Max Quality)
            PngToJpegToWebpLossyMaxQuality(png);
            Console.WriteLine();
            
            //1.1.2 Error PNG - (PNG to JPEG) compare with (PNG to WebP) (Lossless)
            ErrorPngAndPngToJpegCompareWithPngToWebpLossless(png);
            Console.WriteLine();
            
            //2.1 PNG to WebP (Lossy) varying quality
            PngToWebpLossyVaryQuality(png);
            Console.WriteLine();
            
            //2.1.1 (PNG to WebP Lossy) to JPEG varying quality. Compare with (PNG to WebP Lossy) and PNG.
            //Compare with (PNG to JPEG varying quality)
            PngToWebpLossyToJpegVaryQualityAndMore(png);
        }

        private static FileInfo ReadPng(string path)
        {
            if (String.IsNullOrEmpty(path))
            {
                Console.WriteLine("File to read:");
                path = Console.ReadLine();
            }
            
            FileInfo res = new FileInfo(path);

            if (!Directory.Exists(res.DirectoryName + "\\result"))
                Directory.CreateDirectory(res.DirectoryName + "\\result");
            
            if (!Directory.Exists(res.DirectoryName + "\\result\\" + Path.GetFileNameWithoutExtension(res.Name)))
                Directory.CreateDirectory(res.DirectoryName + "\\result\\" + Path.GetFileNameWithoutExtension(res.Name));
            
            return res;
        }

        private static void SetConsoleWriteToFile(FileInfo png)
        {
            string newfile = $"{png.DirectoryName}\\result\\{Path.GetFileNameWithoutExtension(png.Name)}.txt";
            if(File.Exists(newfile))
                new FileInfo(newfile).Delete();
            FileStream filestream = new FileStream(newfile, FileMode.Append);
            var streamwriter = new StreamWriter(filestream);
            streamwriter.AutoFlush = true;
            Console.SetOut(streamwriter);
            Console.SetError(streamwriter);
        }

        private static void PngToWebpLossyToJpegVaryQualityAndMore(FileInfo png)
        {
            Console.WriteLine("2.1.1 PNG to WebPLossy to JPEG Varying");
            FileInfo pngToWebpLossy = ImageProcessor.EncodeToWebp(png, $"ToWebpLossy", 100);
            
            Console.WriteLine("PNGtoWebPLossy-PNGtoWebPLossyToJPEG");
            for (int i = 0; i <= 4; i++)
            {
                FileInfo pngToWebpLossyToJpegVar = ImageProcessor.EncodeToJpeg(pngToWebpLossy, $"ToJpeg{i * 25}", i * 25);
            
                Console.WriteLine($"\tPNG to WebPLossy to JPEG{i * 25}. " +
                                  $"Distortion: {ImageProcessor.CalculatePsnr(pngToWebpLossy, pngToWebpLossyToJpegVar):N2}dB " +
                                  $"Reduction: {ImageProcessor.SizeDifference(pngToWebpLossy.Length, pngToWebpLossyToJpegVar.Length):N2}%");   
            }
            
            Console.WriteLine("PNG-PNGtoWebPLossyToJPEG");
            for (int i = 0; i <= 4; i++)
            {
                FileInfo pngToWebpLossyToJpegVar = ImageProcessor.EncodeToJpeg(pngToWebpLossy, $"ToJpeg{i * 25}", i * 25);
            
                Console.WriteLine($"\tPNG to WebPLossy to JPEG{i * 25}. " +
                                  $"Distortion: {ImageProcessor.CalculatePsnr(png, pngToWebpLossyToJpegVar):N2}dB " +
                                  $"Reduction: {ImageProcessor.SizeDifference(png.Length, pngToWebpLossyToJpegVar.Length):N2}%");   
            }
        }

        private static void PngToWebpLossyVaryQuality(FileInfo png)
        {
            Console.WriteLine("2.1 PNG to WebPLossy Varying");
            for (int i = 0; i <= 4; i++)
            {
                FileInfo pngToWebpLossyVar = ImageProcessor.EncodeToWebp(png, $"ToWebp{i * 25}", i * 25);
                double pngToWebpLossyVarPstr = ImageProcessor.CalculatePsnr(png, pngToWebpLossyVar);
                float sizeDifference = ImageProcessor.SizeDifference(png.Length, pngToWebpLossyVar.Length); 
            
                Console.WriteLine($"\tPNG to WebPLossy{i * 25}. Distortion: {pngToWebpLossyVarPstr:N2}dB Reduction: {sizeDifference:N2}%");   
            }
        }

        private static void ErrorPngAndPngToJpegCompareWithPngToWebpLossless(FileInfo png)
        {
            FileInfo pngToJpeg = ImageProcessor.EncodeToJpeg(png, $"ToJpeg", 100, true);
            FileInfo errorPngAndPngToJpeg = ImageProcessor.CalculateErrorInWebpLossless(png, pngToJpeg);
            FileInfo pngToWebpLossless = ImageProcessor.EncodeToWebp(png, "ToWebpLossless", 100, true);
            
            Console.WriteLine("1.1.2 Error PNG-JPEGtoPNG compared with PNGtoWebP (Lossless)" +
                              "\nErrorPngAndPngToJpeg-PngToWebpLossless:" +
                              $"\n\tReduction: {ImageProcessor.SizeDifference(errorPngAndPngToJpeg.Length, pngToWebpLossless.Length):N2}%");
        }

        private static void PngToJpegToWebpLossyMaxQuality(FileInfo png)
        {
            FileInfo pngToJpeg = ImageProcessor.EncodeToJpeg(png, $"ToJpeg", 100, true);
            FileInfo pngToJpegToWebpLossy = ImageProcessor.EncodeToWebp(pngToJpeg, "ToWebpLossy",  100);
            double psnrPngToJpegAndPngToJpegToWebpLossy = ImageProcessor.CalculatePsnr(pngToJpeg, pngToJpegToWebpLossy);
            double psnrPngAndPngToJpegToWebpLossy = ImageProcessor.CalculatePsnr(png, pngToJpegToWebpLossy);

            Console.WriteLine($"1.1.1a PNG to JPEG to WebPLossy" +
                              $"\nPNGtoJPEG-PNGtoJPEGtoWebpLossy:" +
                              $"\n\tDistortion: {psnrPngToJpegAndPngToJpegToWebpLossy:N2}dB Reduction: {ImageProcessor.SizeDifference(pngToJpeg.Length, pngToJpegToWebpLossy.Length):N2}%" +
                              $"\nPNG-PNGtoJPEGtoWebpLossy:" +
                              $"\n\tDistortion: {psnrPngAndPngToJpegToWebpLossy:N2}dB Reduction: {ImageProcessor.SizeDifference(png.Length, pngToJpegToWebpLossy.Length):N2}%");

            FileInfo errorPngToJpegAndPngToJpegToWebpLossy = ImageProcessor.CalculateErrorInWebpLossless(pngToJpeg, pngToJpegToWebpLossy);
            FileInfo pngToJpegToWebpLossless = ImageProcessor.EncodeToWebp(pngToJpeg, "ToWebpLossless",  100, true);
            
            Console.WriteLine("\n1.1.1b Comparisons with PNG to WebPLossless" +
                              "\nPngToJpegToWebpLossy-PngToJpegToWebpLossless:" +
                              $"\n\tReduction: {ImageProcessor.SizeDifference(pngToJpegToWebpLossy.Length, pngToJpegToWebpLossless.Length):N2}%" +
                              $"\nErrorPngToJpgAndPngToJpegToWebpLossy-PngToJpegToWebpLossless:" +
                              $"\n\tReduction: {ImageProcessor.SizeDifference(errorPngToJpegAndPngToJpegToWebpLossy.Length, pngToJpegToWebpLossless.Length):N2}%");
        }

        private static void PngToJpegVaryQuality(FileInfo png)
        {
            Console.WriteLine("1.1 PNG to JPEG Varying");
            for (int i = 0; i <= 4; i++)
            {
                FileInfo pngToJpegVar = ImageProcessor.EncodeToJpeg(png, $"ToJpeg{i * 25}", i * 25);
                double pngToJpegVarPstr = ImageProcessor.CalculatePsnr(png, pngToJpegVar);
                float sizeDifference = ImageProcessor.SizeDifference(png.Length, pngToJpegVar.Length); 
            
                Console.WriteLine($"\tPNG to JPEG{i * 25}. Distortion: {pngToJpegVarPstr:N2}dB Reduction: {sizeDifference:N2}%");   
            }
        }
    }
}
