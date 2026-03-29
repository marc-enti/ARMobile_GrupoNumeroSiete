using UnityEngine;
using System;

namespace ScannerComponent
{
    public class ScannerManager : MonoBehaviour
    {
        public static ScannerManager Instance { get; private set; }

        [Tooltip("Interval in seconds between each scan attempt.")]
        public float scanInterval = 0.5f;

        public event Action<DecodeResult> OnCodeDetected;

        private IImageProvider imageProvider;
        private IImageReader imageReader;

        private float timer = 0f;
        private bool isScanning = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            imageProvider = GetComponent<IImageProvider>();
            imageReader = GetComponent<IImageReader>();

            if (imageProvider == null || imageReader == null)
            {
                Debug.LogError("Missing IImageProvider or IImageReader in the ScannerManager. The component will be disabled.");
                enabled = false;
                return;
            }
        }

        private void Start()
        {
            if (enabled)
            {
                StartScanning();
            }
        }

        public void StartScanning() => isScanning = true;
        public void StopScanning() => isScanning = false;

        private void Update()
        {
            if (!isScanning) return;

            timer += Time.deltaTime;

            if (timer >= scanInterval)
            {
                timer = 0f;
                TryScan();
            }
        }

        private void TryScan()
        {
            if (!imageProvider.RequestImage(out ImageFrame frame))
                return;

            var readResult = imageReader.DecodeImage(frame);

            if (!readResult.Success)
                return;

            bool has3D = imageProvider.TryGetWorldTransform(readResult.ImagePoints, frame.width, frame.height, out Vector3 pos, out Vector3 norm);

            DecodeResult finalResult = new DecodeResult
            {
                Text = readResult.Text,
                HasWorldTransform = has3D,
                WorldPosition = pos,
                WorldNormal = norm
            };

            OnCodeDetected?.Invoke(finalResult);
        }
    }
}