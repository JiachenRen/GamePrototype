
using EventSystem.Events;
using UnityEngine;

namespace Audio
{
    [System.Serializable]
    public class AudioClipGroup
    {
        public AudioSourceInfo audioSourceInfo;
        public AudioClip[] audioClips;

        public AudioClip GetRandomClip()
        {
            return audioClips[Random.Range(0, audioClips.Length)];
        }
    }
}
