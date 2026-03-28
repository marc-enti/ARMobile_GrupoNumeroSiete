using UnityEngine;
using FaceTrackerComponent;

/// <summary>
/// Dibuja una línea en el espacio 3D desde la posición de la cara
/// en la dirección del GazeDirection. Solo para depuración.
/// </summary>
[RequireComponent(typeof(LineRenderer))]
public class GazeVisualizer : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Longitud de la línea de gaze en metros.")]
    [SerializeField] private float gazeLength = 0.5f;

    [Tooltip("Color de la línea.")]
    [SerializeField] private Color gazeColor = Color.cyan;

    private LineRenderer _line;
    private FaceTrackerManager _manager;
    private bool _isTracking;

    // ── Lifecycle ────────────────────────────────────────────────────────────

    private void Awake()
    {
        _line = GetComponent<LineRenderer>();
        ConfigureLineRenderer();
    }

    private void OnEnable()
    {
        _manager = FindAnyObjectByType<FaceTrackerManager>();

        if (_manager == null)
        {
            Debug.LogWarning("[GazeVisualizer] No se encontró FaceTrackerManager.", this);
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

    // ── Handlers ─────────────────────────────────────────────────────────────

    private void HandleFaceDetected(FaceData data)
    {
        _isTracking = true;
        _line.enabled = true;

        // Punto de inicio: centro de la cara
        Vector3 origin = data.Position;

        // Punto final: desplazarse en la dirección de gaze
        Vector3 end = origin + data.GazeDirection * gazeLength;

        _line.SetPosition(0, origin);
        _line.SetPosition(1, end);
    }

    private void HandleFaceLost(string trackableId)
    {
        _isTracking = false;
        _line.enabled = false;
    }

    // ── Setup ─────────────────────────────────────────────────────────────────

    private void ConfigureLineRenderer()
    {
        _line.positionCount = 2;
        _line.startWidth    = 0.005f;   // 5mm — fino pero visible
        _line.endWidth      = 0.002f;   // se estrecha hacia el extremo
        _line.useWorldSpace = true;     // las posiciones son en espacio mundo
        _line.enabled       = false;    // oculto hasta que se detecte una cara

        // Material sin iluminación para que se vea igual en cualquier condición
        _line.material = new Material(Shader.Find("Sprites/Default"));
        _line.startColor = gazeColor;
        _line.endColor   = gazeColor;
    }
}