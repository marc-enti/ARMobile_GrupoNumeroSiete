using System;
using UnityEngine;
using UnityEngine.Events;

// ← Sin "using FaceTrackerComponent.XR" aquí

namespace FaceTrackerComponent
{
    // ← Sin [RequireComponent(typeof(ARFaceTracker))]
    [AddComponentMenu("AR/Face Tracker/Face Tracker Manager")]
    public class FaceTrackerManager : MonoBehaviour
    {
        [Header("Configuración")]
        [SerializeField] private bool startOnAwake = true;

        [Header("Eventos (Inspector)")]
        public UnityEvent<FaceData> OnFaceDetectedUnity;
        public UnityEvent<string>   OnFaceLostUnity;

        public event Action<FaceData> OnFaceDetected;
        public event Action<string>   OnFaceLost;

        public IFaceTracker Tracker => _tracker;

        private IFaceTracker _tracker;
        private bool _isTracking;

        private void Awake()
        {
            // GetComponent busca cualquier cosa que implemente IFaceTracker
            // No le importa si es ARFaceTracker, un mock, o cualquier otra cosa
            _tracker = GetComponent<IFaceTracker>();

            if (_tracker == null)
            {
                Debug.LogError(
                    "[FaceTrackerManager] No se encontró ningún IFaceTracker " +
                    "en este GameObject. Añade ARFaceTracker al mismo objeto.", this);
                return;
            }

            _tracker.OnFaceDetected += HandleFaceDetected;
            _tracker.OnFaceLost     += HandleFaceLost;

            if (startOnAwake) StartTracking();
        }

        private void OnDestroy()
        {
            StopTracking();
            if (_tracker == null) return;
            _tracker.OnFaceDetected -= HandleFaceDetected;
            _tracker.OnFaceLost     -= HandleFaceLost;
        }

        public void StartTracking()
        {
            if (_isTracking || _tracker == null) return;
            _tracker.StartTracking();
            _isTracking = true;
        }

        public void StopTracking()
        {
            if (!_isTracking || _tracker == null) return;
            _tracker.StopTracking();
            _isTracking = false;
        }

        public void ToggleTracking()
        {
            if (_isTracking) StopTracking();
            else StartTracking();
        }

        private void HandleFaceDetected(FaceData data)
        {
            OnFaceDetected?.Invoke(data);
            OnFaceDetectedUnity?.Invoke(data);
        }

        private void HandleFaceLost(string id)
        {
            OnFaceLost?.Invoke(id);
            OnFaceLostUnity?.Invoke(id);
        }
    }
}