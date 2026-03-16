using UnityEngine;

public interface IImageReader
{
    public string DecodeImage(Color32[] pixels, int width, int height);
}
