using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public AudioMixer audioMixer;

    float musicLevelPercent = 1f;
    public float MusicLevelPercent
    {
        get
        {
            return musicLevelPercent;
        }
        set
        {
            musicLevelPercent = Mathf.Clamp01(value);
            SetMusicVolume(musicLevelPercent * musicLevelMax);
        }
    }

    float soundLevelPercent = 1f;
    public float SoundLevelPercent
    {
        get
        {
            return soundLevelPercent;
        }
        set
        {
            soundLevelPercent = Mathf.Clamp01(value);
            SetSoundVolume(soundLevelPercent * soundLevelMax);
        }
    }

    float musicLevelMax = 1f;
    public float MusicLevelMax
    {
        get
        {
            return musicLevelMax;
        }
        set
        {
            musicLevelMax = Mathf.Clamp01(value);
            SetMusicVolume(musicLevelPercent * musicLevelMax);
        }
    }

    float soundLevelMax = 1f;
    public float SoundLevelMax
    {
        get
        {
            return soundLevelMax;
        }
        set
        {
            soundLevelMax = Mathf.Clamp01(value);
            SetSoundVolume(soundLevelPercent * soundLevelMax);
        }
    }

    public AudioSource audioSource;
    public AudioClip[] footstepClips;
    public AudioClip[] dialogueClips;
    public AudioClip itemPickupClip;
    public AudioClip goalCollectClip;
    public AudioClip shootClip;
    public AudioClip toggleClip;

    static AudioManager instance;

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

    void SetMusicVolume(float decimalVolume)
    {
        float dbVolume = Mathf.Log10(decimalVolume) * 20f;
        dbVolume = Mathf.Clamp(dbVolume, -80f, 0f);
        audioMixer.SetFloat("MusicVol", dbVolume);
    }
    
    void SetSoundVolume(float decimalVolume)
    {
        float dbVolume = Mathf.Log10(decimalVolume) * 20f;
        dbVolume = Mathf.Clamp(dbVolume, -80f, 0f);
        audioMixer.SetFloat("SoundVol", dbVolume);
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

    public void PlaySwitchToggle()
    {
        if (toggleClip != null) audioSource.PlayOneShot(toggleClip, 1.0f);
    }
}