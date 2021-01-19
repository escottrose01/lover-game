using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static AudioManager instance;

    public AudioSource audioSource;
    public AudioClip[] footstepClips;
    public AudioClip[] dialogueClips;
    public AudioClip itemPickupClip;
    public AudioClip goalCollectClip;
    public AudioClip shootClip;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject tmp = new GameObject
                {
                    name = "AudioManager"
                };
                instance = tmp.AddComponent<AudioManager>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySound(AudioClip audioClip)
    {
        if (audioClip != null) audioSource.PlayOneShot(audioClip);
    }

    public void PlaySound(AudioClip audioClip, float volumeScale)
    {
        if (audioClip != null) audioSource.PlayOneShot(audioClip, volumeScale);
    }

    public void PlayItemPickup()
    {
        if (itemPickupClip != null) audioSource.PlayOneShot(itemPickupClip, 0.25f);
    }

    public void PlayFootstep()
    {
        if (footstepClips != null && footstepClips.Length > 0) audioSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length - 1)], 0.25f);
    }

    public void PlayQuietFootstep()
    {
        if (footstepClips != null && footstepClips.Length > 0) audioSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length - 1)], 0.1f);
    }

    public void PlayLoudFootstep()
    {
        if (footstepClips != null && footstepClips.Length > 0) audioSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length - 1)], 0.5f);
    }

    public void PlayDialogue()
    {
        if (dialogueClips != null && dialogueClips.Length > 0) audioSource.PlayOneShot(dialogueClips[Random.Range(0, dialogueClips.Length - 1)], 0.1f);
    }

    public void PlayGoalCollect()
    {
        if (goalCollectClip != null) audioSource.PlayOneShot(goalCollectClip, 0.25f);
    }

    public void PlayShoot()
    {
        if (shootClip != null) audioSource.PlayOneShot(shootClip, 0.25f);
    }
}