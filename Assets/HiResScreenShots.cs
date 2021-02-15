﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HiResScreenShots : MonoBehaviour
{
    public int resWidth = 2550;
    public int resHeight = 3300;


    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screenshots/screen_{1}x{2}_{3}.png",
                             Application.dataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void takeScreenshot()
    {
        print("sdkjhdskfjhsdkjfh Garik");
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        Camera.main.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Camera.main.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        Camera.main.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        GameObject.Find("DebugRawImage").GetComponent<RawImage>().texture = screenShot;
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = ScreenShotName(resWidth, resHeight);
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));

    }

    void LateUpdate()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            takeScreenshot();
        }
    }
}