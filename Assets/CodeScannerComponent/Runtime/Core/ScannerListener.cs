using UnityEngine;
using UnityEngine.Events;

namespace ScannerComponent
{
    public class ScannerListener : MonoBehaviour
    {
        [Tooltip("Triggered when a code is scanned, returning its text and 3D position.")]
        public UnityEvent<DecodeResult> onCodeScanned;

        private void OnEnable()
        {
            if (ScannerManager.Instance != null)
            {
                ScannerManager.Instance.OnCodeDetected += HandleCodeDetected;
            }
        }

        private void OnDisable()
        {
            if (ScannerManager.Instance != null)
            {
                ScannerManager.Instance.OnCodeDetected -= HandleCodeDetected;
            }
        }

        private void HandleCodeDetected(DecodeResult result)
        {
            onCodeScanned?.Invoke(result);
        }
    }
}