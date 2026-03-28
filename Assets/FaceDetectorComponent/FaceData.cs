using UnityEngine;

namespace FaceTrackerComponent
{
    /// <summary>
    /// Modelo de datos que representa un rostro detectado.
    /// Es el objeto que viaja a través de todos los eventos del sistema.
    /// </summary>
    public class FaceData
    {
        public Vector3    Position;      // Centro del rostro en espacio mundo
        public Vector3    GazeDirection; // Hacia dónde miran los ojos (normalizado)
        public Quaternion Rotation;      // Rotación completa del rostro
        public string     TrackableId;   // ID único (soporta múltiples caras)
        public bool       IsTracking;    // false = cara detectada pero sin confianza
    }
}