using UnityEngine;

namespace ScannerComponent
{
    public interface IImageProvider
    {
        bool RequestImage(out ImageFrame imageFrame);

        //bool TryGetWorldTransform(Vector2[] imagePoints, int imageWidth, int imageHeight, out Vector3 position, out Vector3 normal);
    }
}

