using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    public float MasterVolume
    {
        get { return PlayerPrefs.GetFloat("MasterVolume", 1f); }
        set
        {
            PlayerPrefs.SetFloat("MasterVolume", value);
            ApplyVolumes();
        }
    }

    public float MusicVolume
    {
        get { return PlayerPrefs.GetFloat("MusicVolume", 0.7f); }
        set
        {
            PlayerPrefs.SetFloat("MusicVolume", value);
            ApplyVolumes();
        }
    }

    public float SfxVolume
    {
        get { return PlayerPrefs.GetFloat("SfxVolume", 1f); }
        set { PlayerPrefs.SetFloat("SfxVolume", value); }
    }

    public bool MusicOn
    {
        get { return PlayerPrefs.GetInt("MusicOn", 1) == 1; }
        set
        {
            PlayerPrefs.SetInt("MusicOn", value ? 1 : 0);
            ApplyVolumes();
        }
    }

    private void Awake()
    {
        Instance = this;
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
        }
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }
        ApplyVolumes();
    }

    private void ApplyVolumes()
    {
        AudioListener.volume = MasterVolume;
        musicSource.volume = MusicVolume;
        musicSource.mute = !MusicOn;
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null || musicSource.clip == clip)
        {
            return;
        }
        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySfx(AudioClip clip, float volume = 1f)
    {
        if (clip == null)
        {
            return;
        }
        sfxSource.PlayOneShot(clip, volume * SfxVolume);
    }
}
