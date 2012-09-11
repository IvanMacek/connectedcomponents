using System;
using System.Drawing.Imaging;
using System.IO;

using AForge.Imaging.Filters;

using Image = AForge.Imaging.Image;

namespace Fesb.Dip.ConnectedComponents
{
    public class Program
    {
        public static int Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Parameter missing: Path to picture");
                return 1;
            }

            var inputImagePath = args[0];
            var blackTreshold = (args.Length >= 2) ? byte.Parse(args[1]) : (byte.MaxValue / 2);
            var backgroundTreshold = (args.Length >= 3) ? byte.Parse(args[2]) : (byte?)null;

            var inputImage = Image.FromFile(inputImagePath);
            var workingDir = Path.GetDirectoryName(inputImagePath) ?? Directory.GetCurrentDirectory();
            var fileName = Path.GetFileNameWithoutExtension(inputImagePath);
            
            // Generate grayscale
            var grayscaleImage = Grayscale.CommonAlgorithms.BT709.Apply(inputImage);
            grayscaleImage.Save(Path.Combine(workingDir, string.Format("{0}.{1}.png", fileName, "gray")), ImageFormat.Png);
            
            // Generate monochrome
            var monochromeImage = new Threshold(blackTreshold).Apply(grayscaleImage);
            monochromeImage.Save(Path.Combine(workingDir, string.Format("{0}.{1}.{2}.png", fileName, "mono", blackTreshold)), ImageFormat.Png);
            
            // Generate labeled image (using custom algorithm)
            var result = new ConnectedComponentsAlgorithm().Apply(monochromeImage, backgroundTreshold);
            result.Bitmap.Save(Path.Combine(workingDir, string.Format("{0}.{1}.{2}.png", fileName, "blobs", blackTreshold)), ImageFormat.Png);
            Console.WriteLine("My algorithm counted {0} blobs.", result.BlobCount);

            return 0;
        }
    }
}
