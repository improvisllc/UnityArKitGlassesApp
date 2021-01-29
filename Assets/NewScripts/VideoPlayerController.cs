using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{


    public VideoPlayer m_videoPlayer;

    public void setVideoPlayerUrl(string url)
    {
        print("Garik video play111111" + url);
        m_videoPlayer.url = url;
        m_videoPlayer.Play();
        print("Garik video play111111" + url);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
