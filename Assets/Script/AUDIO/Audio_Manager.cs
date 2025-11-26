using UnityEngine;

public class Audio_Manager : MonoBehaviour
{
    public static Audio_Manager Instance { get; private set; }

    public AudioSource BMGSource;
    public AudioSource SFXSource;

    [Range(0f, 1f)]
    public float BGMVolume = 1f; // Volume awal BGM
    [Range(0f, 1f)]
    public float SFXVolume = 1f; // Volume awal SFX

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Setting default AudioSource
        BMGSource.playOnAwake = true;
        BMGSource.loop = true;
        BMGSource.volume = BGMVolume;

        SFXSource.playOnAwake = false;
        SFXSource.loop = false;
        SFXSource.volume = SFXVolume;
    }

    // Fungsi untuk memutar SFX saat klik button
    public void OnClick_BtnSFX()
    {
        if (SFXSource != null)
            SFXSource.Play();
    }

    // Fungsi untuk mengatur volume BGM dari slider
    public void SetBGMVolume(float volume)
    {
        BGMVolume = Mathf.Clamp01(volume);
        if (BMGSource != null)
            BMGSource.volume = BGMVolume;
    }

    // Fungsi untuk mengatur volume SFX dari slider
    public void SetSFXVolume(float volume)
    {
        SFXVolume = Mathf.Clamp01(volume);
        if (SFXSource != null)
            SFXSource.volume = SFXVolume;
    }
}
