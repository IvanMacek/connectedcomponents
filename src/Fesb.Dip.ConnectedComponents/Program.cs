using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using AForge.Imaging.Filters;

using Image = AForge.Imaging.Image;

namespace Fesb.Dip.ConnectedComponents
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var inputImagePath = @"C:\Users\Ivan\Desktop\lena.jpg";
            var inputImage = Image.FromFile(inputImagePath);

            var workingDir = Path.GetDirectoryName(inputImagePath) ?? Directory.GetCurrentDirectory();
            var fileName = Path.GetFileName(inputImagePath);

            // Generate grayscale
            var grayscaleImage = Grayscale.CommonAlgorithms.BT709.Apply(inputImage);
            var grayscaleData = GetRawData(grayscaleImage);

            var grayscaleImagePath = Path.Combine(workingDir, "gray." + fileName);
            grayscaleImage.Save(grayscaleImagePath, ImageFormat.Jpeg);


            // Generate monochrome
            var monochromeImage = Image.Clone(grayscaleImage);

            var blackTreshold = 125;
            var monochromeData = ToMonochrome(grayscaleData, blackTreshold);
            SetRawData(monochromeImage, monochromeData);

            var monochromeImagePath = Path.Combine(workingDir, "mono." + fileName);
            monochromeImage.Save(monochromeImagePath, ImageFormat.Jpeg);


            // Generate labeled components
            var outputImagePath = Path.Combine(workingDir, "out." + fileName);


        }


        public static byte[] GetRawData(Bitmap bitmap)
        {
            var bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var dataLength = Math.Abs(bmpData.Stride) * bitmap.Height;
            var data = new byte[dataLength];

            Marshal.Copy(bmpData.Scan0, data, 0, dataLength);

            bitmap.UnlockBits(bmpData);

            return data;
        }

        public static void SetRawData(Bitmap bitmap, byte[] data)
        {
            var bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            Marshal.Copy(data, 0, bmpData.Scan0, data.Length);

            bitmap.UnlockBits(bmpData);
        }

        public static byte[] ToMonochrome(byte[] grayscaleData, int blackTreshold)
        {
            return grayscaleData.Select(b => (b < blackTreshold) ? byte.MinValue : byte.MaxValue).ToArray();
        }
    }
}
