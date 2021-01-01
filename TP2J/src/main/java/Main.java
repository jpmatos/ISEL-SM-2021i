import com.luciad.imageio.webp.WebPWriteParam;
import com.sun.image.codec.jpeg.JPEGEncodeParam;
import com.sun.media.jai.codec.*;
import sun.awt.image.ImageDecoder;

import javax.imageio.IIOImage;
import javax.imageio.ImageIO;
import javax.imageio.ImageWriteParam;
import javax.imageio.ImageWriter;
import javax.imageio.stream.FileImageOutputStream;
import javax.imageio.stream.ImageOutputStream;
import java.awt.*;
import java.awt.image.BufferedImage;
import java.awt.image.RenderedImage;
import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.Scanner;

public class Main {
    public static void main(String[] args) throws IOException {

        String path = "/home/jpmatos/Documents/SM/Trab2/kodak/kodim01.png";
        //string path = "";
        if (path.equals("") || path == null)
        {
            Scanner scan = new Scanner(System.in);
            System.out.println("File to read:");
            path = scan.nextLine();
        }
        File png = new File(path);


        //PNG to JPEG varying quality
        //PngToJpegVaryQuality(png);

        File pngToJpeg = encodeToJpeg(png, "ToJpeg", 100);
        File pngToJpegToWebpLossy = encodeToWebp(pngToJpeg, "ToWebpLossy", false);
        double pngToJpeg_pngToJpegToWebpLossy_psnr = CalculatePsnr(pngToJpeg, pngToJpegToWebpLossy);
        double png_pngToJpegToWebpLossy_psnr = CalculatePsnr(png, pngToJpegToWebpLossy);

        System.out.printf("PNG to JPEG to WebP. " +
                "Distortion (PNGtoJPEG): %s; " +
                "Distortion (PNG): %s%n",
                pngToJpeg_pngToJpegToWebpLossy_psnr, png_pngToJpegToWebpLossy_psnr);

        //encodeWebp();
    }

    private static File encodeToJpeg(File input, String append, long quality) throws IOException {

        ImageWriter jpgWriter = ImageIO.getImageWritersByFormatName("jpg").next();
        ImageWriteParam jpgWriteParam = jpgWriter.getDefaultWriteParam();
        jpgWriteParam.setCompressionMode(ImageWriteParam.MODE_EXPLICIT);
        jpgWriteParam.setCompressionQuality(0.7f);

        ImageOutputStream outputStream = new FileImageOutputStream(input); // For example implementations see below
        jpgWriter.setOutput(outputStream);
        IIOImage outputImage = new IIOImage(toBufferedImage(ImageIO.read(input)), null, null);
        jpgWriter.write(null, outputImage, jpgWriteParam);
        jpgWriter.dispose();

        return
    }

    private static File encodeToWebp(File input, String append, boolean lossless) throws IOException {
        String inputJpgPath = "/home/jpmatos/Documents/SM/Trab2/kodak/result/kodim01_ToJpeg75.jpg";
        String outputWebpPath = "/home/jpmatos/Documents/SM/Trab2/kodak/result/kodim01_ToJpeg75ToWebp.webp";


        // Obtain an image to encode from somewhere
        BufferedImage image = ImageIO.read(new File(inputJpgPath));

        // Obtain a WebP ImageWriter instance
        ImageWriter writer = ImageIO.getImageWritersByMIMEType("image/webp").next();

        // Configure encoding parameters
        WebPWriteParam writeParam = new WebPWriteParam(writer.getLocale());
        writeParam.setCompressionMode(WebPWriteParam.LOSSLESS_COMPRESSION);

        // Configure the output on the ImageWriter
        writer.setOutput(new FileImageOutputStream(new File(outputWebpPath)));

        // Encode
        long st = System.currentTimeMillis();
        writer.write(null, new IIOImage(image, null, null), writeParam);
        System.out.println("cost: " + (System.currentTimeMillis() - st));

        return new File("");
    }

    public static BufferedImage toBufferedImage(Image img)
    {
        if (img instanceof BufferedImage)
        {
            return (BufferedImage) img;
        }

        // Create a buffered image with transparency
        BufferedImage bimage = new BufferedImage(img.getWidth(null), img.getHeight(null), BufferedImage.TYPE_INT_ARGB);

        // Draw the image on to the buffered image
        Graphics2D bGr = bimage.createGraphics();
        bGr.drawImage(img, 0, 0, null);
        bGr.dispose();

        // Return the buffered image
        return bimage;
    }
}
