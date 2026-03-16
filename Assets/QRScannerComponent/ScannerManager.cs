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
        public event Action<string> OnQRDetected;

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
            // El Manager orquesta, pero no sabe CÓMO se saca la foto ni CÓMO se lee
            if (imageProvider.RequestImage(out ImageFrame imageFrame))
            {
                string result = imageReader.DecodeImage(imageFrame);

                if (!string.IsNullOrEmpty(result))
                {
                    OnQRDetected?.Invoke(result);
                }
            }
        }
    }
}
