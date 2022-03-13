using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public AudioClip[] clips;
    public AudioClip attackSound;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void PlayerStep()
    {
        var clip = GetRandomClip();
        audioSource.PlayOneShot(clip);
    }

    private void PlayerAttack()
    {
        audioSource.PlayOneShot(attackSound);
    }

    private AudioClip GetRandomClip()
    {
        return clips[Random.Range(0, clips.Length)];
    }
}