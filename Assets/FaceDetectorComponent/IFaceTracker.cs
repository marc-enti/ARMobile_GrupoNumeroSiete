using System;
using System.Collections.Generic;

namespace FaceTrackerComponent
{
    /// <summary>
    /// Contrato que define cómo se obtienen datos de detección facial.
    /// Cualquier fuente (ARFoundation, webcam, mock de testing) debe cumplirlo.
    /// </summary>
    public interface IFaceTracker
    {
        event Action<FaceData> OnFaceDetected;
        event Action<string>   OnFaceLost;      // pasa el TrackableId

        void StartTracking();
        void StopTracking();

        IReadOnlyList<FaceData> GetCurrentFaces();
    }
}