using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class GlassesManager : MonoBehaviour
{
    public List<GameObject> m_glasses = new List<GameObject>();

    GameObject m_glassesContainer;
    UIController m_uIController;
    MainController m_mainController;

    string m_currentModelsPath = Application.streamingAssetsPath + "/NetworkingFolder" + "/Brands/" + "Adensco" + "/Models"; //TODO change Adensco

    void Awake()
    {
        m_uIController = GameObject.Find("_manager").GetComponent<UIController>();
        m_mainController = GameObject.Find("_manager").GetComponent<MainController>();
        m_glassesContainer = GameObject.Find("GlassesContainer");
        initializeBrands();
        StartCoroutine(loadGlasses("glasses_1"));
    }

    public Texture2D m_debugTexture;
    Texture2D getTexture2D(string path)
    {
        byte[] pngBytes = System.IO.File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(100, 100);
        tex.LoadImage(pngBytes);
        tex.Apply();
        m_debugTexture = tex;
        return tex;
    }

    public GameObject m_carouselModelsNewTemplate;
    public void showModels(string brandName)
    {


        Destroy(m_uIController.m_carouselModelsNew);
        GameObject newCarouselModelsNew = Instantiate(m_carouselModelsNewTemplate, GameObject.Find("Canvas").transform);
        newCarouselModelsNew.name = "CarouselModelsNEW";
        m_uIController.m_carouselModelsNew = newCarouselModelsNew;
        m_uIController.m_modelsCarouselController = newCarouselModelsNew.transform.Find("ModelsCarouselController").GetComponent<CarouselController>();



        /*int c = GameObject.Find("ModelsCarouselController").GetComponent<CarouselController>().m_cellContainer.Count; //GameObject.Find("CarouselModelsNEW").transform.GetChild(0).GetChild(0).childCount;
        for (int i = 0; i < c; i++)
        {
            //Destroy(GameObject.Find("CarouselModelsNEW").transform.GetChild(0).GetChild(0).GetChild(i).gameObject);
            //GameObject.Find("ModelsCarouselController").GetComponent<CarouselController>().RemoveCell(i);
            //GameObject.Find("ModelsCarouselController").GetComponent<CarouselController>().RemoveCenterCell();
            //GameObject.Find("ModelsCarouselController").GetComponent<CarouselController>().m_cellContainer.RemoveAt(i);
        }*/


        m_currentModelsPath = brandName;

        DirectoryInfo d = new DirectoryInfo(Application.streamingAssetsPath + "/NetworkingFolder" + "/Brands/" + brandName + "/Models");
        m_currentModelsPath = Application.streamingAssetsPath + "/NetworkingFolder" + "/Brands/" + brandName + "/Models";

        FileInfo[] fis = d.GetFiles();


        List<int> nameIndexList = new List<int>();
        foreach (FileInfo fi in fis)
        {
            if (fi.Extension.Contains("png"))
            {
                string s = fi.ToString();
                string fileName = s.Split('/').Last();

                string nameIndexWithExtention = fileName.Split('_').Last();
                string nameIndexNoExtention = Path.GetFileNameWithoutExtension(nameIndexWithExtention);

                print("Garik nameIndex: " + nameIndexNoExtention);
                nameIndexList.Add(int.Parse(nameIndexNoExtention));
            }
        }
        int maxNameIndex = nameIndexList.Max();
        int minNameIndex = nameIndexList.Min();
        nameIndexList.Sort();

        int modelCounter = 0;
        foreach (FileInfo fi in fis)
        {
            if (fi.Extension.Contains("png"))
            {
                Texture2D tex = getTexture2D(Application.streamingAssetsPath + "/NetworkingFolder" + "/Brands/" + brandName + "/Models/" + "glasses_" + nameIndexList[modelCounter] + ".png");
                m_uIController.addModelToCarousel("glasses_" + nameIndexList[modelCounter], tex);
                modelCounter++;

            }
        }
        m_uIController.m_carouselModelsNew.SetActive(true);

    }


    void initializeBrands()
    {
        string[] brandDirectories = Directory.GetDirectories(Application.streamingAssetsPath + "/NetworkingFolder" + "/Brands");
        for (int i = 0; i < brandDirectories.Length; i++)
        {
            string folderName = brandDirectories[i].Split('/').Last();
            print("Garik dir names: " + folderName);
            Texture2D brandThumb = getTexture2D(Application.streamingAssetsPath + "/NetworkingFolder" + "/Brands/" + folderName + "/BrandThumb.png");

            m_uIController.addBrandToCarousel(folderName, brandThumb);
        }


    }



    public IEnumerator loadGlasses(string glassesName)
    {
        if (m_mainController.m_currentGlasses != null)
        {
            Destroy(m_mainController.m_currentGlasses);
        }
        print("Garik combine string: " + Path.Combine(m_currentModelsPath, glassesName));
        var bundleLoadRequest = AssetBundle.LoadFromFileAsync(Path.Combine(m_currentModelsPath, glassesName));

        yield return bundleLoadRequest;

        var myLoadedAssetBundle = bundleLoadRequest.assetBundle;
        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            yield break;
        }

        var assetLoadRequest = myLoadedAssetBundle.LoadAssetAsync<GameObject>(glassesName);
        yield return assetLoadRequest;

        GameObject prefab = assetLoadRequest.asset as GameObject;

        GameObject g = Instantiate(prefab);
        g.name = glassesName;
        g.SetActive(false);
        g.transform.SetParent(m_glassesContainer.transform);
        myLoadedAssetBundle.Unload(false);

        m_mainController.setGlasses(g);

    }

    void Update()
    {

    }
}
