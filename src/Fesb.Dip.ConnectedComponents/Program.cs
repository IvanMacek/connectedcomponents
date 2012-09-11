using System;
using System.Drawing.Imaging;
using System.IO;

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
            var fileName = Path.GetFileNameWithoutExtension(inputImagePath);

            // Generate grayscale
            var grayscaleImage = Grayscale.CommonAlgorithms.BT709.Apply(inputImage);
            grayscaleImage.Save(Path.Combine(workingDir, "gray." + fileName + ".png"), ImageFormat.Png);

            // Generate monochrome
            var blackTreshold = 225;
            var monochromeImage = new Threshold(blackTreshold).Apply(grayscaleImage);
            monochromeImage.Save(Path.Combine(workingDir, "mono." + fileName + ".png"), ImageFormat.Png);

            // Generate labeled image (using custom algorithm)
            var result = new ConnectedComponentsAlgorithm().Apply(monochromeImage);
            result.Bitmap.Save(Path.Combine(workingDir, "my-labeled." + fileName + ".png"), ImageFormat.Png);
            Console.WriteLine("My algorithm counted {0} blobs.", result.BlobCount);

            // Generate labeled image (using aforge algorithm)
            var aforgeAlgorithm = new ConnectedComponentsLabeling();
            var aforgeLabeledImage = aforgeAlgorithm.Apply(monochromeImage);
            aforgeLabeledImage.Save(Path.Combine(workingDir, "aforge-labeled." + fileName + ".png"), ImageFormat.Png);
            Console.WriteLine("AForge algorithm counted {0} blobs.", aforgeAlgorithm.ObjectCount);
        }
    }
}
