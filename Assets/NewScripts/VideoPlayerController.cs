using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{

    public VideoPlayer m_videoPlayerComponent;
    public RenderTexture m_renderTexture;
    public GameObject m_videoPlayerObject;
    public AudioSource m_audioSource;

    public void setAndPlayCapturedVideo(string url)
    {
        StartCoroutine(loadVideoFromThisURL(url));
        return;
        print("Garik setVideoPlayerUrl call" + url);
        m_videoPlayerComponent.url = url;
        m_videoPlayerComponent.Play();
    }

   /* public void initializeVideoPlayer(string url)
    {

        Debug.Log("Garik initializeVideoPlayer 000");
        DestroyImmediate(m_videoPlayerComponent);
        m_videoPlayerComponent = m_videoPlayerObject.AddComponent<VideoPlayer>();
        m_videoPlayerComponent.playOnAwake = false;
        m_videoPlayerComponent.source = VideoSource.Url;
        m_videoPlayerComponent.targetTexture = m_renderTexture;
        m_videoPlayerComponent.audioOutputMode = VideoAudioOutputMode.AudioSource;
        m_videoPlayerComponent.url = url;
        print("Garik name of video: " + m_videoPlayerComponent.clip.name);
        m_videoPlayerComponent.SetTargetAudioSource(0, m_audioSource);
        m_videoPlayerComponent.Play();
        //print("Garik name of video: " + m_videoPlayerComponent.clip.name);
        print("Garik length of video: " + m_videoPlayerComponent.clip.length);



    }*/

    public IEnumerator loadVideoFromThisURL(string _url)
    {
        UnityWebRequest _videoRequest = UnityWebRequest.Get(_url);
        yield return _videoRequest.SendWebRequest();
        if (_videoRequest.isDone == false || _videoRequest.error != null)
        {
            yield return null;
        }
        else
        {
            byte[] _videoBytes = _videoRequest.downloadHandler.data;
            string _pathToFile = Path.Combine(Application.persistentDataPath, "video.mp4");
            File.WriteAllBytes(_pathToFile, _videoBytes);
            StartCoroutine(this.playVideoInThisURL(_pathToFile));
            yield return null;
        }
    }
    private IEnumerator playVideoInThisURL(string _url)
    {
        m_videoPlayerComponent.source = UnityEngine.Video.VideoSource.Url;
        m_videoPlayerComponent.url = _url;
        m_videoPlayerComponent.audioOutputMode = VideoAudioOutputMode.AudioSource;
        m_videoPlayerComponent.EnableAudioTrack(0, true);
        m_videoPlayerComponent.SetTargetAudioSource(0, m_audioSource);
        m_videoPlayerComponent.controlledAudioTrackCount = 1;
        m_videoPlayerComponent.Prepare();
        while (m_videoPlayerComponent.isPrepared == false)
        {
           yield return null;
        }
        m_videoPlayerComponent.targetTexture = m_renderTexture;
        m_audioSource.Play();
        m_videoPlayerComponent.Play();
    }



    /*

    IEnumerator playVideo(string url)
    {

        //Disable Play on Awake for both Video and Audio
        m_videoPlayerComponent.playOnAwake = true;
        //m_audioSource.playOnAwake = true;

        //We want to play from video clip not from url
        m_videoPlayerComponent.source = VideoSource.Url;

        //Set Audio Output to AudioSource
        m_videoPlayerComponent.audioOutputMode = VideoAudioOutputMode.Direct;

        //Assign the Audio from Video to AudioSource to be played
        m_videoPlayerComponent.EnableAudioTrack(0, true);
        m_videoPlayerComponent.SetTargetAudioSource(0, m_audioSource);

        //Set video To Play then prepare Audio to prevent Buffering
        //m_videoPlayerComponent.clip = videoToPlay;
        m_videoPlayerComponent.url = url;
        m_videoPlayerComponent.Prepare();

        //Wait until video is prepared
        while (!m_videoPlayerComponent.isPrepared)
        {
            Debug.Log("Preparing Video");
            yield return null;
        }

        Debug.Log("Done Preparing Video");

        //Assign the Texture from Video to RawImage to be displayed
        //image.texture = m_videoPlayerComponent.texture;
        //m_audioSource.clip = m_videoPlayerComponent.GetTargetAudioSource(0).clip;
        //Play Video
        m_videoPlayerComponent.Play();

        //Play Sound
        //m_audioSource.Play();

        Debug.Log("Playing Video");
        while (m_videoPlayerComponent.isPlaying)
        {
            Debug.LogWarning("Video Time: " + Mathf.FloorToInt((float)m_videoPlayerComponent.time));
            yield return null;
        }

        Debug.Log("Done Playing Video");
    }


    */



    void Awake()
    {
        //m_renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
