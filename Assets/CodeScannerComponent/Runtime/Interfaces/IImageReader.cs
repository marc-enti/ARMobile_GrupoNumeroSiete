using UnityEngine;

namespace ScannerComponent
{
    public interface IImageReader
    {
        ReadResult DecodeImage(ImageFrame imageFrame);     
    }
}
