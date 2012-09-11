using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

using AForge.Imaging.Filters;

namespace Fesb.Dip.ConnectedComponents
{
    public partial class ConnectedComponentsAlgorithm
    {
        public ApplyResult Apply(Bitmap bitmap, byte? backgroundTreshold = null)
        {
            var bitmapData = GetRawData(bitmap);
            var labelData = new byte[bitmapData.Length];

            var blobCount = _floodFillAlgorithm.Fill(bitmapData, bitmap.Size, backgroundTreshold, labelData);

            var labeledImage = new Bitmap(bitmap.Size.Width, bitmap.Size.Height, PixelFormat.Format8bppIndexed);
            labeledImage.Palette = _AdjustColorPallete(labeledImage.Palette, backgroundTreshold);
            SetRawData(labeledImage, labelData);

            return new ApplyResult { Bitmap = labeledImage, BlobCount = blobCount };
        }

        public class ApplyResult
        {
            public Bitmap Bitmap { get; set; }
            public int BlobCount { get; set; }
        }
    }

    public partial class ConnectedComponentsAlgorithm
    {
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

        private ColorPalette _AdjustColorPallete(ColorPalette palette, byte? backgroundTreshold)
        {
            for (var i = 0; i <= (backgroundTreshold ?? -1); i++)
            {
                palette.Entries[i] = Color.Black;
            }

            for (var i = (backgroundTreshold ?? -1) + 1; i < palette.Entries.Length; i++)
            {
                palette.Entries[i] = _colorTable[i % _colorTable.Length];
            }

            return palette;
        }
    }

    public partial class ConnectedComponentsAlgorithm
    {
        private readonly Point[] _connectivityPoints;
        private readonly FloodFillAlgorithm _floodFillAlgorithm;
        private readonly Color[] _colorTable;

        public ConnectedComponentsAlgorithm()
        {
            _connectivityPoints = new[]
            {
                new Point(-1, -1), new Point(0, -1), new Point(1, -1),
                new Point(-1,  0), /*new Point(0, 0),*/ new Point(1, 0),
                new Point(-1,  1), new Point(0,  1), new Point(1,  1),
            };

            _floodFillAlgorithm = new FloodFillAlgorithm(_connectivityPoints);

            _colorTable = new[]
            {
                Color.Red,		Color.Green,	Color.Blue,			Color.Yellow,
                Color.Violet,	Color.Brown,	Color.Olive,		Color.Cyan,

                Color.Magenta,	Color.Gold,		Color.Indigo,		Color.Ivory,
                Color.HotPink,	Color.DarkRed,	Color.DarkGreen,	Color.DarkBlue,

                Color.DarkSeaGreen,	Color.Gray,	Color.DarkKhaki,	Color.DarkGray,
                Color.LimeGreen, Color.Tomato,	Color.SteelBlue,	Color.SkyBlue,

                Color.Silver,	Color.Salmon,	Color.SaddleBrown,	Color.RosyBrown,
                Color.PowderBlue, Color.Plum,	Color.PapayaWhip,	Color.Orange
            };
        }
    }
}