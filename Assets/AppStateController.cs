using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class AppStateController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    /*void Update()
    {
        print("Garik m_isPaused: " + m_isPaused);
    }*/

    bool m_isPaused = false;

    /*void OnApplicationFocus(bool hasFocus)
    {
        Debug.Log("Garik hasFocus " + hasFocus);
        m_isPaused = !hasFocus;
    }*/

    void OnApplicationPause(bool pauseStatus)
    {
        //Debug.Log("Garik pauseStatus " + pauseStatus);
        m_isPaused = pauseStatus;

        if (pauseStatus)
        {
            GameObject.Find("VideoPlayer").GetComponent<VideoPlayer>().Stop();
        }
        else
        {
            if (GameObject.Find("_manager").GetComponent<UIController>().m_videoOutputRawimage.gameObject.activeInHierarchy)
            {
                GameObject.Find("VideoPlayer").GetComponent<VideoPlayer>().Play();
            }
        }
    }
}
