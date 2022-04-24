using UnityEngine;

namespace Audio
{
    [System.Serializable]
    public class AudioClipGroup
    {
        [Range(0, 1)]
        public float volumeScale = 1f;
        public AudioSourceInfo audioSourceInfo;
        public AudioClip[] audioClips;

        public AudioClip GetRandomClip()
        {
            return audioClips[Random.Range(0, audioClips.Length)];
        }
    }
}
