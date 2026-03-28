using UnityEngine;
using FaceTrackerComponent;

public class FaceTrackerTester : MonoBehaviour
{
    private FaceTrackerManager _manager;

    private void Awake()
    {
        _manager = GetComponent<FaceTrackerManager>();
        _manager.OnFaceDetected += OnFace;
        _manager.OnFaceLost     += OnLost;
    }

    private void OnDestroy()
    {
        _manager.OnFaceDetected -= OnFace;
        _manager.OnFaceLost     -= OnLost;
    }

    private void OnFace(FaceData data)
    {
        Debug.Log($"[FaceTracker] Posición: {data.Position} | Gaze: {data.GazeDirection}");
    }

    private void OnLost(string id)
    {
        Debug.Log($"[FaceTracker] Cara perdida: {id}");
    }
}