using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip[] eventClips;
    private AudioSource eventAudioSource;
    public AudioSource newSource;
    public AudioSource currentAudioSource = new AudioSource();

    public string backgroundMusicName;
    public float fadeDuration = 1.5f;
    private bool isPlayingSource1 = true;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Set up the AudioSource
        eventAudioSource = gameObject.AddComponent<AudioSource>();
        eventAudioSource.loop = false;
        eventAudioSource.playOnAwake = false;
        eventAudioSource.spatialBlend = 0f; // 2D sound
        eventAudioSource.loop = true;
        eventAudioSource.volume = 0.7f;
    }

    public void PlayEventSound(string clipName)
    {
        AudioClip clip = GetEventClipByName(clipName);
        currentAudioSource = eventAudioSource;
        if (clip != null)
        {
            if (currentAudioSource.isPlaying == true)
            {
                StartCoroutine(FadeMusic(clip));
            }
            else
            {
                eventAudioSource.clip = clip;
                
                eventAudioSource.Play();
            }
            
        }
        else
        {
            Debug.LogWarning("Clip not found: " + clipName);
        }
    }
    private IEnumerator FadeMusic(AudioClip newClip)
    {
        Debug.Log(newClip.name);
        AudioSource activeSource;
        AudioSource otherSource;

        if (isPlayingSource1)
        {
            activeSource = eventAudioSource;
            otherSource = newSource;
        }
        else
        {
            activeSource = newSource;
            otherSource = eventAudioSource;
        }

        isPlayingSource1 = !isPlayingSource1;
        // Prepare the new AudioSource
        otherSource.clip = newClip;
        otherSource.volume = 0f;
        otherSource.Play();

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float fraction = timer / fadeDuration;

            activeSource.volume = Mathf.Lerp(1f, 0f, fraction);
            otherSource.volume = Mathf.Lerp(0f, 1f, fraction);

            yield return null;
        }

        // Ensure volumes are set correctly after fading
        activeSource.volume = 0f;
        activeSource.Stop();

        otherSource.volume = 1f;
        currentAudioSource = otherSource;

    }

    private AudioClip GetEventClipByName(string clipName)
    {
        foreach (AudioClip clip in eventClips)
        {
            if (clip.name == clipName)
                return clip;
        }
        return null;
    }

    public AudioSource GetEventAudioSource()
    {
        return eventAudioSource;
    }
}
