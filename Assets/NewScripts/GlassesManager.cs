﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class GlassesManager : MonoBehaviour
{
    public List<GameObject> m_glasses = new List<GameObject>();

    GameObject m_glassesContainer;
    UIController m_uIController;
    MainController m_mainController;

    string m_currentModelsPath = Application.streamingAssetsPath + "/NetworkingFolder" + "/Brands/" + "Prada" + "/Models"; //TODO change Adensco
    public void setCurrentModelsPath(string path)
    {
        m_currentModelsPath = path;
    }

    void Awake()
    {
        m_uIController = GameObject.Find("_manager").GetComponent<UIController>();
        m_mainController = GameObject.Find("_manager").GetComponent<MainController>();
        m_glassesContainer = GameObject.Find("GlassesContainer");
        initializeBrands();
        //StartCoroutine(loadGlasses("glasses_15"));
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
        m_uIController.m_carouselModelsNew.transform.Find("_carouselManager").GetComponent<GCarouselController>().setCellArrangementMethod(GCarouselController.CellArrangementMethod.eFromCenterToRight);
        //m_uIController.m_carouselModelsNew.transform.Find("Viewport").Find("Content").GetComponent<HorizontalLayoutGroup>().enabled = false;
        if (GameObject.Find("CarouselModelsNEW") != null)
        {
            int c = GameObject.Find("CarouselModelsNEW").transform.Find("Viewport").Find("Content").childCount;
            GameObject.Find("CarouselModelsNEW").transform.Find("_carouselManager").GetComponent<GCarouselController>().m_cellList.Clear();
            GameObject.Find("CarouselModelsNEW").transform.Find("_carouselManager").GetComponent<GCarouselController>().m_cellList.Capacity = 0;
            for (int i = 0; i < c; i++)
            {
                Destroy(GameObject.Find("CarouselModelsNEW").transform.Find("Viewport").Find("Content").GetChild(i).gameObject);
            }
        }

        m_currentModelsPath = brandName;

        DirectoryInfo d = new DirectoryInfo(Application.streamingAssetsPath + "/NetworkingFolder" + "/Brands/" + brandName + "/Models");
        m_currentModelsPath = Application.streamingAssetsPath + "/NetworkingFolder" + "/Brands/" + brandName + "/Models";

        FileInfo[] fis = d.GetFiles();

        int modelCounter = 0;
        List<int> nameIndexList = new List<int>();
        foreach (FileInfo fi in fis)
        {
            if (fi.Extension.Contains("jpg"))
            {
                modelCounter++;
                string s = fi.ToString();
                //print("GARIK FI NAMEEEEE: " + fi.Name);
                Texture2D tex = getTexture2D(Application.streamingAssetsPath + "/NetworkingFolder" + "/Brands/" + brandName + "/Models/" + fi.Name);

                //string nameIndexWithExtention = fileName.Split('_').Last();
                string nameIndexNoExtention = Path.GetFileNameWithoutExtension(fi.Name);

                m_uIController.addModelToCarousel(nameIndexNoExtention, tex);
                //m_uIController.addModelToCarousel("glasses_" + nameIndexList[modelCounter], tex);
                /*
                string fileName = s.Split('/').Last();

                string nameIndexWithExtention = fileName.Split('_').Last();
                string nameIndexNoExtention = Path.GetFileNameWithoutExtension(nameIndexWithExtention);

                //print("Garik nameIndex: " + nameIndexNoExtention);
                nameIndexList.Add(int.Parse(nameIndexNoExtention));*/
            }
        }
        /*int maxNameIndex = nameIndexList.Max();
        int minNameIndex = nameIndexList.Min();
        nameIndexList.Sort();

        int modelCounter = 0;
        foreach (FileInfo fi in fis)
        {
            if (fi.Extension.Contains("jpg"))
            {
                Texture2D tex = getTexture2D(Application.streamingAssetsPath + "/NetworkingFolder" + "/Brands/" + brandName + "/Models/" + nameIndexList[modelCounter] + ".jpg");
                m_uIController.addModelToCarousel("glasses_" + nameIndexList[modelCounter], tex);
                modelCounter++;

            }
        }*/
        m_uIController.m_carouselModelsNew.SetActive(true);

        //Invoke("focusModelsInvokeMethod", 0.1f);
        //m_uIController.m_carouselModelsNew.transform.Find("_carouselManager").GetComponent<GCarouselController>().m_carouselPanel.Find("Viewport").Find("Content").transform.localPosition = Vector3.zero;
        m_uIController.m_carouselModelsNew.transform.Find("_carouselManager").GetComponent<GCarouselController>().focusOnCellIndex(1);

    }


    void initializeBrands()
    {
        m_uIController.m_carouselBrandsNew.transform.Find("_carouselManager").GetComponent<GCarouselController>().setCellArrangementMethod(GCarouselController.CellArrangementMethod.eFromCenterToRight);
        m_uIController.m_carouselBrandsNew.transform.Find("_carouselManager").GetComponent<GCarouselController>().focusOnCellIndex(2);
        //m_uIController.m_carouselBrandsNew.transform.Find("_carouselManager").GetComponent<GCarouselController>().m_swapEnabled = true;
        string[] brandDirectories = Directory.GetDirectories(Application.streamingAssetsPath + "/NetworkingFolder" + "/Brands");
        for (int i = 0; i < brandDirectories.Length; i++)
        {
            string folderName = brandDirectories[i].Split('/').Last();
            //print("Garik dir names: " + folderName);
            Texture2D brandThumb = getTexture2D(Application.streamingAssetsPath + "/NetworkingFolder" + "/Brands/" + folderName + "/BrandThumb.png");

            m_uIController.addBrandToCarousel(folderName, brandThumb);
        }
        //Invoke("focusBrandsInvokeMethod", 0.1f);




    }

    /*void focusBrandsInvokeMethod()
    {
        GameObject.Find("CarouselBrandsNEW").transform.Find("_carouselManager").GetComponent<GCarouselController>().focusOnItem(2, 0);
    }
    void focusModelsInvokeMethod()
    {
        GameObject.Find("CarouselModelsNEW").transform.Find("_carouselManager").GetComponent<GCarouselController>().focusOnItem(2, 0);
    }*/

    public IEnumerator loadGlasses(string glassesName)
    {
        if (m_mainController.m_currentGlasses != null)
        {
            Destroy(m_mainController.m_currentGlasses);
        }
        print("Garik combine string: " + Path.Combine(m_currentModelsPath, glassesName.ToLower()));
        var bundleLoadRequest = AssetBundle.LoadFromFileAsync(Path.Combine(m_currentModelsPath, glassesName.ToLower()));

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

    void Update()
    {

    }
}
