using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    GlassesProvider m_glassesProvider = new GlassesProvider();
    MainController m_mainController = new MainController();
    void Start()
    {
        m_glassesProvider = GameObject.Find("_manager").GetComponent<GlassesProvider>();
        m_mainController = GameObject.Find("_manager").GetComponent<MainController>();

        print("Garik name: " + m_glassesProvider.m_glasses[m_currentGlassesIndex]);

        m_mainController.setGlasses(m_glassesProvider.m_glasses[m_currentGlassesIndex]);
    }

    int m_currentGlassesIndex = 0;
    public void nextGlasses()
    {
        if(m_currentGlassesIndex > m_glassesProvider.m_glasses.Count-2)
        {
            return; 
        }
        m_glassesProvider.m_glasses[m_currentGlassesIndex].SetActive(false);
        m_glassesProvider.m_glasses[m_currentGlassesIndex].transform.SetParent(GameObject.Find("GlassesContainer").transform);
        m_glassesProvider.m_glasses[m_currentGlassesIndex].transform.localScale = Vector3.one;
        m_currentGlassesIndex++;
        m_mainController.setGlasses(m_glassesProvider.m_glasses[m_currentGlassesIndex]);
    }

    public void prevGlasses()
    {
        if (m_currentGlassesIndex < 1)
        {
            return;
        }
        m_glassesProvider.m_glasses[m_currentGlassesIndex].SetActive(false);
        m_glassesProvider.m_glasses[m_currentGlassesIndex].transform.SetParent(GameObject.Find("GlassesContainer").transform);
        m_glassesProvider.m_glasses[m_currentGlassesIndex].transform.localScale = Vector3.one;
        m_currentGlassesIndex--;
        m_mainController.setGlasses(m_glassesProvider.m_glasses[m_currentGlassesIndex]);
    }

    void Update()
    {
        
    }
}
