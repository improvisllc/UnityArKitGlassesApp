using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using pmjo.NextGenRecorder;
using pmjo.NextGenRecorder.Sharing;
using System.Runtime.InteropServices;
using UnityEngine.Video;

namespace pmjo.Examples
{
    public class SimpleRecorder : MonoBehaviour
    {
        public Button startRecordingButton;
        public Button stopRecordingButton;
        public Button saveRecordingButton;
        public Button viewRecordingButton;

        //public AudioPlayerController m_audioPlayerController;

        private long mLastSessionId;

        void OnEnable()
        {
            Recorder.RecordingStarted += RecordingStarted;
            Recorder.RecordingPaused += RecordingPaused;
            Recorder.RecordingResumed += RecordingResumed;
            Recorder.RecordingStopped += RecordingStopped;
            Recorder.RecordingExported += RecordingExported;
        }

        void OnDisable()
        {
            Recorder.RecordingStarted -= RecordingStarted;
            Recorder.RecordingPaused -= RecordingPaused;
            Recorder.RecordingResumed -= RecordingResumed;
            Recorder.RecordingStopped -= RecordingStopped;
            Recorder.RecordingExported -= RecordingExported;
        }

        void Awake()
        {
            mLastSessionId = Recorder.GetLastRecordingSession();
            UpdateStartAndStopRecordingButton();
            UpdateSaveOrViewRecordingButton();
        }

        void Start()
        {
            //string videoClipName = GameObject.Find("VideoPlayer").GetComponent<VideoPlayer>().clip.name;
            //print("Garik Video Clip Name: " + videoClipName);
            CreateEventSystemIfItDoesNotExist();
        }

        void RecordingStarted(long sessionId)
        {
            Debug.Log("Recording started, session id " + sessionId);

            UpdateStartAndStopRecordingButton();
            UpdateSaveOrViewRecordingButton();
        }

        void RecordingPaused(long sessionId)
        {
            Debug.Log("Recording paused, session id " + sessionId);
        }

        void RecordingResumed(long sessionId)
        {
            Debug.Log("Recording resumed, session id " + sessionId);
        }

        void RecordingStopped(long sessionId)
        {
            Debug.Log("Recording stopped, session id " + sessionId);

            mLastSessionId = sessionId;

            UpdateStartAndStopRecordingButton();
            UpdateSaveOrViewRecordingButton();
            ExportLastRecording();
        }
        bool m_isAudioRecStarted = false;
        public void StartRecording()
        {
            Recorder.StartRecording();
            if (!m_isAudioRecStarted)
            {
                //GameObject.Find("_manager").GetComponent<AudioPlayerController>().startMicrophoneRecording();
                NativeInfoProvider.startAudioRecording();
                m_isAudioRecStarted = true;
            }
        }

        public void StopRecording()
        {
            Recorder.StopRecording();

            if (m_isAudioRecStarted)
            {
                NativeInfoProvider.stopAudioRecording();
                m_isAudioRecStarted = false;
            }
        }

        public  void ExportLastRecording()
        {
            if (mLastSessionId > 0)
            {
                Recorder.ExportRecordingSession(mLastSessionId);
            }
        }

        string m_currentVideoPath = "";
        public void saveVideoToGallery()
        {
            char[] p = m_currentVideoPath.ToCharArray();
            NativeInfoProvider.mergeVideoWithAudio(p);
            while (true)
            {
                int merged = NativeInfoProvider.isMergedVideoWithAudio();
                if (merged == 1)
                {
                    Debug.Log("Garik already merged: " + "Save Video");
                    NativeInfoProvider.saveVideo(p);
                    break;
                }
                else if (merged == 0)
                {
                    Debug.Log("Garik While Loop: " + merged);
                }

            }
            //Sharing.SaveToPhotos(m_currentVideoPath, "Glassee");
        }
        public void shareRecordedVideo()
        {
            Sharing.ShowShareSheet(m_currentVideoPath, true);
        }
        void RecordingExported(long sessionId, string path, Recorder.ErrorCode errorCode)
        {
            if (errorCode == Recorder.ErrorCode.NoError)
            {
                m_currentVideoPath = path;
                Debug.Log("Recording exported to " + path + ", session id " + sessionId);
                char[] p = m_currentVideoPath.ToCharArray();
                NativeInfoProvider.mergeVideoWithAudio(p);
                while (true)
                {
                    int merged = NativeInfoProvider.isMergedVideoWithAudio();
                    if (merged == 1)
                    {
                        Debug.Log("Garik already merged: " + "Play Video");
                        NativeInfoProvider.changeSpeakerConfigurationToDefault();
                        PlayVideo(path);
                        break;
                    }
                    else if(merged == 0)
                    {
                        Debug.Log("Garik While Loop: " + merged);
                    }

                }
                //PlayVideo(path);
                //StartCoroutine(playVideoCoroutine(path));
                //Sharing.SaveToPhotos(path, "Glassee");
                //Sharing.ShowShareSheet(path, true);
            }
            else
            {
                Debug.Log("Failed to export recording, error code " + errorCode + ", session id " + sessionId);
            }
        }

        private void UpdateStartAndStopRecordingButton()
        {
            if (Recorder.IsSupported)
            {
                startRecordingButton.interactable = true;
                stopRecordingButton.interactable = true;

                startRecordingButton.gameObject.SetActive(!Recorder.IsRecording);
                stopRecordingButton.gameObject.SetActive(Recorder.IsRecording);
            }
            else
            {
                startRecordingButton.gameObject.SetActive(true);
                startRecordingButton.interactable = false;
                stopRecordingButton.gameObject.SetActive(false);
            }
        }

        public void UpdateSaveOrViewRecordingButton()
        {
            viewRecordingButton.gameObject.SetActive(true);
            viewRecordingButton.interactable = (mLastSessionId > 0) && !Recorder.IsRecording;
            //saveRecordingButton.gameObject.SetActive(false);
        }


        IEnumerator playVideoCoroutine(string path)
        {
            yield return new WaitForSeconds(5);
            PlayVideo(path);
            yield return null;
        }
        private static void PlayVideo(string path)
        {
            if (!path.Contains("file://"))
            {
                path = "file://" + path;
            }
            Debug.Log("Garik Path For Meri 007: " + path);
            //NativeInfoProvider.playAudio();
            //GameObject.Find("_manager").GetComponent<VideoPlayerController>().initializeVideoPlayer(path);
            //GameObject.Find("_manager").GetComponent<VideoPlayerController>().loadVideoFromThisURL(path);
            //GameObject.Find("_manager").GetComponent<AudioPlayerController>().stopMicrophoneRecording();
            //GameObject.Find("_manager").GetComponent<AudioPlayerController>().playLastAudioRecording();


            GameObject.Find("_manager").GetComponent<UIController>().showVideoOutputRawimage();
            GameObject.Find("_manager").GetComponent<VideoPlayerController>().setAndPlayCapturedVideo(path);

            print("Garik after initializeVideoPlayer");
            //Handheld.PlayFullScreenMovie(path);
        }

        private static void CreateEventSystemIfItDoesNotExist()
        {
            EventSystem eventSystem = FindObjectOfType<EventSystem>();
            StandaloneInputModule inputModule = FindObjectOfType<StandaloneInputModule>();

            // Create EventSystem if it does not exist
            if (eventSystem == null && inputModule == null)
            {
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }
        }
    }
}
