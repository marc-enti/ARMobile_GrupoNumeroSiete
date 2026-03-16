using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using ZXing;

public class QrScanner : MonoBehaviour
{
    [SerializeField] private ARCameraManager cameraManager;
    [SerializeField] private float startDelay = 2f;

    private IBarcodeReader reader;
    private string lastValue = "";

    IEnumerator Start()
    {
        Debug.Log("[QrScanner] START");

        reader = new BarcodeReader
        {
            AutoRotate = true,
            Options = new ZXing.Common.DecodingOptions
            {
                TryHarder = true,
                PossibleFormats = new System.Collections.Generic.List<BarcodeFormat>
                {
                    BarcodeFormat.QR_CODE
                }
            }
        };

        Debug.Log("[QrScanner] Esperando ARCore...");
        yield return new WaitForSeconds(startDelay);

        cameraManager.frameReceived += OnCameraFrame;

        Debug.Log("[QrScanner] READY");
    }

    void OnCameraFrame(ARCameraFrameEventArgs args)
    {
        Debug.Log("[QrScanner] ---- FRAME ----");

        if (!cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            Debug.Log("[QrScanner] No CPU image");
            return;
        }

        int width = image.width;
        int height = image.height;

        Debug.Log("[QrScanner] Imagen: " + width + "x" + height);

        var conversion = new XRCpuImage.ConversionParams
        {
            inputRect = new RectInt(0, 0, width, height),
            outputDimensions = new Vector2Int(width, height),
            outputFormat = TextureFormat.RGBA32,
            transformation = XRCpuImage.Transformation.None
        };

        var buffer = new NativeArray<byte>(image.GetConvertedDataSize(conversion), Allocator.Temp);

        image.Convert(conversion, buffer);
        image.Dispose();

        Debug.Log("[QrScanner] Convert OK");

        byte[] raw = buffer.ToArray();
        buffer.Dispose();

        Debug.Log("[QrScanner] Bytes: " + raw.Length);

        Color32[] colors = new Color32[width * height];

        Debug.Log("[QrScanner] Convirtiendo a Color32...");

        for (int i = 0; i < colors.Length; i++)
        {
            int index = i * 4;

            colors[i] = new Color32(
                raw[index],
                raw[index + 1],
                raw[index + 2],
                raw[index + 3]
            );
        }

        Debug.Log("[QrScanner] Color32 convertido");

        Result result = null;

        try
        {
            Debug.Log("[QrScanner] Intentando Decode");
            result = reader.Decode(colors, width, height);
        }
        catch (System.Exception e)
        {
            Debug.LogError("[QrScanner] Decode ERROR: " + e.Message);
        }

        if (result == null)
        {
            Debug.Log("[QrScanner] No QR");
            return;
        }

        if (result.Text != lastValue)
        {
            lastValue = result.Text;
            Debug.Log("QR DETECTADO: " + result.Text);
        }
    }

    void OnDisable()
    {
        if (cameraManager != null)
            cameraManager.frameReceived -= OnCameraFrame;
    }
}