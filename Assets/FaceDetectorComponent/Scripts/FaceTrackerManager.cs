using System;
using UnityEngine;

namespace FaceTrackerComponent
{
    [AddComponentMenu("AR/Face Tracker/Face Tracker Manager")]
    public class FaceTrackerManager : MonoBehaviour
    {
        [SerializeField] private bool startOnAwake = true;

        public event Action<FaceData> OnFaceDetected;
        public event Action<string>   OnFaceLost;

        public IFaceTracker Tracker { get; private set; }

        private void Awake()
        {
            Tracker = GetComponent<IFaceTracker>();
            Tracker.OnFaceDetected += data => OnFaceDetected?.Invoke(data);
            Tracker.OnFaceLost     += id   => OnFaceLost?.Invoke(id);

            if (startOnAwake) Tracker.StartTracking();
        }

        private void OnDestroy() => Tracker.StopTracking();

        public void StartTracking() => Tracker.StartTracking();
        public void StopTracking()  => Tracker.StopTracking();
    }
}