using ScannerComponent;
using UnityEngine;
using ZXing;

public class ZXingImageReader : MonoBehaviour, IImageReader
{
    private IBarcodeReader reader;

    private void Awake()
    {
        reader = new BarcodeReader
        {
            AutoRotate = true,
            Options = new ZXing.Common.DecodingOptions
            {
                TryHarder = true
            }
        };
    }

    public DecodeResult DecodeImage(Color32[] pixels, int width, int height)
    {
        try
        {
            var result = reader.Decode(pixels, width, height);
            if (result != null)
            {
                // Extraemos las coordenadas 2D donde ZXing vio el código
                Vector2[] points = new Vector2[result.ResultPoints.Length];
                for (int i = 0; i < points.Length; i++)
                {
                    points[i] = new Vector2(result.ResultPoints[i].X, result.ResultPoints[i].Y);
                }

                return new DecodeResult
                {
                    Text = result.Text,
                    ImagePoints = points
                };
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("Error al decodificar: " + ex.Message);
        }

        return null;
    }
}