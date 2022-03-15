using System;
using System.Collections.Generic;
using Audio;
using EventSystem.Events;
using EventSystem.Events.UI;
using UnityEngine;

namespace EventSystem
{
    public class AudioEventManager : MonoBehaviour
    {
        public AudioClip buttonClickDownAudio;
        public AudioClip buttonClickUpAudio;
        public AudioClip buttonEnterAudio;
        public AudioClip buttonExitAudio;

        public AudioClipGroup[] groupedAudioClips;

        private Dictionary<String, AudioClipGroup> audioDict;

        private AudioSource audioSource;

        private static AudioEventManager _instance;
        
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            audioSource = GetComponent<AudioSource>();

            audioDict = new Dictionary<string, AudioClipGroup>();
            foreach (var clipGroup in groupedAudioClips)
            {
                audioDict[clipGroup.audioSourceInfo.ToString()] = clipGroup;
            }

            DontDestroyOnLoad(gameObject);

            EventManager.StartListening<ButtonClickDownEvent>(OnButtonClickDown);
            EventManager.StartListening<ButtonClickUpEvent>(OnButtonClickUp);
            EventManager.StartListening<ButtonEnterEvent>(OnButtonEnter);
            EventManager.StartListening<ButtonExitEvent>(OnButtonExit);
            
            EventManager.StartListening<AudioEvent, AudioSourceInfo, AudioSource>(OnAudioEvent);
        }

        private void OnButtonClickDown()
        {
            if (buttonClickDownAudio == null) return;
            audioSource.PlayOneShot(buttonClickDownAudio);
        }
        
        private void OnButtonClickUp()
        {
            if (buttonClickUpAudio == null) return;
            audioSource.PlayOneShot(buttonClickUpAudio);
        }

        private void OnButtonEnter()
        {
            if (buttonEnterAudio == null) return;
            audioSource.PlayOneShot(buttonEnterAudio);
        }

        private void OnButtonExit()
        {
            if (buttonExitAudio == null) return;
            audioSource.PlayOneShot(buttonExitAudio);
        }

        private void OnAudioEvent(AudioSourceInfo info, AudioSource source)
        {
            var clipGroup = audioDict[info.ToString()];
            if (clipGroup != null)
            {
                source.PlayOneShot(clipGroup.GetRandomClip());
            }
        }
    }
}