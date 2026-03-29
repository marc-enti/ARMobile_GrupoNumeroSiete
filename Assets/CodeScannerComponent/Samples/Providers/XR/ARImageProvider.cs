using System.Collections.Generic;
using ScannerComponent;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARImageProvider : MonoBehaviour, IImageProvider
{
    [Tooltip("Assign the AR Camera Manager here.")]
    public ARCameraManager cameraManager;

    [Tooltip("Assign the AR Raycast Manager here.")]
    public ARRaycastManager raycastManager;

    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    public bool RequestImage(out ImageFrame imageFrame)
    {
        imageFrame.pixels = null;
        imageFrame.width = 0;
        imageFrame.height = 0;

        if (cameraManager == null || !cameraManager.TryAcquireLatestCpuImage(out XRCpuImage image))
        {
            return false;
        }

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
        image.Dispose();

        imageFrame.pixels = buffer.Reinterpret<Color32>(1).ToArray();
        buffer.Dispose();

        return true;
    }

    public bool TryGetWorldTransform(Vector2[] imagePoints, int imageWidth, int imageHeight, out Vector3 position, out Vector3 normal)
    {
        position = Vector3.zero;
        normal = Vector3.up;

        if (imagePoints == null || imagePoints.Length == 0 || raycastManager == null) return false;

        Vector2 centerPoint = Vector2.zero;
        foreach (var p in imagePoints)
        {
            centerPoint += p;
        }
        centerPoint /= imagePoints.Length;

        float xRatio = centerPoint.x / imageWidth;
        float yRatio = centerPoint.y / imageHeight;
        Vector2 screenPoint = new Vector2(xRatio * Screen.width, yRatio * Screen.height);

        UnityEngine.XR.ARSubsystems.TrackableType trackableTypes =
            UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon;

        if (raycastManager.Raycast(screenPoint, hits, trackableTypes))
        {
            position = hits[0].pose.position;
            normal = hits[0].pose.up;
            return true;
        }

        Vector2 centerScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);
        if (raycastManager.Raycast(centerScreen, hits, trackableTypes))
        {
            position = hits[0].pose.position;
            normal = hits[0].pose.up;
            return true;
        }

        return false;
    }
}