using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    

    public AudioClip[] eventClips;
    private AudioSource eventAudioSource;
    public AudioSource newSource;
    AudioSource currentAudioSource;

    AudioClip backgroundMusic;
    [SerializeField] AudioClip startingMusic;

    public float fadeDuration = 1.5f;
    bool isPlayingSource1 = true;

    Dictionary<string, AudioClip> audioClipMap = new Dictionary<string, AudioClip>();

    #region  Singleton
    public static AudioManager instance;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("More than one instance of AudioManager found!");
            Destroy(gameObject);
            return;
        }
        instance = this;
        // Set up the AudioSource
        eventAudioSource = gameObject.AddComponent<AudioSource>();
        eventAudioSource.loop = false;
        eventAudioSource.playOnAwake = false;
        eventAudioSource.spatialBlend = 0f; // 2D sound
        eventAudioSource.loop = true;
        eventAudioSource.volume = 0.5f;
        currentAudioSource = eventAudioSource;
    }
    #endregion

    void Start()
    {
        backgroundMusic = startingMusic;
        foreach (AudioClip item in eventClips)
        {
            if (audioClipMap.ContainsKey(item.name) == false)
            {
                audioClipMap.Add(item.name, item);

            }
        }
    }

    public void PlayEventSound(string clipName)
    {
        AudioClip clip = GetEventClipByName(clipName);
        
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
        currentAudioSource.loop = true;

    }

    private AudioClip GetEventClipByName(string clipName)
    {
        if (audioClipMap.ContainsKey(clipName))
            return audioClipMap[clipName];
        Debug.LogError("Clip Map does not contain this key: " + clipName);
        return null;
    }

    public AudioSource GetEventAudioSource()
    {
        return eventAudioSource;
    }
    public AudioClip GetBackgroundMusic()
    {
        return backgroundMusic;
    }
    public void SetBackgroundMusic(AudioClip newBackground)
    {
        backgroundMusic = newBackground;
        
    }
}
