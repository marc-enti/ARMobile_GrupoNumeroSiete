using UnityEngine;

namespace ScannerComponent
{
    public class DecodeResult
    {
        public string Text;
        public Vector2[] ImagePoints;

        public Vector3 WorldPosition;
        public Vector3 WorldNormal;
        public bool HasWorldTransform;
    }

    public interface IImageReader
    {
        DecodeResult DecodeImage(ImageFrame imageFrame);     
    }
}
