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

    public ReadResult DecodeImage(ImageFrame imageFrame)
    {
        try
        {
            var result = reader.Decode(imageFrame.pixels, imageFrame.width, imageFrame.height);
            if (result != null)
            {
                Vector2[] points = new Vector2[result.ResultPoints.Length];
                for (int i = 0; i < points.Length; i++)
                {
                    points[i] = new Vector2(result.ResultPoints[i].X, result.ResultPoints[i].Y);
                }

                return new ReadResult
                {
                    Success = true,
                    Text = result.Text,
                    ImagePoints = points
                };
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning("Decoding error: " + ex.Message);
        }

        return new ReadResult { Success = false };
    }
}