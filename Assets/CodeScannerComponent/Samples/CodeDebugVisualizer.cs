using ScannerComponent;
using UnityEngine;

public class CodeDebugVisualizer : MonoBehaviour
{
    [Tooltip("The 3D prefab to instantiate over the scanned code.")]
    public GameObject qrPrefab;

    private GameObject instantiatedObject;

    public void VisualizeCodeLocation(DecodeResult result)
    {
        Debug.Log($"Code successfully scanned: {result.Text}");

        if (result.HasWorldTransform)
        {
            if (instantiatedObject != null)
            {
                instantiatedObject.transform.position = result.WorldPosition;

                instantiatedObject.transform.up = result.WorldNormal;
            }
            else
            {
                instantiatedObject = Instantiate(qrPrefab, result.WorldPosition, Quaternion.identity);
                instantiatedObject.transform.up = result.WorldNormal;
            }
        }
        else
        {
            Debug.LogWarning("Code read in 2D, but AR Foundation hasn't detected the physical surface yet.");
        }
    }
}