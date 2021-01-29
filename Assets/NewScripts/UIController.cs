using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using pmjo.NextGenRecorder;
using pmjo.NextGenRecorder.Sharing;

public class UIController : MonoBehaviour
{
    GlassesManager m_glassesManager = new GlassesManager();
    MainController m_mainController = new MainController();


    public GameObject m_carouselBrandsNew;

    public GameObject m_carouselModelsNew;

    public RectTransform m_centerCircleButton;

    public RectTransform m_videoOutputRawimage;

    public void showVideoOutputRawimage()
    {
        m_videoOutputRawimage.gameObject.SetActive(true);
    }
    public void hideVideoOutputRawimage()
    {
        m_videoOutputRawimage.gameObject.SetActive(false);
    }

    public void onCloseVideoBtnClicked()
    {
        hideVideoOutputRawimage();
    }
    public void onSaveVideoBtnClicked()
    {
        this.gameObject.GetComponent<pmjo.Examples.SimpleRecorder>().saveVideoToGallery();
    }
    public void onShareVideoBtnClicked()
    {
        this.gameObject.GetComponent<pmjo.Examples.SimpleRecorder>().shareRecordedVideo();
    }
    public RectTransform m_videoOutputBottomPanel;

    public void hideVideoOutputBottomPanel()
    {
        m_videoOutputBottomPanel.gameObject.SetActive(false);
    }
    public void showVideoOutputBottomPanel()
    {
        m_videoOutputBottomPanel.gameObject.SetActive(true);
    }

    GameObject m_glassesModelTextGameobject;
    void Awake()
    {
        m_glassesManager = GameObject.Find("_manager").GetComponent<GlassesManager>();
        m_mainController = GameObject.Find("_manager").GetComponent<MainController>();
        m_glassesModelTextGameobject = GameObject.Find("GlassesModelText");
        //m_brandsCarouselController = m_carouselBrandsNew.transform.Find("_carouselManager").GetComponent<GCarouselController>();
        //m_modelsCarouselController = m_carouselModelsNew.transform.Find("_carouselManager").GetComponent<GCarouselController>();
    }

    void Start()
    {
        firstCallForBrandsAndModels();
        m_textureData = null;

        addListeners();
    }

    public void firstCallForBrandsAndModels()
    {
        m_glassesManager.showModels("Aristar");
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

    public RawImage m_debugRawImage;
    public byte[] m_textureData;
    public void startRecord()
    {
        /*
        var texture = new Texture2D(480, 640, TextureFormat.ARGB32, false);
        // set the pixel values
        texture.SetPixel(0, 0, new Color(1.0f, 1.0f, 1.0f, 0.5f));
        texture.SetPixel(1, 0, Color.clear);
        texture.SetPixel(0, 1, Color.white);
        texture.SetPixel(1, 1, Color.black);

        // Apply all SetPixel calls
        texture.Apply();

        NativeInfoProvider.init(480, 640);
        print("Garik Meri After Init");
        m_textureData = texture.EncodeToPNG();//.GetRawTextureData();
        print("Garik Meri After GetRawTextureData");*/
    }

    public void stopRecord()
    {
        /*
        NativeInfoProvider._appendFrameFromImage(m_textureData, m_textureData.Length, 50, 0);
        m_textureData = null;
        print("Garik Meri Stop");*/
    }


    void addListeners()
    {
        EventTrigger triggerDown = m_centerCircleButton.gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => onCenterCircleDownPtrDown());
        triggerDown.triggers.Add(pointerDown);

        EventTrigger triggerUp = m_centerCircleButton.gameObject.AddComponent<EventTrigger>();
        var pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((e) => onCenterCircleDownPtrUp());
        triggerUp.triggers.Add(pointerUp);
    }

    float m_recordButtonPointerDownStartTime = 0;
    float m_recordButtonPointerDownElapsedTime = 0;
    bool m_startTimerForRecordButton = false;

    void onCenterCircleDownPtrDown()
    {
        m_recordButtonPointerDownStartTime = Time.time;
        m_startTimerForRecordButton = true;
    }
    void onCenterCircleDownPtrUp()
    {
        if(m_startTimerForRecordButton)
        {
            print("Garik Stop Recording");
            m_startTimerForRecordButton = false;
            GameObject.Find("_manager").GetComponent<pmjo.Examples.SimpleRecorder>().StopRecording();
        }
    }

    bool m_recCanStart = false;
    void Update()
    {
        if(m_startTimerForRecordButton)
        {
            m_recordButtonPointerDownElapsedTime = Time.time - m_recordButtonPointerDownStartTime;

            if (m_recordButtonPointerDownElapsedTime > 1)
            {
                m_recCanStart = true;
            }
        }

        if(m_recCanStart)
        {
            print("Garik Start Recording");
            GameObject.Find("_manager").GetComponent<pmjo.Examples.SimpleRecorder>().StartRecording();
            m_recCanStart = false;
        }
        /*
        if (m_textureData != null && m_textureData.Length > 0)
        {
            print("Garik Call _appendFrameFromImage");
            print("Garik m_textureData: " + m_textureData);
            print("Garik m_textureData Length: " + m_textureData.Length);

            NativeInfoProvider._appendFrameFromImage(m_textureData, m_textureData.Length, 50, 1);
        }*/
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

        screenImage.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenImage.Apply();

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
