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


    public GameObject m_carouselModels;
    public GameObject m_modelItem;

    public GameObject m_carouselBrandsNew;
    CarouselController m_brandsCarouselController;
    public GameObject m_carouselModelsNew;
    public CarouselController m_modelsCarouselController;


    GameObject m_glassesModelTextGameobject;
    void Awake()
    {
        m_glassesManager = GameObject.Find("_manager").GetComponent<GlassesManager>();
        m_mainController = GameObject.Find("_manager").GetComponent<MainController>();
        m_glassesModelTextGameobject = GameObject.Find("GlassesModelText");

        m_brandsCarouselController = GameObject.Find("BrandsCarouselController").GetComponent<CarouselController>();
        m_modelsCarouselController = GameObject.Find("ModelsCarouselController").GetComponent<CarouselController>();
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
        m_brandsCarouselController.AddCell(name, tex);

    }


    public void onModelButtonClicked(PointerEventData data)
    {

        string modelName = data.pointerClick.name;
        print("Garik: MODEL Name: " + modelName);

        m_glassesManager.loadGlasses(modelName);

    }

    public void addModelToCarousel(string name, Texture2D tex)
    {

        m_modelsCarouselController.AddCell(name, tex);
        return;
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
        if(Input.GetKeyDown(KeyCode.A))
        {
            //GameObject.Find("CarouselController").GetComponent<CarouselController>().AddCell(); 
        }

    }


    ////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////



    public void destroyModelsScrollRect()
    {

    }

}
