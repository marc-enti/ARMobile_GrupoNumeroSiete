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
            // Patrón Singleton
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
            // Solo iniciamos si el script no fue desactivado en el Awake
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
            if (imageProvider.RequestImage(out ImageFrame imageFrame) && imageFrame.IsValid)
            {
                ReadResult readResult = imageReader.DecodeImage(imageFrame);

                if (readResult.Success)
                {
                    DecodeResult finalResult = new DecodeResult
                    {
                        Text = readResult.Text,
                        HasWorldTransform = false
                    };

                    if (readResult.ImagePoints != null && readResult.ImagePoints.Length > 0)
                    {
                        if (imageProvider.TryGetWorldTransform(
                            readResult.ImagePoints,
                            imageFrame.width,
                            imageFrame.height,
                            out Vector3 worldPosition,
                            out Vector3 worldNormal))
                        {
                            finalResult.WorldPosition = worldPosition;
                            finalResult.WorldNormal = worldNormal;
                            finalResult.HasWorldTransform = true;
                        }
                    }

                    OnCodeDetected?.Invoke(finalResult);
                }
            }
        }
    }
}