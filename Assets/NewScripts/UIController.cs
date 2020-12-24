using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AsPerSpec;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    GlassesManager m_glassesManager = new GlassesManager();
    MainController m_mainController = new MainController();


    public GameObject m_carouselBrands;
    public GameObject m_brandItem;
    public GameObject m_carouselModels;
    public GameObject m_modelItem;

    GameObject m_glassesModelTextGameobject;
    void Awake()
    {
        m_glassesManager = GameObject.Find("_manager").GetComponent<GlassesManager>();
        m_mainController = GameObject.Find("_manager").GetComponent<MainController>();
        m_glassesModelTextGameobject = GameObject.Find("GlassesModelText");

        //m_carouselBrands = GameObject.Find("CarouselBrands");
        //m_brandItem = GameObject.Find("BrandItem");
        //m_brandItem.SetActive(false);
        //m_modelItem = GameObject.Find("ModelItem");
        //m_modelItem.SetActive(false);

        //initializeEventPanel();

    }

    public void onBrandButtonClicked(PointerEventData data)
    {
        string brandName = data.pointerClick.name;
        m_glassesManager.showModels(brandName);
        print("Garik: Brand Name: " + brandName);

    }

    public void addBrandToCarousel(string name, Texture2D tex)
    {

        GameObject brandItem = Instantiate(m_brandItem);
        brandItem.SetActive(true);
        brandItem.name = "BrandItem";

        RectTransform rt = brandItem.transform.GetChild(0).GetComponent<RectTransform>();



        rt.gameObject.AddComponent<EventTrigger>();
        EventTrigger eventTrigger = rt.GetComponent<EventTrigger>();
        EventTrigger.Entry clicked = new EventTrigger.Entry();
        clicked.eventID = EventTriggerType.PointerClick;
        clicked.callback.AddListener((data) => { onBrandButtonClicked((PointerEventData)data); });
        eventTrigger.triggers.Add(clicked);


        brandItem.transform.GetChild(0).name = name;
        brandItem.transform.GetChild(0).GetComponent<RawImage>().texture = tex;
        RectTransform brandCarouselContent = m_carouselBrands.transform.Find("ScrollRect").Find("Content").GetComponent<RectTransform>();
        brandItem.transform.SetParent(brandCarouselContent);

        rt.GetComponent<FixScrollRect>().MainScroll = rt.parent.parent.parent.GetComponent<ScrollRect>();

    }


    public void onModelButtonClicked(PointerEventData data)
    {

        string modelName = data.pointerClick.name;
        print("Garik: Brand Name: " + modelName);

        m_glassesManager.loadGlasses(modelName);

    }

    public void addModelToCarousel(string name, Texture2D tex)
    {

        GameObject modelItem = Instantiate(m_modelItem);
        modelItem.SetActive(true);
        modelItem.name = "ModelItem";

        RectTransform rt = modelItem.transform.GetChild(0).GetComponent<RectTransform>();

        rt.gameObject.AddComponent<EventTrigger>();
        EventTrigger eventTrigger = rt.GetComponent<EventTrigger>();
        EventTrigger.Entry clicked = new EventTrigger.Entry();
        clicked.eventID = EventTriggerType.PointerClick;
        clicked.callback.AddListener((data) => { onModelButtonClicked((PointerEventData)data); });
        eventTrigger.triggers.Add(clicked);


        modelItem.transform.GetChild(0).name = name;
        modelItem.transform.GetChild(0).GetComponent<RawImage>().texture = tex;
        RectTransform modelCarouselContent = m_carouselModels.transform.Find("ScrollRect").Find("Content").GetComponent<RectTransform>();
        modelItem.transform.SetParent(modelCarouselContent);

        rt.GetComponent<FixScrollRect>().MainScroll = rt.parent.parent.parent.GetComponent<ScrollRect>();
    }




    void Update()
    {

    }


    ////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////



    public void destroyModelsScrollRect()
    {

    }

}
