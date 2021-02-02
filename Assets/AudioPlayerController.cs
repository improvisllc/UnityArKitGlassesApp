using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AudioPlayerController : MonoBehaviour
{
    public AudioSource m_audioSource;

    void Start()
    {
        
    }

    public void setAudioClip(string path)
    {
        StartCoroutine("GetAudioClip");
    }


    string m_currentAudioPath = "";
    AudioClip m_currentAudioClip;

    public void playCurrentAudioClip()
    {
        m_audioSource.Play();
    }

    IEnumerator GetAudioClip()
    {
        //using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("http://www.my-server.com/audio.ogg", AudioType.OGGVORBIS))
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(m_currentAudioPath, AudioType.OGGVORBIS))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
                //m_currentAudioClip = myClip;
                m_audioSource.clip = myClip;
                playCurrentAudioClip();
                print("Garik aftercoroutine");
            }
        }
    }

    void Update()
    {
        
    }
}
