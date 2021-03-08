using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GlassesInfo
{
    public string m_glassesName;
    public string m_glassesImagePath;
    public string m_glassesBundlePath;
    public string m_glassesBrand;
}

public class SearchController : MonoBehaviour
{

    public List<GlassesInfo> m_glassesInfoList = new List<GlassesInfo>();

    UIController m_uIController;
    MainController m_mainController;
    GlassesManager m_glassesManager;

    GameObject m_glassesContainer;


    //TouchScreenKeyboard m_searchKeyboard = new TouchScreenKeyboard("", TouchScreenKeyboardType.Default, false, false, false, false, "", 100);
    //TouchScreenKeyboard.hideInput = true;

    private void Awake()
    {


    }

    void Start()
    {
        m_uIController = GameObject.Find("_manager").GetComponent<UIController>();
        m_mainController = GameObject.Find("_manager").GetComponent<MainController>();
        m_glassesManager = GameObject.Find("_manager").GetComponent<GlassesManager>();

        m_glassesContainer = GameObject.Find("GlassesContainer");
        initializeGlassesInfo();
        m_uIController.hideSearchPanel();


        TouchScreenKeyboard.hideInput = true;

        EventTrigger eventTrigger = m_uIController.m_searchInputField.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry ptrClickEntry = new EventTrigger.Entry();
        ptrClickEntry.eventID = EventTriggerType.PointerClick;
        ptrClickEntry.callback.AddListener((data) => { onSearchInputFieldPtrClicked((PointerEventData)data); });
        eventTrigger.triggers.Add(ptrClickEntry);



        //updateSearchResultsUI();

        /*for (int i = 0; i < m_glassesInfoList.Count; i++)
        {
            print("GARIKKK AFTERRRR GLASSES NAME: " + m_glassesInfoList[i].m_glassesName);
            print("GARIKKK AFTERRRR GLASSES BRAND: " + m_glassesInfoList[i].m_glassesBrand);
            print("GARIKKK AFTERRRR GLASSES IMAGE PATH: " + m_glassesInfoList[i].m_glassesImagePath);
            print("GARIKKK AFTERRRR GLASSES BUNDLE PATH: " + m_glassesInfoList[i].m_glassesBundlePath);
        }*/

        m_uIController.m_searchInputField.onValueChanged.AddListener(delegate { onSearchInputFieldValueChanged(); });

        //m_uIController.m_searchInputField..AddListener(delegate { onSearchInputFieldEdit(); });
    }

    IEnumerator touchScreenKeyboardVisibilityCorotuine()
    {
        yield return new WaitUntil(() => TouchScreenKeyboard.visible == true);
        TouchScreenKeyboard.hideInput = true;
        m_uIController.m_bottomButtonsPanel.gameObject.SetActive(false);
        yield return new WaitUntil(() => TouchScreenKeyboard.visible == false);
        m_uIController.m_bottomButtonsPanel.gameObject.SetActive(true);

        //print("Garik +++ TouchScreenKeyboard.Status.Visible");
    }

    public void onSearchInputFieldPtrClicked(PointerEventData data)
    {
        print("Garik Clicked On Search Input Field");
        m_uIController.m_selectedModelMarker.transform.SetParent(null);
        m_uIController.m_selectedModelMarker.SetActive(false);


        m_uIController.m_carouselModelsNew.transform.Find("_carouselManager").GetComponent<GCarouselController>().setCellArrangementMethod(GCarouselController.CellArrangementMethod.eFromCenterToRight);

        if (GameObject.Find("CarouselModelsNEW") != null)
        {
            int c = GameObject.Find("CarouselModelsNEW").transform.Find("Viewport").Find("Content").childCount;
            GameObject.Find("CarouselModelsNEW").transform.Find("_carouselManager").GetComponent<GCarouselController>().m_cellList.Clear();
            GameObject.Find("CarouselModelsNEW").transform.Find("_carouselManager").GetComponent<GCarouselController>().m_cellList.Capacity = 0;
            for (int k = 0; k < c; k++)
            {
                Destroy(GameObject.Find("CarouselModelsNEW").transform.Find("Viewport").Find("Content").GetChild(k).gameObject);

            }
        }


        m_uIController.m_bottomButtonsPanel.gameObject.SetActive(false);
        m_uIController.m_carouselBrandsNew.SetActive(false);
        //m_uIController.m_carouselModelsNew.SetActive(false);
        //m_uIController.m_carouselModelsNew.transform.localScale = Vector3.zero;

        StartCoroutine(touchScreenKeyboardVisibilityCorotuine());
    }

    /*void updateSearchResultsUI()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject res = Instantiate(m_uIController.m_FoundGlassesTemplate.gameObject);
            res.name = "result_" + i;
            res.transform.SetParent(m_uIController.m_searchResultPanel);
            res.SetActive(true);
        }
    }*/

    void onSearchInputFieldValueChanged()
    {
        //TouchScreenKeyboard.hideInput = true;
        m_uIController.showSearchPanel();

        for (int i = 0; i < m_uIController.m_searchResultPanel.childCount; i++)
        {
            Destroy(m_uIController.m_searchResultPanel.GetChild(i).gameObject);
        }

        string findText = m_uIController.m_searchInputField.text;
        //print("Search Input Field Val: " + findText);



        List<GlassesInfo> returnedGlassesInfoList = findGlassesList(m_glassesInfoList, findText);


        m_uIController.m_carouselModelsNew.transform.Find("_carouselManager").GetComponent<GCarouselController>().setCellArrangementMethod(GCarouselController.CellArrangementMethod.eFromCenterToRight);

        if (GameObject.Find("CarouselModelsNEW") != null)
        {
            int c = GameObject.Find("CarouselModelsNEW").transform.Find("Viewport").Find("Content").childCount;
            GameObject.Find("CarouselModelsNEW").transform.Find("_carouselManager").GetComponent<GCarouselController>().m_cellList.Clear();
            GameObject.Find("CarouselModelsNEW").transform.Find("_carouselManager").GetComponent<GCarouselController>().m_cellList.Capacity = 0;
            for (int k = 0; k < c; k++)
            {
                Destroy(GameObject.Find("CarouselModelsNEW").transform.Find("Viewport").Find("Content").GetChild(k).gameObject);

            }
        }
        for (int i = 0; i < returnedGlassesInfoList.Count; i++)
        {
            GameObject res = Instantiate(m_uIController.m_FoundGlassesTemplate.gameObject);
            res.name = returnedGlassesInfoList[i].m_glassesName;

            res.transform.Find("NameText").GetComponent<Text>().text = returnedGlassesInfoList[i].m_glassesName.Substring(0, 15);//returnedGlassesInfoList[i].m_glassesName;

            Texture2D tex = getTexture2D(returnedGlassesInfoList[i].m_glassesImagePath);
            res.GetComponent<RawImage>().texture = tex;

            res.transform.SetParent(m_uIController.m_searchResultPanel);


            res.GetComponent<Button>().onClick.AddListener(() => onResultModelClicked());

            res.SetActive(true);


    
            m_uIController.addModelToCarousel(res.name, tex);
        }
    }

    public void onResultModelClicked()    
    {
        //m_uIController.m_carouselModelsNew.SetActive(true);
        //m_uIController.m_carouselModelsNew.transform.localScale = Vector3.one;
        m_uIController.hideSearchPanel();
        m_uIController.m_bottomButtonsPanel.gameObject.SetActive(true);
        var go = EventSystem.current.currentSelectedGameObject;
        string modelName = go.name;

        for (int i = 0; i < m_glassesInfoList.Count; i++)
        {
            if(m_glassesInfoList[i].m_glassesName.Contains(modelName))
            {
                print("CLICKED GLASSES PATH: " + m_glassesInfoList[i].m_glassesBundlePath);
                string p = Application.streamingAssetsPath + "/NetworkingFolder" + "/Brands/" + m_glassesInfoList[i].m_glassesBrand + "/Models";
                print("BrandName: " + m_glassesInfoList[i].m_glassesBrand);
                m_glassesManager.setCurrentModelsPath(p);
                //StartCoroutine(loadGlasses(modelName, m_glassesInfoList[i].m_glassesBundlePath));
                StartCoroutine(m_glassesManager.loadGlasses(modelName));
            }
        }

    }
    /*

    public IEnumerator loadGlasses(string glassesName, string path)
    {
        if (m_mainController.m_currentGlasses != null)
        {
            Destroy(m_mainController.m_currentGlasses);
        }
        //print("Garik combine string: " + Path.Combine(m_currentModelsPath, glassesName.ToLower()));

        var bundleLoadRequest = AssetBundle.LoadFromFileAsync(path.ToLower());

        yield return bundleLoadRequest;

        var myLoadedAssetBundle = bundleLoadRequest.assetBundle;
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            yield break;
        }

        var assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync<GameObject>(glassesName.ToLower());
        yield return assetLoadRequest;

        GameObject prefab = assetLoadRequest.asset as GameObject;

        GameObject g = Instantiate(prefab);
        g.name = glassesName;
        g.SetActive(true);
        g.transform.SetParent(m_glassesContainer.transform);
        myLoadedAssetBundle.Unload(false);

        m_mainController.setGlasses(g);

        //GameObject.Find("CarouselModelsNEW").transform.Find("_carouselManager").GetComponent<GCarouselController>().m_selectedModelMarker.transform.SetParent(GameObject.Find(glassesName).transform);
        //GameObject.Find("SelectedModelMarker");//.transform.SetParent(GameObject.Find(glassesName).transform);
    }

    */
    List<GlassesInfo> findGlassesList(List<GlassesInfo> glassesInfoList, string nameStr)
    {



        List<GlassesInfo> returnableList = new List<GlassesInfo>();
        if (nameStr != "")
        {
            //print("GARIKKKK COUNT: " + glassesInfoList.Count);
            for (int i = 0; i < glassesInfoList.Count; i++)
            {
                string name = glassesInfoList[i].m_glassesName.ToUpper();
                string nameStrU = nameStr.ToUpper();

                //print("G IF IC DUS: " + glassesInfoList[i].m_glassesImagePath + "QQQQQQ: " + glassesInfoList[i].m_glassesName);
                if (name.Contains(nameStrU))
                {
                    //print("G IFFFFFF: " + glassesInfoList[i].m_glassesImagePath);
                    GlassesInfo g = new GlassesInfo();
                    g.m_glassesBrand = glassesInfoList[i].m_glassesBrand;
                    g.m_glassesBundlePath = glassesInfoList[i].m_glassesBundlePath;
                    g.m_glassesImagePath = glassesInfoList[i].m_glassesImagePath;
                    g.m_glassesName = glassesInfoList[i].m_glassesName;

                    //print("G IMAGE PATH: " + g.m_glassesImagePath);

                    returnableList.Add(g);
                }
            }
        }
        return returnableList;
    }

    Texture2D getTexture2D(string path)
    {
        byte[] pngBytes = System.IO.File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(100, 100);
        tex.LoadImage(pngBytes);
        tex.Apply();
        //m_debugTexture = tex;
        return tex;
    }

    private void initializeGlassesInfo()
    {
        string[] brandDirectories = Directory.GetDirectories(Application.streamingAssetsPath + "/NetworkingFolder" + "/Brands");
        for (int i = 0; i < brandDirectories.Length; i++)
        {
            string brandName = brandDirectories[i].Split('/').Last();
            //print("Garik folderName: " + brandDirectories[i]);



            DirectoryInfo d = new DirectoryInfo(Application.streamingAssetsPath + "/NetworkingFolder" + "/Brands/" + brandName + "/Models");
            //m_currentModelsPath = Application.streamingAssetsPath + "/NetworkingFolder" + "/Brands/" + brandName + "/Models";

            FileInfo[] fis = d.GetFiles();
            List<int> nameIndexList = new List<int>();
            foreach (FileInfo fi in fis)
            {
                if (fi.Extension.Contains("jpg"))
                {
                    string modelName = Path.GetFileNameWithoutExtension(fi.Name);
                    GlassesInfo gInfo = new GlassesInfo();
                    gInfo.m_glassesBrand = brandName;
                    gInfo.m_glassesName = modelName;
                    gInfo.m_glassesImagePath = Application.streamingAssetsPath + "/NetworkingFolder" + "/Brands/" + brandName + "/Models/" + modelName + ".jpg";
                    gInfo.m_glassesBundlePath = Application.streamingAssetsPath + "/NetworkingFolder" + "/Brands/" + brandName + "/Models/" + modelName;

                    //print("GARIK FI NAMEEEEE: " + gInfo.m_glassesImagePath);

                    m_glassesInfoList.Add(gInfo);
                }
            }
        }

    }

    //List<GlassesInfo>  = new List<GlassesInfo>();
    void Update()
    {

    }
}
