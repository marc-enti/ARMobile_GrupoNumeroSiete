using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace FaceTrackerComponent.XR
{
    public class ARFaceTracker : MonoBehaviour, IFaceTracker
    {
        [SerializeField] private ARFaceManager faceManager;

        public event Action<FaceData> OnFaceDetected;
        public event Action<string>   OnFaceLost;

        private readonly Dictionary<TrackableId, FaceData> _activeFaces = new();
        private bool _isTracking;

        private void Start()
        {
            if (faceManager == null)
                faceManager = FindAnyObjectByType<ARFaceManager>();

            if (faceManager == null)
                Debug.LogError("[ARFaceTracker] No ARFaceManager found.", this);
        }

        private void OnDestroy() => StopTracking();

        public void StartTracking() => _isTracking = true;

        public void StopTracking()
        {
            _isTracking = false;
            _activeFaces.Clear();
        }

        public IReadOnlyList<FaceData> GetCurrentFaces() => new List<FaceData>(_activeFaces.Values);

        private void Update()
        {
            if (!_isTracking || faceManager == null) return;

            var currentIds = new HashSet<TrackableId>();

            foreach (var face in faceManager.trackables)
            {
                currentIds.Add(face.trackableId);

                if (face.trackingState != TrackingState.Tracking) continue;

                var data = new FaceData
                {
                    Position         = face.transform.position,
                    GazeDirection    = -face.transform.forward,
                    IsTracking       = true
                };

                _activeFaces[face.trackableId] = data;
                OnFaceDetected?.Invoke(data);
            }

            foreach (var id in new List<TrackableId>(_activeFaces.Keys))
            {
                if (!currentIds.Contains(id))
                {
                    _activeFaces.Remove(id);
                    OnFaceLost?.Invoke(id.ToString());
                }
            }
        }
    }
}