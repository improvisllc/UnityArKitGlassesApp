using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class GlassesProvider : MonoBehaviour
{
    public List<GameObject> m_glasses = new List<GameObject>();

    GameObject m_glassesContainer;

    void Awake()
    {
        m_glassesContainer = GameObject.Find("GlassesContainer");
        initializeGlassesList();

    }

    void initializeGlassesList()
    {
        DirectoryInfo d = new DirectoryInfo(Application.streamingAssetsPath);

        int c = 0;
        FileInfo[] fis = d.GetFiles();
        foreach (FileInfo fi in fis)
        {
            if (fi.ToString().Contains("glasses") && !fi.Extension.Contains("meta"))
            {
                c++;
                string s = fi.ToString();
                string myLastWord = s.Split('/').Last();
                var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, myLastWord));
                if (myLoadedAssetBundle == null)
                {
                    Debug.Log("Failed to load AssetBundle!");
                    return;
                }

                var prefab = myLoadedAssetBundle.LoadAsset<GameObject>(myLastWord);
                GameObject g = Instantiate(prefab);
                g.name = myLastWord;
                g.SetActive(false);
                g.transform.SetParent(m_glassesContainer.transform);
                myLoadedAssetBundle.Unload(false);
            }

       }

        for (int i = 0; i < m_glassesContainer.transform.childCount; i++)
        {
            m_glasses.Add(m_glassesContainer.transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        
    }
}
