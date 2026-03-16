using System.Collections.Generic;
using UnityEngine;
using ZXing;

namespace QRScannerComponent
{
    public static class QRFunctions
    {
        private static IBarcodeReader reader = new BarcodeReader
        {
            AutoRotate = true,
            Options = new ZXing.Common.DecodingOptions
            {
                // Solo admetemos QR
                PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE }
            }
        };

        public static string DecodeQR(Color32[] pixels, int width, int height)
        {
            try
            {
                var result = reader.Decode(pixels, width, height);

                if (result != null)
                {
                    return result.Text;
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning("Error al decodificar QR: " + ex.Message);
            }

            return null;
        }
    }
}
