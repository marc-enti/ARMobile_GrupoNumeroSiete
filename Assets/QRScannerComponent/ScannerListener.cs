using UnityEngine;
using UnityEngine.Events;

namespace ScannerComponent
{
    public class ScannerListener : MonoBehaviour
    {
        [Tooltip("Añade aquí las funciones que quieres que se ejecuten cuando se lea un QR.")]
        public UnityEvent<string> onCodeScanned;

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

        private void HandleCodeDetected(DecodeResult qrText)
        {
            onCodeScanned?.Invoke(qrText.Text);
        }
    }
}
