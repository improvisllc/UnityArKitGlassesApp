using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    GlassesManager m_glassesManager = new GlassesManager();
    MainController m_mainController = new MainController();


    public GameObject m_carouselBrandsNew;
    //GCarouselController m_brandsCarouselController;

    public GameObject m_carouselModelsNew;
    //GCarouselController m_modelsCarouselController;


    GameObject m_glassesModelTextGameobject;
    void Awake()
    {
        m_glassesManager = GameObject.Find("_manager").GetComponent<GlassesManager>();
        m_mainController = GameObject.Find("_manager").GetComponent<MainController>();
        m_glassesModelTextGameobject = GameObject.Find("GlassesModelText");

        //m_brandsCarouselController = m_carouselBrandsNew.transform.Find("_carouselManager").GetComponent<GCarouselController>();

        //m_modelsCarouselController = m_carouselModelsNew.transform.Find("_carouselManager").GetComponent<GCarouselController>();

    }

    public void onBrandOrModelButtonClicked(PointerEventData data)
    {
        string d = data.pointerClick.name;
        if(d.Contains("glasses_"))
        {
            print("Garik: Model Name: " + d);
            onModelButtonClicked(data);
            return;
        }
        print("Garik: Brand Name: " + d);
        m_glassesManager.showModels(d);


    }


    public void addBrandToCarousel(string name, Texture2D tex)
    {
        m_carouselBrandsNew.transform.Find("_carouselManager").GetComponent<GCarouselController>().SetupCell(name, tex);
    }


    public void onModelButtonClicked(PointerEventData data)
    {
        string modelName = data.pointerClick.name;
        print("Garik: MODEL Name: " + modelName);
        StartCoroutine(m_glassesManager.loadGlasses(modelName));
    }

    public void addModelToCarousel(string name, Texture2D tex)
    {
        m_carouselModelsNew.transform.Find("_carouselManager").GetComponent<GCarouselController>().SetupCell(name, tex);
    }

    public void startRecord()
    {
        //ReplayKitBridge.StartRecording();
        if (!ReplayKitBridge.IsScreenRecorderAvailable || ReplayKitBridge.IsRecording)
        {
            return;
        }

        // Set up delegates
        ReplayKitBridge.Instance.onStartRecordingCallback = OnStartRecording;
        ReplayKitBridge.Instance.onCancelRecordingCallback = OnCancelRecording;
        ReplayKitBridge.Instance.onStopRecordingCallback = OnStopRecording;
        ReplayKitBridge.Instance.onStopRecordingWithErrorCallback = OnStopRecordingWithError;
        ReplayKitBridge.Instance.onFinishPreviewCallback = OnFinishPreview;

        // Enable camera and microphone
        ReplayKitBridge.IsCameraEnabled = true;
        ReplayKitBridge.IsMicrophoneEnabled = true;

        // And then start recording
        ReplayKitBridge.StartRecording();

    }

    public void OnStartRecording()
    {
        Debug.Log("OnStartRecording");
    }

    public void OnCancelRecording()
    {
        Debug.Log("OnCancelRecording");
    }

    public void OnStopRecording()
    {
        Debug.Log("OnStopRecording");

        Time.timeScale = 0;
        ReplayKitBridge.PresentPreviewView();
    }

    public void OnStopRecordingWithError(string error)
    {
        Debug.Log("OnStopRecordingWithError error=" + error);
    }

    public void OnFinishPreview(string activityType)
    {
        Debug.Log("OnFinishPreview activityType=" + activityType);

        ReplayKitBridge.DismissPreviewView();
        Time.timeScale = 1;
    }

    public void stopRecord()
    {
        ReplayKitBridge.StopRecording();
    }


    void Update()
    {


    }

    public void onCaptureButtonClicked()
    {
        StartCoroutine(captureScreenshot());
        print("onCaptureButtonClicked");
    }

    int screenshotCount = 0;
    IEnumerator captureScreenshot()
    {
        yield return new WaitForEndOfFrame();

        //print("persistent data path: garik: " + path);
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;

        yield return new WaitForEndOfFrame();


        Texture2D screenImage = new Texture2D(Screen.width, Screen.height);
        //Get Image from screen
        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

        //GameObject.Find("DebugRawImage").GetComponent<RawImage>().texture = screenImage;
        yield return new WaitForEndOfFrame();
        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;

        string fileName = "Glassee";
        string screenshotFilename;
        string date = System.DateTime.Now.ToString("ddMMyyHHmmss");
        screenshotFilename = fileName + "_" + date + ".jpg";


        StartCoroutine(saveCaptureScreenshot(screenImage, "Glassee", screenshotFilename));

    }

    IEnumerator saveCaptureScreenshot(Texture2D texture, string album, string fileName)
    {

        yield return NativeGallery.SaveImageToGallery(texture, album, fileName, (success, path) => Debug.Log("Media save result: " + success + " " + path));

        yield return null;
    }

}
