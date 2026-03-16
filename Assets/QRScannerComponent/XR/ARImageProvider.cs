using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Unity.Collections;
using ScannerComponent;

public class ARImageProvider : MonoBehaviour, IImageProvider
{
    [Tooltip("Arrastra aquí la Main Camera de AR Foundation")]
    public ARCameraManager cameraManager;

    public bool RequestImage(out ImageFrame imageFrame)
    {
        // Valores por defecto por si falla
        imageFrame.pixels = null;
        imageFrame.width = 0;
        imageFrame.height = 0;

        // Si no hay cámara o no puede sacar el frame, devolvemos false
        if (cameraManager == null || !cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            return false;
        }

        // Reducimos la resolución a la mitad para mejorar el rendimiento
        var conversionParams = new XRCpuImage.ConversionParams
        {
            inputRect = new RectInt(0, 0, image.width, image.height),
            outputDimensions = new Vector2Int(image.width / 2, image.height / 2),
            outputFormat = TextureFormat.RGBA32,
            transformation = XRCpuImage.Transformation.None
        };

        imageFrame.width = conversionParams.outputDimensions.x;
        imageFrame.height = conversionParams.outputDimensions.y;

        int size = image.GetConvertedDataSize(conversionParams);
        var buffer = new NativeArray<byte>(size, Allocator.Temp);

        image.Convert(conversionParams, buffer);
        image.Dispose(); // ¡Crítico para no llenar la RAM!

        imageFrame.pixels = buffer.Reinterpret<Color32>(1).ToArray();
        buffer.Dispose();

        // Devolvemos true porque conseguimos extraer los píxeles con éxito
        return true;
    }
}