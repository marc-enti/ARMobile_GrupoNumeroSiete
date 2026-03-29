using UnityEngine;

namespace ScannerComponent
{
    public struct ImageFrame
    {
        public Color32[] pixels;
        public int width;
        public int height;

        public bool IsValid => pixels != null &&
                               pixels.Length > 0 &&
                               width > 0 &&
                               height > 0;
    }
}