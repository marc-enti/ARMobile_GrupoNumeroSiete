using UnityEngine;

namespace ScannerComponent
{
    public struct ReadResult
    {
        public bool Success;
        public string Text;
        public Vector2[] ImagePoints;
    }
}