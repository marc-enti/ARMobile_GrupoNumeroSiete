using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;
using UnityEngine.Rendering;

namespace QRScannerComponent
{
    public class ScannerManager : MonoBehaviour, IImageProvider, IImageReader
    {
        private static ScannerManager _instance;

        private UnityEvent<string> OnQRDetected;

        private float timer = 0f;
        private bool isScanning = false;
        private float scanInterval = 0.5f;

        public static ScannerManager Instance
        {
            get { return _instance; }   
        }

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            StartScanning();
        }

        public void StartScanning()
        {
            isScanning = true;
        }

        public void StopScanning()
        {
            isScanning = false;
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer >= scanInterval)
            {
                timer = 0f;
                TryScanCurrentFrame();
            }
        }

        private void TryScanCurrentFrame()
        {
            

            //buffer.Dispose();

            //string qrResult = QRFunctions.DecodeQR(pixels, conversionParams.outputDimensions.x, conversionParams.outputDimensions.y);

            //if (!string.IsNullOrEmpty(qrResult))
            //{
            //    Debug.Log("<color=green>¡QR Encontrado!</color> Contenido: " + qrResult);
            //    OnQRDetected.Invoke(qrResult);
            //}
        }

        public Color32[] RequestImage()
        {
            //if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
            //{
            //    return;
            //}

            //Hacemos la imagen mas pequeña para mejorar rendimiento del procesador del mobil
            //var conversionParams = new XRCpuImage.ConversionParams
            //{
            //    inputRect = new RectInt(0, 0, image.width, image.height),
            //    outputDimensions = new Vector2Int(image.width / 2, image.height / 2),
            //    outputFormat = TextureFormat.RGBA32,
            //    transformation = XRCpuImage.Transformation.None
            //};

            //int size = image.GetConvertedDataSize(conversionParams);
            //var buffer = new NativeArray<byte>(size, Allocator.Temp);

            //image.Convert(conversionParams, buffer);

            //image.Dispose(); //evitar memory leaks

            //Color32[] pixels = buffer.Reinterpret<Color32>(1).ToArray();

            //return pixels;
            return null;
        }

        public string DecodeImage(Color32[] pixels, int width, int height)
        {
            //try
            //{
            //    var result = reader.Decode(pixels, width, height);

            //    if (result != null)
            //    {
            //        return result.Text;
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    Debug.LogWarning("Error al decodificar QR: " + ex.Message);
            //}

            return null;
        }

    }
}
