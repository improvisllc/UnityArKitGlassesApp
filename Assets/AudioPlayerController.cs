using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading;
using System.IO;

public class AudioPlayerController : MonoBehaviour
{
    public AudioSource m_audioSource;

    void Start()
    {

    }

    void Update()
    {
        
    }

    AudioClip m_recording;

    private float startRecordingTime;


        /*
    public void stopMicrophoneRecording()
    {

        Debug.Log("Garik stopMicrophoneRecording 0");
        //End the recording when the mouse comes back up, then play it
        Microphone.End("");
        Debug.Log("Garik stopMicrophoneRecording 1");
        //Trim the audioclip by the length of the recording
        AudioClip recordingNew = AudioClip.Create(m_recording.name, (int)((Time.time - startRecordingTime) * m_recording.frequency), m_recording.channels, m_recording.frequency, false);
        Debug.Log("Garik stopMicrophoneRecording 2");
        float[] data = new float[(int)((Time.time - startRecordingTime) * m_recording.frequency)];
        Debug.Log("Garik stopMicrophoneRecording 3");
        m_recording.GetData(data, 0);
        recordingNew.SetData(data, 0);
        Debug.Log("Garik stopMicrophoneRecording 4");
        this.m_recording = recordingNew;
        //Play recording
        Debug.Log("Garik stopMicrophoneRecording 5");
        //m_audioSource.clip = m_recording;
        //m_audioSource.Play();
    }

    public void playLastAudioRecording()
    {
        //Play recording
        m_audioSource.clip = m_recording;
        m_audioSource.Play();
    }


    public void startMicrophoneRecording()
    {
        Debug.Log("Garik startMicrophoneRecording 0");
        m_recording = null;
        m_audioSource = null;
        Debug.Log("Garik startMicrophoneRecording 1");
        //Get the max frequency of a microphone, if it's less than 44100 record at the max frequency, else record at 44100
        int minFreq;
        int maxFreq;
        int freq = 44100;
        Debug.Log("Garik startMicrophoneRecording 2");
        Microphone.GetDeviceCaps("", out minFreq, out maxFreq);
        Debug.Log("Garik startMicrophoneRecording 3");
        if (maxFreq < 44100)
        {
            Debug.Log("Garik startMicrophoneRecording 4");
            freq = maxFreq;
        }

        Debug.Log("Garik startMicrophoneRecording 5");
        //Start the recording, the length of 300 gives it a cap of 5 minutes
        m_recording = Microphone.Start("", false, 300, 44100);
        startRecordingTime = Time.time;

        Debug.Log("Garik startMicrophoneRecording 6");
    }*/


}
