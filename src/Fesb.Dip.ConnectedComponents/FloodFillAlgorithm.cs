using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Fesb.Dip.ConnectedComponents
{
    public partial class FloodFillAlgorithm
    {
        public int Fill(byte[] imageData, Size imageSize, byte? backgroundTreshold, byte[] labelData)
        {
            var blobsCount = 0;

            for (var y = 0; y < imageSize.Height; y++)
                for (var x = 0; x < imageSize.Width; x++)
                {
                    var currentPoint = new Point(x, y);
                    
                    var currentValue = _GetPixel(currentPoint, imageData, imageSize);
                    if (backgroundTreshold.HasValue && currentValue <= backgroundTreshold) { continue; }

                    var currentLabel = _GetPixel(currentPoint, labelData, imageSize);
                    if (currentLabel != 0) { continue; }

                    ++blobsCount;
                    var label = (byte)Math.Max(1, blobsCount % byte.MaxValue);
                    FillFrom(currentPoint, label, imageData, imageSize, labelData);
                }

            return blobsCount;
        }

        public void FillFrom(Point currentPoint, byte label, byte[] imageData, Size imageSize, byte[] labelData)
        {
            var nextPointsQueue = new Queue<Point>();
            nextPointsQueue.Enqueue(currentPoint);

            while (nextPointsQueue.Any())
            {
                currentPoint = nextPointsQueue.Dequeue();

                var currentLabel = _GetPixel(currentPoint, labelData, imageSize);
                if (currentLabel != 0) { continue; }

                _SetPixel(currentPoint, label, labelData, imageSize);

                var currentValue = _GetPixel(currentPoint, imageData, imageSize);

                var sameValueNeighborPoints =
                    _testPoints
                        .Select(tp => new Point(currentPoint.X + tp.X, currentPoint.Y + tp.Y))
                        .Where(p => _GetPixel(p, imageData, imageSize) != null) // Izbaci susjede van slike
                        .Where(p => _GetPixel(p, imageData, imageSize) == currentValue) // Izbaci elemente koji nisu iste boje kao trenutni
                        .Where(p => _GetPixel(p, labelData, imageSize) == 0) // Izbaci elemente koji vec imaju labelu
                        .ToList();

                sameValueNeighborPoints.ForEach(nextPointsQueue.Enqueue);
            }
        }
    }

    public partial class FloodFillAlgorithm
    {
        private static byte? _GetPixel(Point p, byte[] data, Size imageSize)
        {
            if (p.X < 0 || imageSize.Width <= p.X) { return null; }
            if (p.Y < 0 || imageSize.Height <= p.Y) { return null; }

            return data[p.Y * imageSize.Width + p.X];
        }

        private static void _SetPixel(Point p, byte value, byte[] data, Size imageSize)
        {
            data[p.Y * imageSize.Width + p.X] = value;
        }
    }

    public partial class FloodFillAlgorithm
    {
        private readonly Point[] _testPoints;

        public FloodFillAlgorithm(Point[] testPoints)
        {
            _testPoints = testPoints;
        }
    }
}