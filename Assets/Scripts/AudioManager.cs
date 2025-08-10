using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Clips")]
    public AudioClip bgmLoop, coinPickup, goldenPickup, tick, gameOver, start;

    [Header("Sources")]
    public AudioSource musicSource;   
    public AudioSource sfxSource;     

    [Header("Volumes")]
    [Range(0f, 1f)] public float musicVolume = 0.25f; // softer music
    [Range(0f, 1f)] public float sfxVolume = 0.6f;   // louder SFX

    void Awake()
    {
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
            return; 
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Apply initial volumes
        if (musicSource != null) musicSource.volume = musicVolume;
        if (sfxSource != null) sfxSource.volume = sfxVolume;

        // Start background music
        if (musicSource != null && bgmLoop != null)
        {
            musicSource.clip = bgmLoop;
            musicSource.loop = true;
            musicSource.spatialBlend = 0f; // 2D
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning("[AudioManager] Missing Music Source or BGM clip.");
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null) 
        { 
            Debug.LogWarning("[AudioManager] SFX Source is not assigned."); 
            return; 
        }
        if (clip == null) return;

        sfxSource.volume = sfxVolume; 
        sfxSource.spatialBlend = 0f; 
        sfxSource.PlayOneShot(clip);
    }

    public void PlayStart()    => PlaySFX(start);
    public void PlayTick()     => PlaySFX(tick);
    public void PlayGameOver() => PlaySFX(gameOver);
    public void PlayCoin()     => PlaySFX(coinPickup);
    public void PlayGolden()   => PlaySFX(goldenPickup);

    // Optional: Call these from UI sliders
    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        if (musicSource != null) musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        if (sfxSource != null) sfxSource.volume = sfxVolume;
    }
}
