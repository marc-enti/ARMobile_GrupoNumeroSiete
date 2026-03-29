using System;
using System.Collections.Generic;

namespace FaceTrackerComponent
{
    public interface IFaceTracker
    {
        event Action<FaceData> OnFaceDetected;
        event Action<string>   OnFaceLost;

        void StartTracking();
        void StopTracking();
        IReadOnlyList<FaceData> GetCurrentFaces();
    }
}