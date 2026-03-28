using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace FaceTrackerComponent.XR
{
    /// <summary>
    /// Implementación concreta que lee datos del ARFaceManager de AR Foundation
    /// y los traduce al modelo FaceData propio.
    /// Solo hace mapeo, sin lógica de negocio.
    /// </summary>
    public class ARFaceTracker : MonoBehaviour, IFaceTracker
    {
        [Tooltip("Arrastra aquí el ARFaceManager de tu escena. " +
                 "Si se deja vacío, se busca automáticamente.")]
        [SerializeField] private ARFaceManager faceManager;

        public event Action<FaceData> OnFaceDetected;
        public event Action<string>   OnFaceLost;

        private readonly Dictionary<TrackableId, FaceData> _activeFaces = new();
        private bool _isTracking;

        // ── Lifecycle ────────────────────────────────────────────────────────────

        private void Start()
        {
            if (faceManager == null)
                faceManager = FindAnyObjectByType<ARFaceManager>();

            if (faceManager == null)
                Debug.LogError("[ARFaceTracker] No hay ARFaceManager en la escena.", this);
        }

        private void OnDestroy() => StopTracking();

        // ── IFaceTracker ─────────────────────────────────────────────────────────

        public void StartTracking()
        {
            if (_isTracking || faceManager == null) return;
            faceManager.facesChanged += OnFacesChanged;
            _isTracking = true;
        }

        public void StopTracking()
        {
            if (!_isTracking || faceManager == null) return;
            faceManager.facesChanged -= OnFacesChanged;
            _activeFaces.Clear();
            _isTracking = false;
        }

        public IReadOnlyList<FaceData> GetCurrentFaces() =>
            new List<FaceData>(_activeFaces.Values);

        // ── Lógica interna ───────────────────────────────────────────────────────

        private void OnFacesChanged(ARFacesChangedEventArgs args)
        {
            foreach (var face in args.added)   ProcessFace(face);
            foreach (var face in args.updated) ProcessFace(face);

            foreach (var face in args.removed)
            {
                _activeFaces.Remove(face.trackableId);
                OnFaceLost?.Invoke(face.trackableId.ToString());
            }
        }

        private void ProcessFace(ARFace face)
        {
            if (face == null) return;

            var t = face.transform;

            // GazeDirection: ARFoundation apunta 'forward' HACIA el usuario
            // (cámara frontal), así que lo invertimos para que el vector
            // indique hacia dónde "sale" la mirada desde los ojos.
            var data = new FaceData
            {
                TrackableId   = face.trackableId.ToString(),
                Position      = t.position,
                Rotation      = t.rotation,
                GazeDirection = -t.forward,
                IsTracking    = face.trackingState == TrackingState.Tracking
            };

            _activeFaces[face.trackableId] = data;

            if (data.IsTracking)
                OnFaceDetected?.Invoke(data);
        }
    }
}