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
        StartCoroutine(m_glassesManager.loadGlasses(modelName));
    }

    public void addModelToCarousel(string name, Texture2D tex)
    {
        m_modelsCarouselController.AddCell(name, tex);
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
