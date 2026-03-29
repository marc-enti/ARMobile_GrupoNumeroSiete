using UnityEngine;

namespace ScannerComponent
{
    public interface ISpatialMapper
    {
        bool TryGet3DInfo(Vector2 screenPosition, out Vector3 position, out Vector3 normal);
    }
}