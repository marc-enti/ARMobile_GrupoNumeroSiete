using ScannerComponent;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

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
    public bool TryGetWorldTransform(Vector2[] imagePoints, int imageWidth, int imageHeight, out Vector3 position, out Vector3 normal)
    {
        position = Vector3.zero;
        normal = Vector3.up;

        //if (imagePoints == null || imagePoints.Length == 0 || raycastManager == null) return false;

        //// 1. Calculamos el centro exacto del código en la imagen 2D
        //Vector2 centerPoint = Vector2.zero;
        //foreach (var p in imagePoints)
        //{
        //    centerPoint += p;
        //}
        //centerPoint /= imagePoints.Length;

        //// 2. Convertimos ese punto de la imagen a un punto en la pantalla del móvil
        //// (La imagen suele estar rotada 90 grados en móviles, así que invertimos X e Y de forma básica)
        //float xRatio = centerPoint.x / imageWidth;
        //float yRatio = centerPoint.y / imageHeight;
        //Vector2 screenPoint = new Vector2(xRatio * Screen.width, yRatio * Screen.height);

        //// 3. Disparamos el Raycast de AR contra los planos detectados (el suelo, la mesa, etc.)
        //List<ARRaycastHit> hits = new List<ARRaycastHit>();
        //if (raycastManager.Raycast(screenPoint, hits, TrackableType.PlaneWithinPolygon))
        //{
        //    // ¡Golpeó un plano físico! Sacamos la posición y la normal (hacia dónde mira)
        //    position = hits[0].pose.position;
        //    normal = hits[0].pose.up;
        //    return true;
        //}

        return false;
    }
}