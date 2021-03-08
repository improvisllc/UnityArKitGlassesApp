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

    public InputField m_searchInputField;
    public RectTransform m_searchPanel;
    public RectTransform m_searchResultPanel;
    public RectTransform m_FoundGlassesTemplate;

    public RectTransform m_bottomButtonsPanel;

    public RenderTexture m_renderTextureOutput;

    public GameObject m_carouselBrandsNew;

    public GameObject m_carouselModelsNew;

    public GameObject m_selectedBrandBorder;
    public GameObject m_selectedModelMarker;

    public RectTransform m_centerCircleButton;

    public RectTransform m_videoOutputRawimage;

    public RectTransform m_photoOutputRawimage;


    public void showSearchPanel()
    {
        m_searchPanel.gameObject.SetActive(true);
    }
    public void hideSearchPanel()
    {
        m_searchPanel.gameObject.SetActive(false);
    }


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


        //m_searchInputField.keyboardType =  TouchScreenKeyboardType.Default;



            


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
        m_glassesManager.showModels("Prada");
    }
    string m_currentChoosedClickedName = "";
    public void onBrandButtonClicked(PointerEventData data)
    {
        string d = data.pointerClick.name;
        m_currentChoosedClickedName = d;
        m_glassesManager.showModels(d);
    }
    /*public void onModelButtonClicked(PointerEventData data)
    {
        string d = data.pointerClick.name;
        if(d.Contains("glasses_"))
        {
            print("Garik: Model Name: " + d);
            onModelButtonClicked(data);
        }
    }*/


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
            //print("Garik Stop Recording");
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
            //print("Garik Start Recording");
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
        //print("onCaptureButtonClicked");
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

        //print("Taking ScreenShot Garik");
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
        //print("Garik m_currentSavedCaptureScreenshotPath: " + m_currentSavedCaptureScreenshotPath);
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
        //print("onFlashButtonClicked clicked");
        if (m_flashButton.GetChild(0).gameObject.activeInHierarchy)
        {
            PlayerPrefs.SetInt(Constants.s_flashButtonPreferenceName, 0);
            m_flashButton.GetChild(0).gameObject.SetActive(false);
            m_flashButton.GetChild(1).gameObject.SetActive(true);
            return;
        }
        if (m_flashButton.GetChild(1).gameObject.activeInHierarchy)
        {
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

    public void onBackSearchBtnClicked()
    {
        /*m_carouselModelsNew.transform.Find("_carouselManager").GetComponent<GCarouselController>().setCellArrangementMethod(GCarouselController.CellArrangementMethod.eFromCenterToRight);

        if (GameObject.Find("CarouselModelsNEW") != null)
        {
            int c = GameObject.Find("CarouselModelsNEW").transform.Find("Viewport").Find("Content").childCount;
            GameObject.Find("CarouselModelsNEW").transform.Find("_carouselManager").GetComponent<GCarouselController>().m_cellList.Clear();
            GameObject.Find("CarouselModelsNEW").transform.Find("_carouselManager").GetComponent<GCarouselController>().m_cellList.Capacity = 0;
            for (int k = 0; k < c; k++)
            {
                Destroy(GameObject.Find("CarouselModelsNEW").transform.Find("Viewport").Find("Content").GetChild(k).gameObject);

            }
        }*/

        print("Garik m_currentChoosedClickedName " + m_currentChoosedClickedName);
        if(!m_searchPanel.gameObject.activeSelf)
        {
            showSearchPanel();
            return;
        }
        else
        {
            hideSearchPanel(); 
        }

        m_bottomButtonsPanel.gameObject.SetActive(true);
        m_carouselBrandsNew.SetActive(true);
        m_glassesManager.showModels(m_currentChoosedClickedName);
    }
}
