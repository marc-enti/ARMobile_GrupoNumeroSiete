using UnityEngine;
using UnityEngine.Events;

namespace FaceTrackerComponent
{
    /// <summary>
    /// Añade este script a cualquier GameObject para recibir datos de cara
    /// sin escribir código. Se auto-conecta al FaceTrackerManager de la escena.
    ///
    /// Es equivalente al ScannerListener de tu componente de QR.
    /// </summary>
    [AddComponentMenu("AR/Face Tracker/Face Tracker Listener")]
    public class FaceTrackerListener : MonoBehaviour
    {
        [Header("Datos por separado (cómodo para el Inspector)")]
        public UnityEvent<Vector3> OnPositionReceived;
        public UnityEvent<Vector3> OnGazeDirectionReceived;

        [Header("Dato completo (para scripts avanzados)")]
        public UnityEvent<FaceData> OnFaceDataReceived;
        public UnityEvent<string>   OnFaceLost;

        private FaceTrackerManager _manager;

        private void OnEnable()
        {
            _manager = FindAnyObjectByType<FaceTrackerManager>();

            if (_manager == null)
            {
                Debug.LogWarning("[FaceTrackerListener] No hay FaceTrackerManager en escena.", this);
                return;
            }

            _manager.OnFaceDetected += HandleFaceDetected;
            _manager.OnFaceLost     += HandleFaceLost;
        }

        private void OnDisable()
        {
            if (_manager == null) return;
            _manager.OnFaceDetected -= HandleFaceDetected;
            _manager.OnFaceLost     -= HandleFaceLost;
        }

        private void HandleFaceDetected(FaceData data)
        {
            OnPositionReceived?.Invoke(data.Position);
            OnGazeDirectionReceived?.Invoke(data.GazeDirection);
            OnFaceDataReceived?.Invoke(data);
        }

        private void HandleFaceLost(string id) => OnFaceLost?.Invoke(id);
    }
}