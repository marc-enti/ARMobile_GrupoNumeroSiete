using UnityEngine;
using System;

namespace ScannerComponent
{
    public struct ImageFrame
    {
        public Color32[] pixels;
        public int width;
        public int height;

        public bool IsValid
        {
            get
            {
                return pixels != null &&
                       pixels.Length > 0 &&
                       width > 0 &&
                       height > 0;
            }
        }
    }

    public class ScannerManager : MonoBehaviour
    {
        public static ScannerManager Instance { get; private set; }

        public float scanInterval = 0.5f;

        // Nuestro evento C#
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
        }

        private void Start()
        {
            // Buscamos las interfaces en este mismo GameObject
            imageProvider = GetComponent<IImageProvider>();
            imageReader = GetComponent<IImageReader>();

            if (imageProvider == null || imageReader == null)
            {
                Debug.LogError("Falta un IImageProvider o un IImageReader en el QRScannerManager.");
                return;
            }

            StartScanning();
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
            if (imageProvider.RequestImage(out ImageFrame imageFrame))
            {
                DecodeResult result = imageReader.DecodeImage(imageFrame);

                if (result != null)
                {
                    if (result.ImagePoints != null && result.ImagePoints.Length > 0)
                    {
                        if (imageProvider.TryGetWorldTransform(
                            result.ImagePoints,
                            imageFrame.width,
                            imageFrame.height,
                            out Vector3 worldPosition,
                            out Vector3 worldNormal))
                        {
                            result.WorldPosition = worldPosition;
                            result.WorldNormal = worldNormal;
                            result.HasWorldTransform = true;
                        }
                    }

                    OnCodeDetected?.Invoke(result);
                }
            }
        }
    }
}
