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
            onModelButtonClicked(data);
            return;
        }

        m_glassesManager.showModels(d);
        print("Garik: Brand Name: " + d);

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
