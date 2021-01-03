using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TP2
{
    public class ImageProcessor
    {
        public static FileInfo EncodeToWebp(FileInfo input, string append, long quality, bool lossless = false)
        {
            string newFile =
                $"{input.DirectoryName}\\" +
                $"{(Directory.Exists(input.DirectoryName + "\\result\\") ? $"result\\{Path.GetFileNameWithoutExtension(input.Name)}\\" : "")}" +
                $"{Path.GetFileNameWithoutExtension(input.Name)}_{append}.webp";

            if (File.Exists(newFile))
                return new FileInfo(newFile);

            using var webPFileStream = new FileStream(newFile, FileMode.Create);
            using Bitmap bitmap = new Bitmap(input.FullName);
            new Imazen.WebP.SimpleEncoder().Encode(bitmap, webPFileStream, lossless ? -1f : quality);
            return new FileInfo(newFile);
        }

        public static FileInfo EncodeToJpeg(FileInfo input, string append, long quality, bool defaultQuality = false)
        {
            string newFile =
                $"{input.DirectoryName}\\" +
                $"{(Directory.Exists(input.DirectoryName + "\\result\\") ? $"result\\{Path.GetFileNameWithoutExtension(input.Name)}\\" : "")}" +
                $"{Path.GetFileNameWithoutExtension(input.Name)}_{append}.jpg";

            if (File.Exists(newFile))
                return new FileInfo(newFile);

            // Create a Bitmap object based on a BMP file.
            Bitmap image;
            if (input.Extension == ".webp")
            {
                byte[] img1Bytes = File.ReadAllBytes(input.FullName);
                image = new Imazen.WebP.SimpleDecoder().DecodeFromBytes(img1Bytes, img1Bytes.Length);
            }
            else
                image = new Bitmap(input.FullName);

            // Get an ImageCodecInfo object that represents the JPEG codec.
            var myImageCodecInfo = GetEncoderInfo("image/jpeg");

            // Create an Encoder object based on the GUID
            // for the Quality parameter category.
            Encoder myEncoder;
            if (!defaultQuality)
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
            var myEncoderParameters = new EncoderParameters(1);

            // Save the bitmap as a JPEG file
            var myEncoderParameter = new EncoderParameter(myEncoder, quality);
            myEncoderParameters.Param[0] = myEncoderParameter;
            image.Save(newFile, myImageCodecInfo, myEncoderParameters);

            image.Dispose();
            return new FileInfo(newFile);
        }

        public static FileInfo CalculateErrorInWebpLossless(FileInfo image1, FileInfo image2)
        {
            string newFile =
                $"{image1.DirectoryName}\\" +
                $"{(Directory.Exists(image1.DirectoryName + "\\result\\") ? $"result\\{Path.GetFileNameWithoutExtension(image1.Name)}\\" : "")}" +
                $"{Path.GetFileNameWithoutExtension(image1.Name)}_ErrorDiff_{Path.GetFileNameWithoutExtension(image2.Name)}.webp";

            if (File.Exists(newFile))
                return new FileInfo(newFile);

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

            Bitmap img3 = new Bitmap(img1.Width, img2.Height);

            for (int i = 0; i < img1.Height; i++)
            {
                for (int j = 0; j < img1.Width; j++)
                {
                    Color p1 = img1.GetPixel(j, i);
                    Color p2 = img2.GetPixel(j, i);
                    int errR = Math.Abs(p2.R - p1.R);
                    int errG = Math.Abs(p2.G - p1.G);
                    int errB = Math.Abs(p2.B - p1.B);
                    img3.SetPixel(j, i, Color.FromArgb(errR, errG, errB));
                }
            }


            using var webPFileStream = new FileStream(newFile, FileMode.Create);
            new Imazen.WebP.SimpleEncoder().Encode(img3, webPFileStream, -1f);

            img1.Dispose();
            img2.Dispose();
            img3.Dispose();
            return new FileInfo(newFile);
        }

        public static double CalculatePsnr(FileInfo image1, FileInfo image2)
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
                    sumSq += (double) (errR * errR) / 3 + (double) (errG * errG) / 3 + (double) (errB * errB) / 3;
                }
            }

            double mse = sumSq / (img1.Height * img1.Width);
            double psnr = 20 * Math.Log10(255d / Math.Sqrt(mse));

            img1.Dispose();
            img2.Dispose();
            return psnr;
        }

        public static float SizeDifference(long first, long second)
        {
            return (float) (first - second) / first * 100;
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }

            return null;
        }
    }
}