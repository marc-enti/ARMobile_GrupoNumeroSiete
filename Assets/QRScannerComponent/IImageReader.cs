using UnityEngine;

namespace ScannerComponent
{
    public class DecodeResult
    {
        public string Text;
        public Vector2[] ImagePoints;
    }

    public interface IImageReader
    {
        //DecodeResult DecodeImage(ImageFrame imageFrame);
        string DecodeImage(ImageFrame imageFrame);
    }
}
