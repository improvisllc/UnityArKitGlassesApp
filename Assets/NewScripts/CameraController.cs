using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    RenderTexture myRenderTexture;
    void Start()
    {
        m_screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        /*
        myRenderTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);
        Camera.main.targetTexture = myRenderTexture;*/

        //GameObject.Find("DebugRawImage").GetComponent<RawImage>().texture = GetCameraRT(Camera.main);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static RenderTexture GetCameraRT(Camera cam)
    {
        var rt = RenderTexture.GetTemporary(Screen.width, Screen.height);
        cam.targetTexture = rt;
        cam.Render();
        cam.targetTexture = null;
        return rt;
    }

    public Material m_mattt;
    public Texture m_screenShot;
    private void OnPostRender()
    {
        //m_screenShot = GetCameraRT(Camera.main);
        //GameObject.Find("DebugRawImage").GetComponent<RawImage>().texture = m_screenShot;
        /*
        RenderTexture.active = Camera.main.targetTexture;
        m_screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        GameObject.Find("DebugRawImage").GetComponent<RawImage>().texture = m_screenShot;*/
        /*
        Camera.main.targetTexture = null; //null means framebuffer
        Graphics.Blit(myRenderTexture, null as RenderTexture, m_mattt);
        RenderTexture.ReleaseTemporary(myRenderTexture);
        GameObject.Find("DebugRawImage").GetComponent<RawImage>().material = m_mattt; 
        /*

        RenderTexture.active = Camera.main.targetTexture;
        m_screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        GameObject.Find("DebugRawImage").GetComponent<RawImage>().texture = m_screenShot;*/
    }
}
