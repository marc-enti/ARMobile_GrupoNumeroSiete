using UnityEngine;
using FaceTrackerComponent;

[RequireComponent(typeof(LineRenderer))]
public class GazeVisualizer : MonoBehaviour
{
    [SerializeField] private float    gazeLength = 0.5f;
    [SerializeField] private Material lineMaterial;

    private LineRenderer       _line;
    private FaceTrackerManager _manager;

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        _line.positionCount = 2;
        _line.startWidth    = 0.005f;
        _line.endWidth      = 0.002f;
        _line.useWorldSpace = true;
        _line.enabled       = false;
        _line.material      = lineMaterial;
    }

    private void OnEnable()
    {
        _manager = FindAnyObjectByType<FaceTrackerManager>();
        if (_manager == null) { Debug.LogWarning("[GazeVisualizer] No FaceTrackerManager found."); return; }

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
        _line.enabled = true;
        _line.SetPosition(0, data.Position);
        _line.SetPosition(1, data.Position + data.GazeDirection * gazeLength);
    }

    private void HandleFaceLost(string id) => _line.enabled = false;
}