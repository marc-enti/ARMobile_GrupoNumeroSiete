using UnityEngine;

namespace ScannerComponent
{
    [System.Serializable]
    public struct DecodeResult
    {
        public string Text;
        public Vector3 WorldPosition;
        public Vector3 WorldNormal;
        public bool HasWorldTransform;
    }
}