using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using pmjo.NextGenRecorder;
using pmjo.NextGenRecorder.Sharing;
using UnityEngine.Video;
using DG.Tweening;
using System;

public class UIController : MonoBehaviour
{
    GlassesManager m_glassesManager = new GlassesManager();
    MainController m_mainController = new MainController();

    public RenderTexture m_renderTextureOutput;

    public GameObject m_carouselBrandsNew;

    public GameObject m_carouselModelsNew;

    public RectTransform m_centerCircleButton;

    public RectTransform m_videoOutputRawimage;

    public RectTransform m_photoOutputRawimage;

    public void showPhotoOutputRawimage()
    {
        m_photoOutputRawimage.gameObject.SetActive(true);
    }
    public void hidePhotoOutputRawimage()
    {
        m_photoOutputRawimage.gameObject.SetActive(false);
    }

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
        GameObject.Find("VideoPlayer").GetComponent<VideoPlayer>().Stop();
        m_renderTextureOutput = null;
    }
    public void onSaveVideoBtnClicked()
    {
        this.gameObject.GetComponent<pmjo.Examples.SimpleRecorder>().saveVideoToGallery();
        showSavedTextUI();
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
    public Text m_savedText;

    void showSavedTextUI()
    {
        m_savedText.GetComponent<Text>().DOFade(1, 0.5f).OnComplete(onFinishSavedTextShowing);
    }
    void onFinishSavedTextShowing()
    {
        StartCoroutine(onFinishSavedTextShowingCoroutine());
    }
    IEnumerator onFinishSavedTextShowingCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        m_savedText.GetComponent<Text>().DOFade(0, 0.5f);
    }

    void Awake()
    {
        m_glassesManager = GameObject.Find("_manager").GetComponent<GlassesManager>();
        m_mainController = GameObject.Find("_manager").GetComponent<MainController>();
        m_glassesModelTextGameobject = GameObject.Find("GlassesModelText");
        flashButtonImageChekerOnStart();
        //m_brandsCarouselController = m_carouselBrandsNew.transform.Find("_carouselManager").GetComponent<GCarouselController>();
        //m_modelsCarouselController = m_carouselModelsNew.transform.Find("_carouselManager").GetComponent<GCarouselController>();
    }

    void Start()
    {
        firstCallForBrandsAndModels();
        m_textureData = null;

        addListeners();
        //m_renderTextureOutput = null;

        //initializeLineRenderers();
        //setMaxTime(1000);
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
    bool m_canTakeSnapshot = true;

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
            //StartCoroutine(stopRecordingCoroutine());
        }
    }


    bool m_recCanStart = false;


    float m_currentTime = 0;
    float m_maxTime = 1000;
    void Update()
    {
        m_currentTime = Mathf.Clamp(m_currentTime - Time.deltaTime, 0, m_maxTime);
        //setTimerValue(m_currentTime, (m_maxTime - m_currentTime) / m_maxTime);


        if (m_startTimerForRecordButton)
        {
            m_recordButtonPointerDownElapsedTime = Time.time - m_recordButtonPointerDownStartTime;

            if (m_recordButtonPointerDownElapsedTime > 1)
            {
                m_canTakeSnapshot = false;
                m_recCanStart = true;
            }
        }

        if(m_recCanStart)
        {
            print("Garik Start Recording");
            GameObject.Find("_manager").GetComponent<pmjo.Examples.SimpleRecorder>().StartRecording();
            m_recCanStart = false;
        }
    }

    public void onCaptureButtonClicked()
    {

        if(!m_canTakeSnapshot)
        {
            m_canTakeSnapshot = true;
            return;
        }
        StartCoroutine(takeScreenshot());
        print("onCaptureButtonClicked");
    }

    public int resWidth = 2550;
    public int resHeight = 3300;

    public static string ScreenShotName(int width, int height)
    {
        return string.Format("{0}/screen_{1}x{2}_{3}.png",
                             Application.persistentDataPath,
                             width, height,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }
    public RawImage m_flashingImage;

    string m_currentSavedCaptureScreenshotPath = "";
    public Texture2D m_currentCaptureScreenshot;

    IEnumerator takeScreenshot()
    {
        if (PlayerPrefs.GetInt(Constants.s_flashButtonPreferenceName) == 1)
        {
            m_flashingImage.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);
        }
        if (PlayerPrefs.GetInt(Constants.s_flashButtonPreferenceName) == 0)
        {
            m_flashingImage.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.0f);
        }

        print("Taking ScreenShot Garik");
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);
        Camera.main.targetTexture = rt;
        Texture2D screenShot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        Camera.main.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);

        m_currentCaptureScreenshot = screenShot;


        Camera.main.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string filename = ScreenShotName(Screen.width, Screen.height);
        System.IO.File.WriteAllBytes(filename, bytes);
        Debug.Log(string.Format("Took screenshot to: {0}", filename));
        m_currentSavedCaptureScreenshotPath = filename;
        m_flashingImage.gameObject.SetActive(false);

        Texture2D t = NativeGallery.LoadImageAtPath(m_currentSavedCaptureScreenshotPath);
        yield return new WaitForSeconds(0.5f);
        m_photoOutputRawimage.GetComponent<RawImage>().texture = t;
        showPhotoOutputRawimage();
        //yield return new WaitUntil(() => m_flashingImage.enabled == true);


    }

    IEnumerator saveCaptureScreenshot(Texture2D texture, string album, string fileName)
    {
        yield return NativeGallery.SaveImageToGallery(texture, album, fileName, (success, path) => Debug.Log("Garik Media save result: " + success + " " + path));
        print("Garik m_currentSavedCaptureScreenshotPath: " + m_currentSavedCaptureScreenshotPath);
        //yield return new WaitForSeconds(1);
        m_flashingImage.gameObject.SetActive(false);
        yield return null;
    }
    public void onSaveCurrentCapturedScreenshotToGallery()
    {
        string fileName = "Glassee";
        string screenshotFilename;
        string date = System.DateTime.Now.ToString("ddMMyyHHmmss");
        screenshotFilename = fileName + "_" + date + ".jpg";
        StartCoroutine(saveCaptureScreenshot(m_currentCaptureScreenshot, "Glassee", screenshotFilename));
    }
    public void onShareCurrentCapturedScreenshot()
    {
        Sharing.ShowShareSheet(m_currentSavedCaptureScreenshotPath);
    }
    public void onCloseCapturedScreenshotPanel()
    {
        hidePhotoOutputRawimage(); 
    }

    public RectTransform m_flashButton;
    public void onFlashButtonClicked()
    {
        print("onFlashButtonClicked clicked");
        if (m_flashButton.GetChild(0).gameObject.activeInHierarchy)
        {
            print("00000 is active");
            PlayerPrefs.SetInt(Constants.s_flashButtonPreferenceName, 0);
            m_flashButton.GetChild(0).gameObject.SetActive(false);
            m_flashButton.GetChild(1).gameObject.SetActive(true);
            return;
        }
        if (m_flashButton.GetChild(1).gameObject.activeInHierarchy)
        {
            print("111111 is active");
            PlayerPrefs.SetInt(Constants.s_flashButtonPreferenceName, 1);
            m_flashButton.GetChild(1).gameObject.SetActive(false);
            m_flashButton.GetChild(0).gameObject.SetActive(true);
            return;
        }

    }

    void flashButtonImageChekerOnStart()
    {
        if(PlayerPrefs.GetInt(Constants.s_flashButtonPreferenceName,1) == 1)
        {
            m_flashButton.GetChild(0).gameObject.SetActive(true);
            m_flashButton.GetChild(1).gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt(Constants.s_flashButtonPreferenceName) == 0)
        {
            m_flashButton.GetChild(0).gameObject.SetActive(false);
            m_flashButton.GetChild(1).gameObject.SetActive(true);
        }
    }









    /*





    public void setMaxTime(float m)
    {
        m_maxTime = m;
        setTimerValue(m_currentTime, (m_maxTime - m_currentTime) / m_maxTime);
    }



    LineRenderer m_ellapsedTimeLineRenderer;// = new LineRenderer();
    GameObject m_ellapsedTimeLineRendererGameObject;// = new LineRenderer();
    GameObject m_passedCheckPointsIndicator;// = new LineRenderer();

    public void setTimerValue(float val, float normalizedValue)
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(val);
        string answer = string.Format("{1:D2}m {2:D2}s",
            t.Hours,
            t.Minutes,
            t.Seconds,
            t.Milliseconds);

        //m_canvasController.m_timerItem.text = answer;
        updateEllapsedTimeLineRenderer(normalizedValue);
    }

    void initializeLineRenderers()
    {

        m_ellapsedTimeLineRendererGameObject = new GameObject("ellapsedTimeLineRenderer");

        m_ellapsedTimeLineRendererGameObject.transform.position = Vector3.zero;

        m_ellapsedTimeLineRenderer = m_ellapsedTimeLineRendererGameObject.AddComponent<LineRenderer>();

       //Material passedCheckPointIndicatorMaterial = Resources.Load(Constants.s_globalObjectsFolderName + "/" + Constants.s_passedCheckPointIndicatorMaterialsResourceFolderName + "/" + Constants.s_gimblePassedCheckPointIndicatorMaterialResourceName, typeof(Material)) as Material;

       //m_ellapsedTimeLineRenderer.material = passedCheckPointIndicatorMaterial;

    }

    public void updateEllapsedTimeLineRenderer(float scale)
    {
        float x;
        float y;
        float z = 1f;

        float angle = 0f;

        int segments = 128;
        float width = 0.02f;
        float xRadius = 0.5f - width;
        float yRadius = 0.5f - width;

        m_ellapsedTimeLineRenderer.SetVertexCount(segments + 1);
        m_ellapsedTimeLineRenderer.SetColors(new Color(255, 0, 0), new Color(200, 0, 0));
        m_ellapsedTimeLineRenderer.SetWidth(width, width);

        for (int i = 0; i < segments + 1; i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xRadius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yRadius;
            m_ellapsedTimeLineRenderer.SetPosition(i, new Vector3(x, y, z) + Vector3.zero);
            angle += ((scale * 360.0f) / segments);
        }
    }*/
}
