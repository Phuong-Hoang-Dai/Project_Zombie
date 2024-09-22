using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

public class GenerateIcon : MonoBehaviour
{
    public Camera cam;
    public string Path;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            TakeScreenshot($"{Application.dataPath}/{Path}/Icon_1.png");
    }

    public void TakeScreenshot(string fullpath)
    {
        RenderTexture rt = new RenderTexture(256, 256, 24);
        cam.targetTexture = rt;
        cam.Render();
        RenderTexture.active = rt;

        Texture2D t2D = new Texture2D(256, 256, TextureFormat.RGBA32, false);
        t2D.ReadPixels(new Rect(0,0,256,256),0,0);

        cam.targetTexture = null;
        RenderTexture.active = null;

        byte[] data = t2D.EncodeToPNG();
        System.IO.File.WriteAllBytes(fullpath, data);

        AssetDatabase.Refresh();
    }
}
